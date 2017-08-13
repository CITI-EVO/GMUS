using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.RegularExpressions;
using log4net;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.Services;
using Gms.Portal.Web.Models;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Entities.ServiceStructure;

namespace Gms.Portal.Web.Helpers
{
    public class ExternalServiceInvoker
    {
        private const String ServicesCacheKey = "ServicesCacheRootFolder";

        private readonly ILog _logger;

        private readonly ServiceModel _serviceModel;
        private readonly MethodEntity _methodEntity;

        public ExternalServiceInvoker(ServiceModel serviceModel, Guid methodID)
        {
            _logger = LogUtil.GetLogger("ServicesLogger");

            _serviceModel = serviceModel;
            _methodEntity = _serviceModel.Entity.Methods.Single(n => n.ID == methodID);

            InitDynamicObject(false);
        }
        public ExternalServiceInvoker(ServiceModel serviceModel, String methodName)
        {
            _logger = LogUtil.GetLogger("ServicesLogger");

            _serviceModel = serviceModel;
            _methodEntity = _serviceModel.Entity.Methods.Single(n => n.Name == methodName);

            InitDynamicObject(false);
        }

        public ILog Logger
        {
            get { return _logger; }
        }

        public ServiceModel Service
        {
            get { return _serviceModel; }
        }

        public MethodEntity Method
        {
            get { return _methodEntity; }
        }

        public DynamicObject DynamicObject { get; private set; }

        public IServiceAssemblyLoader AssemblyLoader { get; private set; }

        public ServiceInvokeResult Invoke(IDictionary<String, Object> parameters)
        {
            var sw = Stopwatch.StartNew();
            var result = TryInvoke(parameters);
            sw.Stop();

            if (_logger != null)
                _logger.InfoFormat("Invoke - {0} - {1} - {2}", Service.Name, Method.Name, sw.ElapsedMilliseconds);

            return result;
        }

        private ServiceInvokeResult TryInvoke(IDictionary<String, Object> inputs)
        {
            try
            {
                return CallMethod(inputs);
            }
            catch (Exception ex)
            {
                if (ex is ActionNotSupportedException ||
                    ex is InvalidOperationException ||
                    (ex is FaultException && ex.InnerException is SerializationException) ||
                    (ex is TargetInvocationException && ex.InnerException is ProtocolException))
                {
                    if (_logger != null)
                        _logger.InfoFormat("Load - {0} - {1} - {2}", Service.Name, Method.Name, ex.Message);

                    InitDynamicObject(true);

                    return CallMethod(inputs);
                }

                throw;
            }
        }

        private ServiceInvokeResult CallMethod(IDictionary<String, Object> inputs)
        {
            var parameters = DictionaryToParameters(inputs);

            var invokeResult = new ServiceInvokeResult
            {
                Return = DynamicObject.CallMethod(Method.Name, parameters),
                Outs = new Dictionary<String, Object>()
            };

            foreach (var parameter in Method.Parameters)
            {
                if (parameter.IsOut)
                {
                    var name = parameter.Name;
                    var index = parameter.OrderIndex;

                    invokeResult.Outs.Add(name, parameters[index]);
                }
            }

            return invokeResult;
        }

        private IDictionary<String, ParameterEntity> GetParameters()
        {
            var parameters = new Dictionary<String, ParameterEntity>();

            var parametersLp = Method.Parameters.ToLookup(n => n.Name);

            foreach (var parametersGrp in parametersLp)
            {
                if (parametersGrp.Count() != 1)
                    throw new Exception(parametersGrp.Key);

                var parameter = parametersGrp.Single();
                parameters.Add(parameter.Name, parameter);
            }

            return parameters;
        }

        private Object[] DictionaryToParameters(IDictionary<String, Object> parametersData)
        {
            var parameterValues = new Object[parametersData.Count];
            var parametersDict = GetParameters();

            foreach (var pair in parametersData)
            {
                var parameter = parametersDict.GetValueOrDefault(pair.Key);
                if (parameter == null)
                    continue;

                if (pair.Value is IDictionary<String, Object>) //if complex
                {
                    var properties = pair.Value as IDictionary<String, Object>;

                    var assembly = AssemblyLoader.Assembly;
                    var @object = assembly.CreateInstance(parameter.Type);

                    FillProperties(properties, @object, assembly);

                    parameterValues[parameter.OrderIndex] = @object;
                }
                else
                {
                    parameterValues[parameter.OrderIndex] = DataConverter.ChangeType(pair.Value, parameter.Type);
                }
            }

            return parameterValues;
        }

        private void FillProperties(IDictionary<String, Object> properties, Object @object, Assembly assembly)
        {
            var objectType = @object.GetType();

            var propertiesKeys = properties.Keys.ToList();
            foreach (var propertyKey in propertiesKeys)
            {
                var propertyValue = properties[propertyKey];

                var propertyInfo = objectType.GetProperty(propertyKey);
                var propertyType = propertyInfo.PropertyType;

                var subDict = propertyValue as IDictionary<String, Object>;
                if (subDict != null)
                {
                    var propertyObj = assembly.CreateInstance(propertyType.FullName);
                    FillProperties(subDict, propertyObj, assembly);

                    properties[propertyKey] = propertyObj;
                }
                else
                {
                    var changedValue = DataConverter.ChangeType(propertyValue, propertyType);
                    propertyInfo.SetValue(@object, changedValue, null);
                }
            }
        }

        private void InitDynamicObject(bool refresh)
        {
            var folderPath = ConfigurationManager.AppSettings[ServicesCacheKey];
            if (String.IsNullOrWhiteSpace(folderPath))
                throw new Exception("Invalid service cache folder path");

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            folderPath = Regex.Replace(folderPath, @"{AppPath}", baseDirectory, RegexOptions.IgnoreCase);

            var serviceFolderPath = $@"{folderPath}\{Service.ID}";

            try
            {
                AssemblyLoader = DynamicProxyFactory.Load(serviceFolderPath);
            }
            catch (Exception)
            {
                refresh = true;
            }

            if (refresh)
            {
                var serviceUri = new Uri(Service.Url);
                AssemblyLoader = DynamicProxyFactory.Load(serviceUri);

                if (!Directory.Exists(serviceFolderPath))
                    Directory.CreateDirectory(serviceFolderPath);

                DynamicProxyFactory.Save(serviceFolderPath, (DynamicProxyLoader)AssemblyLoader);
            }

            var dynamicProxyLoader = (DynamicProxyLoader)AssemblyLoader;
            DynamicObject = AssemblyLoader.CreateProxy(dynamicProxyLoader.ServiceContract.Name);
        }
    }
}