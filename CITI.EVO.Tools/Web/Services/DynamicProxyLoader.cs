using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Binding = System.ServiceModel.Channels.Binding;

namespace CITI.EVO.Tools.Web.Services
{
    public class DynamicProxyLoader : IServiceAssemblyLoader
    {
        private const String defaultNamespace = "http://tempuri.org/";

        private readonly CodeCompileUnit codeCompileUnit;
        private readonly CodeDomProvider codeDomProvider;
        private readonly WebReferenceOptions webReferenceOptions;
        private readonly ServiceContractGenerator contractGenerator;

        public DynamicProxyLoader(DiscoveryClientProtocol discoveryClient, String @namespace, DynamicProxyLoaderOptions options)
            : this(discoveryClient, null, null, null, @namespace, options)
        {
        }

        public DynamicProxyLoader(DiscoveryClientProtocol discoveryClient, Assembly assembly, byte[] assemblyBytes, String assemblyCode, String @namespace, DynamicProxyLoaderOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            Options = options;

            if (!String.IsNullOrEmpty(@namespace))
            {
                if (!options.Namespaces.ContainsKey("*"))
                    options.Namespaces.Add("*", @namespace);
                else
                    options.Namespaces["*"] = @namespace;
            }

            DiscoveryClient = discoveryClient;

            AssemblyBytes = assemblyBytes;
            AssemblyCode = assemblyCode;
            Assembly = assembly;

            codeDomProvider = CodeDomProvider.CreateProvider(options.Language.ToString());
            codeCompileUnit = new CodeCompileUnit();

            contractGenerator = new ServiceContractGenerator(codeCompileUnit);
            contractGenerator.Options |= ServiceContractGenerationOptions.ClientClass;

            webReferenceOptions = new WebReferenceOptions();

            DownloadMetadata();
            ImportMetadata();
            CreateProxy();

            if (String.IsNullOrEmpty(assemblyCode))
                GenerateProxyCode();

            if (assembly == null && assemblyBytes != null)
                Assembly = Assembly.Load(assemblyBytes);
            else if (assembly != null && assemblyBytes == null)
                AssemblyBytes = File.ReadAllBytes(Assembly.Location);
            else if (Assembly == null && assemblyBytes == null)
                CompileProxyCode();

            ServiceContracts = new List<Type>();
            foreach (var contractDescription in ContractDescriptions)
            {
                var serviceContract = Assembly.GetType(contractDescription.Name);
                if (serviceContract != null)
                {
                    ServiceContracts.Add(serviceContract);

                    if (serviceContract.Name == ContractDescription.Name)
                        ServiceContract = serviceContract;
                }
            }
        }

        public DiscoveryClientProtocol DiscoveryClient { get; private set; }
        public Collection<MetadataSection> Metadata { get; private set; }
        public ServiceEndpointCollection Endpoints { get; private set; }
        public IEnumerable<Binding> Bindings { get; private set; }

        public byte[] AssemblyBytes { get; private set; }
        public String AssemblyCode { get; private set; }
        public Assembly Assembly { get; private set; }

        public DynamicProxyLoaderOptions Options { get; private set; }

        public IList<MetadataConversionError> MetadataImportWarnings { get; private set; }
        public IList<MetadataConversionError> CodeGenerationWarnings { get; private set; }
        public IList<CompilerError> CompilationWarnings { get; private set; }

        public IList<ContractDescription> ContractDescriptions { get; private set; }
        public IList<Type> ServiceContracts { get; private set; }


        public ContractDescription ContractDescription { get; private set; }
        public Type ServiceContract { get; private set; }

        public IList<Type> DataContracts { get; private set; }
        public IList<MethodInfo> Methods { get; private set; }

        private void DownloadMetadata()
        {
            var results = new Collection<MetadataSection>();
            if (DiscoveryClient.Documents.Values != null)
            {
                foreach (var document in DiscoveryClient.Documents.Values)
                {
                    var wsdl = document as System.Web.Services.Description.ServiceDescription;
                    var schema = document as XmlSchema;
                    var xmlDoc = document as XmlElement;

                    if (wsdl != null)
                    {
                        var metaSect = MetadataSection.CreateFromServiceDescription(wsdl);
                        results.Add(metaSect);
                    }
                    else if (schema != null)
                        results.Add(MetadataSection.CreateFromSchema(schema));
                    else if (xmlDoc != null && xmlDoc.LocalName == "Policy")
                        results.Add(MetadataSection.CreateFromPolicy(xmlDoc, null));
                    else
                    {
                        var mexDoc = new MetadataSection();
                        mexDoc.Metadata = document;
                        results.Add(mexDoc);
                    }
                }
            }

            Metadata = results;
        }

        private void ImportMetadata()
        {
            var metadataSet = new MetadataSet(Metadata);
            var importer = new WsdlImporter(metadataSet);

            AddStateForDataContractSerializerImport(importer);
            AddStateForXmlSerializerImport(importer);
            AddStateForFaultSerializerImport(importer);
            AddStateForWrappedOptions(importer);

            Bindings = importer.ImportAllBindings();
            ContractDescriptions = importer.ImportAllContracts();

            Endpoints = importer.ImportAllEndpoints();
            MetadataImportWarnings = importer.Errors;

            ConfigureEndpoints(Endpoints);

            bool success = true;
            if (MetadataImportWarnings != null)
            {
                foreach (var error in MetadataImportWarnings)
                {
                    if (!error.IsWarning)
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (!success)
            {
                var exception = new DynamicProxyException(DynamicErrorMessages.ImportError);
                exception.MetadataImportErrors = MetadataImportWarnings;

                throw exception;
            }
        }

        private void ConfigureEndpoints(IEnumerable<ServiceEndpoint> endpoints)
        {
            //HttpBinding: maxBufferSize="33554432" maxReceivedMessageSize="33554432"
            //ReaderQuotas: maxStringContentLength="33554432" maxArrayLength="33554432" 

            foreach (var endpoint in endpoints)
            {
                var dataContractBehavior = endpoint.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dataContractBehavior != null)
                    dataContractBehavior.MaxItemsInObjectGraph = (int)Math.Pow(2, 31) - 1;

                var binding = endpoint.Binding;

                binding.OpenTimeout = TimeSpan.FromMinutes(10);
                binding.SendTimeout = TimeSpan.FromMinutes(10);
                binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
                binding.CloseTimeout = TimeSpan.FromMinutes(10);

                if (endpoint.Binding is BasicHttpBinding)
                {
                    var basicHttpBinding = (BasicHttpBinding)endpoint.Binding;
                    ConfigureBasicHttpBinding(basicHttpBinding);
                }

                if (endpoint.Binding is WSHttpBinding)
                {
                    var wsHttpBinding = (WSHttpBinding)endpoint.Binding;
                    ConfigureWsHttpBinding(wsHttpBinding);
                }

                if (endpoint.Binding is CustomBinding)
                {
                    var customBinding = (CustomBinding)endpoint.Binding;
                    ConfigureCustomHttpBinding(customBinding);
                }
            }
        }

        private static void ConfigureCustomHttpBinding(CustomBinding binding)
        {
            foreach (var bindingElement in binding.Elements)
            {
                if (bindingElement is BinaryMessageEncodingBindingElement)
                {
                    var binaryBindingElement = (BinaryMessageEncodingBindingElement)bindingElement;
                    var readerQuotas = binaryBindingElement.ReaderQuotas;
                    if (readerQuotas != null)
                    {
                        readerQuotas.MaxArrayLength = int.MaxValue;
                        readerQuotas.MaxStringContentLength = int.MaxValue;
                    }
                }

                if (bindingElement is HttpTransportBindingElement)
                {
                    var httpBindingElement = (HttpTransportBindingElement)bindingElement;

                    httpBindingElement.AllowCookies = true;
                    httpBindingElement.DecompressionEnabled = true;

                    httpBindingElement.MaxBufferSize = int.MaxValue;
                    httpBindingElement.MaxBufferPoolSize = int.MaxValue;
                    httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                }
            }
        }
        private static void ConfigureWsHttpBinding(WSHttpBinding binding)
        {
            binding.AllowCookies = true;
            binding.Security = new WSHttpSecurity { Mode = SecurityMode.None };
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var readerQuotas = binding.ReaderQuotas;
            if (readerQuotas != null)
            {
                readerQuotas.MaxArrayLength = int.MaxValue;
                readerQuotas.MaxStringContentLength = int.MaxValue;
            }
        }
        private static void ConfigureBasicHttpBinding(BasicHttpBinding binding)
        {
            binding.AllowCookies = true;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;

            var readerQuotas = binding.ReaderQuotas;
            if (readerQuotas != null)
            {
                readerQuotas.MaxArrayLength = int.MaxValue;
                readerQuotas.MaxStringContentLength = int.MaxValue;
            }
        }

        private void AddStateForWrappedOptions(WsdlImporter importer)
        {
            if (!importer.State.ContainsKey(typeof(WrappedOptions)))
            {
                var wrappedOptions = new WrappedOptions();
                wrappedOptions.WrappedFlag = false;

                importer.State.Add(typeof(WrappedOptions), wrappedOptions);
            }
        }
        private void AddStateForXmlSerializerImport(WsdlImporter importer)
        {
            webReferenceOptions.CodeGenerationOptions = CodeGenerationOptions.GenerateOrder |
                                                        CodeGenerationOptions.GenerateNewAsync |
                                                        CodeGenerationOptions.GenerateOldAsync |
                                                        CodeGenerationOptions.EnableDataBinding |
                                                        CodeGenerationOptions.GenerateProperties;

            webReferenceOptions.SchemaImporterExtensions.Add(typeof(TypedDataSetSchemaImporterExtension).AssemblyQualifiedName);
            webReferenceOptions.SchemaImporterExtensions.Add(typeof(DataSetSchemaImporterExtension).AssemblyQualifiedName);

            var importOptions = new XmlSerializerImportOptions(codeCompileUnit);
            importOptions.CodeProvider = codeDomProvider;
            importOptions.WebReferenceOptions = webReferenceOptions;

            if (Options.Namespaces.ContainsKey("*"))
                importOptions.ClrNamespace = Options.Namespaces["*"];

            importer.State.Add(typeof(XmlSerializerImportOptions), importOptions);
        }
        private void AddStateForFaultSerializerImport(WsdlImporter importer)
        {
            var faultImportOptions = new FaultImportOptions();
            faultImportOptions.UseMessageFormat = true;

            importer.State.Add(typeof(FaultImportOptions), faultImportOptions);
        }
        private void AddStateForDataContractSerializerImport(WsdlImporter importer)
        {
            var importOptions = new ImportOptions();
            importOptions.GenerateSerializable = true;
            //importOptions.GenerateInternal = true;
            importOptions.EnableDataBinding = true;
            importOptions.ImportXmlType = (Options.FormatMode == DynamicProxyLoaderOptions.FormatModeOptions.DataContractSerializer);
            importOptions.CodeProvider = codeDomProvider;

            foreach (var pair in Options.Namespaces)
                importOptions.Namespaces.Add(pair.Key, pair.Value);

            var xsdDataContractImporter = new XsdDataContractImporter(codeCompileUnit);
            xsdDataContractImporter.Options = importOptions;

            importer.State.Add(typeof(XsdDataContractImporter), xsdDataContractImporter);

            foreach (var importExtension in importer.WsdlImportExtensions)
            {
                var dcConverter = importExtension as DataContractSerializerMessageContractImporter;
                if (dcConverter != null)
                    dcConverter.Enabled = (Options.FormatMode != DynamicProxyLoaderOptions.FormatModeOptions.XmlSerializer);
            }
        }

        private void CreateProxy()
        {
            foreach (var contractDescription in ContractDescriptions)
            {
                contractGenerator.GenerateServiceContractType(contractDescription);
                ContractDescription = contractDescription;
            }

            bool success = true;

            CodeGenerationWarnings = contractGenerator.Errors;
            if (CodeGenerationWarnings != null)
            {
                foreach (var error in CodeGenerationWarnings)
                {
                    if (!error.IsWarning)
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (!success)
            {
                var exception = new DynamicProxyException(DynamicErrorMessages.CodeGenerationError);
                exception.CodeGenerationErrors = CodeGenerationWarnings;

                throw exception;
            }
        }

        private void CompileProxyCode()
        {
            var compilerParams = new CompilerParameters();
            compilerParams.GenerateInMemory = false;
            compilerParams.GenerateExecutable = false;
            compilerParams.IncludeDebugInformation = true;

            AddAssemblyReference(typeof(ServiceContractAttribute).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(System.Web.Services.Description.ServiceDescription).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(DataContractAttribute).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(XmlElement).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(Uri).Assembly, compilerParams.ReferencedAssemblies);
            AddAssemblyReference(typeof(DataSet).Assembly, compilerParams.ReferencedAssemblies);

            var results = codeDomProvider.CompileAssemblyFromSource(compilerParams, AssemblyCode);
            if (results.Errors != null)
            {
                CompilationWarnings = results.Errors.Cast<CompilerError>().ToList();

                if (results.Errors.HasErrors)
                    throw new DynamicProxyException(DynamicErrorMessages.CompilationError);
            }

            Assembly = results.CompiledAssembly;
            AssemblyBytes = File.ReadAllBytes(results.PathToAssembly);

            ImportContractsAndMethods();
        }

        private void GenerateProxyCode()
        {
            using (var stringWriter = new StringWriter())
            {
                var codeGenOptions = new CodeGeneratorOptions();
                codeGenOptions.BracingStyle = "C";
                codeDomProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, codeGenOptions);
                stringWriter.Flush();

                AssemblyCode = stringWriter.ToString();
            }

            if (Options.CodeModifier != null)
                AssemblyCode = Options.CodeModifier(AssemblyCode);
        }

        private void ImportContractsAndMethods()
        {
            var methodNames = new List<String>();
            var dataContracts = new List<Type>();
            if (ServiceContracts == null)
            {
                ServiceContracts = new List<Type>();
                foreach (var contractDescription in ContractDescriptions)
                {
                    var serviceContract = Assembly.GetType(contractDescription.Name);
                    if (serviceContract != null)
                    {
                        ServiceContracts.Add(serviceContract);

                        if (serviceContract.Name == ContractDescription.Name)
                            ServiceContract = serviceContract;
                    }
                }
            }


            foreach (var type in Assembly.GetTypes())
            {
                if (type.GetInterface("IExtensibleDataObject") != null)
                    dataContracts.Add(type);

                if (type.Name == ContractDescription.Name && type.IsInterface)
                {
                    foreach (var method in type.GetMethods())
                        methodNames.Add(method.Name);
                }
            }

            var clientName = String.Format("{0}Client", ContractDescription.Name);
            foreach (var type in Assembly.GetTypes())
            {
                if (type.Name == clientName)
                {
                    Methods = new List<MethodInfo>();

                    foreach (var methodName in methodNames)
                        Methods.Add(type.GetMethod(methodName));
                }
            }

            DataContracts = dataContracts;
        }

        private void AddAssemblyReference(Assembly referencedAssembly, StringCollection refAssemblies)
        {
            var path = Path.GetFullPath(referencedAssembly.Location);
            var name = Path.GetFileName(path);

            if (!(refAssemblies.Contains(name) || refAssemblies.Contains(path)))
                refAssemblies.Add(path);
        }

        public ServiceEndpoint GetEndpoint(String contractName)
        {
            return GetEndpoint(contractName, null);
        }
        public ServiceEndpoint GetEndpoint(String contractName, String contractNamespace)
        {
            ServiceEndpoint matchingEndpoint = null;

            foreach (ServiceEndpoint endpoint in Endpoints)
            {
                if (ContractNameMatch(endpoint.Contract, contractName) && ContractNsMatch(endpoint.Contract, contractNamespace))
                {
                    matchingEndpoint = endpoint;
                    break;
                }
            }

            if (matchingEndpoint == null)
                throw new ArgumentException(String.Format(DynamicErrorMessages.EndpointNotFound, contractName, contractNamespace));

            return matchingEndpoint;
        }

        public DynamicObject CreateProxy(String contractName)
        {
            return CreateProxy(contractName, null);
        }
        public DynamicObject CreateProxy(String contractName, String contractNamespace)
        {
            var endpoint = GetEndpoint(contractName, contractNamespace);
            return CreateProxy(endpoint);
        }
        public DynamicObject CreateProxy(ServiceEndpoint endpoint)
        {
            var contractType = GetContractType(endpoint.Contract.Name, endpoint.Contract.Namespace);
            var proxyType = GetProxyType(contractType);

            return new DynamicProxy(proxyType, endpoint.Binding, endpoint.Address);
        }

        private Type GetContractType(String contractName, String contractNamespace)
        {
            var allTypes = Assembly.GetTypes();
            var contractType = (Type)null;

            foreach (var type in allTypes)
            {
                if (!type.IsInterface)
                    continue;

                var attrs = type.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                if (attrs.Length == 0)
                    continue;

                var scAttr = (ServiceContractAttribute)attrs[0];
                var cntName = GetContractName(type, scAttr.Name, scAttr.Namespace);

                if (!String.Equals(contractName, cntName.Name, StringComparison.OrdinalIgnoreCase) ||
                    !String.Equals(contractNamespace, cntName.Namespace, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                contractType = type;
                break;
            }

            if (contractType == null)
                throw new ArgumentException(DynamicErrorMessages.UnknownContract);

            return contractType;
        }

        public Type GetProxyType(String contractName)
        {
            return GetProxyType(contractName, null);
        }
        public Type GetProxyType(String contractName, String contractNamespace)
        {
            var endpoint = GetEndpoint(contractName, contractNamespace);
            return GetProxyType(endpoint);
        }
        public Type GetProxyType(ServiceEndpoint endpoint)
        {
            var contractType = GetContractType(endpoint.Contract.Name, endpoint.Contract.Namespace);
            var proxyType = GetProxyType(contractType);

            return proxyType;
        }
        private Type GetProxyType(Type contractType)
        {
            var clientBaseType = typeof(ClientBase<>).MakeGenericType(contractType);

            var allTypes = Assembly.GetTypes();
            var proxyType = (Type)null;

            foreach (var type in allTypes)
            {
                if (type.IsClass && contractType.IsAssignableFrom(type) && type.IsSubclassOf(clientBaseType))
                {
                    proxyType = type;
                    break;
                }
            }

            if (proxyType == null)
                throw new DynamicProxyException(String.Format(DynamicErrorMessages.ProxyTypeNotFound, contractType.FullName));

            return proxyType;
        }

        private static XmlQualifiedName GetContractName(Type contractType, String name, String ns)
        {
            name = (String.IsNullOrEmpty(name) ? contractType.Name : name);
            ns = (ns == null ? defaultNamespace : Uri.EscapeUriString(ns));

            return new XmlQualifiedName(name, ns);
        }

        private static bool ContractNameMatch(ContractDescription cDesc, string name)
        {
            return String.Equals(cDesc.Name, name, StringComparison.OrdinalIgnoreCase);
        }
        private static bool ContractNsMatch(ContractDescription cDesc, string ns)
        {
            return (ns == null || String.Equals(cDesc.Namespace, ns, StringComparison.OrdinalIgnoreCase));
        }

        public static String ToString(IEnumerable<MetadataConversionError> importErrors)
        {
            if (importErrors != null)
            {
                var importErrStr = new StringBuilder();

                foreach (var error in importErrors)
                {
                    if (error.IsWarning)
                        importErrStr.AppendLine("Warning : " + error.Message);
                    else
                        importErrStr.AppendLine("Error : " + error.Message);
                }

                return importErrStr.ToString();
            }

            return null;
        }
        public static String ToString(IEnumerable<CompilerError> compilerErrors)
        {
            if (compilerErrors != null)
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in compilerErrors)
                    stringBuilder.AppendLine(error.ToString());

                return stringBuilder.ToString();
            }

            return null;
        }
    }
}