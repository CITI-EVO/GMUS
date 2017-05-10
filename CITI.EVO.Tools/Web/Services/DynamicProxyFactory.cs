using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Services.Discovery;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Web.Services
{
    public static class DynamicProxyFactory
    {
        private const String ProxyCacheKey = "$[DynamicProxyLoader_ServiceReferences]";

        private static DynamicProxyLoader ReadCache(String fileName)
        {
            var proxyCache = CommonObjectCache.InitObject(ProxyCacheKey, ConcurrencyHelper.CreateDictionary<String, DynamicProxyLoader>);
            var hashCode = fileName.ComputeMd5();

            var factory = proxyCache.GetValueOrDefault(hashCode);
            return factory;
        }

        private static void WriteCache(String fileName, DynamicProxyLoader factory)
        {
            var proxyCache = CommonObjectCache.InitObject(ProxyCacheKey, ConcurrencyHelper.CreateDictionary<String, DynamicProxyLoader>);
            var hashCode = fileName.ComputeMd5();

            proxyCache[hashCode] = factory;
        }

        public static void Save(String fileName, DynamicProxyLoader factory)
        {
            var svcFileName = Path.GetFileName(fileName);
            var svcFolderPath = Path.GetDirectoryName(fileName);

            if (String.IsNullOrWhiteSpace(svcFileName) || String.IsNullOrWhiteSpace(svcFolderPath))
                throw new Exception("Invalid folder or file path");

            if (!Directory.Exists(svcFolderPath))
                Directory.CreateDirectory(svcFolderPath);

            WriteCache(fileName, factory);

            var proxyClientFileName = $"{svcFileName}.config";
            var proxyAssemblyFileName = $"{fileName}.dll";
            var proxyCodeFileName = $"{fileName}.cs";

            factory.DiscoveryClient.WriteAll(svcFolderPath, proxyClientFileName);

            File.WriteAllBytes(proxyAssemblyFileName, factory.AssemblyBytes);
            File.WriteAllText(proxyCodeFileName, factory.AssemblyCode);
        }

        public static DynamicProxyLoader Load(String fileName)
        {
            return Load(fileName, null, null);
        }

        public static DynamicProxyLoader Load(String fileName, String @namespace)
        {
            return Load(fileName, @namespace, null);
        }

        public static DynamicProxyLoader Load(String fileName, String @namespace, DynamicProxyLoaderOptions options)
        {
            @namespace = (@namespace ?? "Hmis_Service_Ref");
            options = (options ?? new DynamicProxyLoaderOptions());

            var dynamicProxyFactory = ReadCache(fileName);
            if (dynamicProxyFactory != null)
                return dynamicProxyFactory;

            var proxyClientFileName = String.Format("{0}.config", fileName);
            var proxyAssemblyFileName = String.Format("{0}.dll", fileName);
            var proxyCodeFileName = String.Format("{0}.cs", fileName);

            var discoveryClient = new DiscoveryClientProtocol();
            discoveryClient.ReadAll(proxyClientFileName);

            var proxyAssemblyBytes = File.ReadAllBytes(proxyAssemblyFileName);
            var proxyAssemblyCode = File.ReadAllText(proxyCodeFileName);
            var proxyAssembly = Assembly.Load(proxyAssemblyBytes);

            dynamicProxyFactory = new DynamicProxyLoader(discoveryClient, proxyAssembly, proxyAssemblyBytes, proxyAssemblyCode, @namespace, options);

            WriteCache(fileName, dynamicProxyFactory);

            return dynamicProxyFactory;
        }

        public static DynamicProxyLoader Load(Uri uri)
        {
            return Load(uri, null, null);
        }

        public static DynamicProxyLoader Load(Uri uri, String @namespace)
        {
            return Load(uri, @namespace, null);
        }

        public static DynamicProxyLoader Load(Uri uri, String @namespace, DynamicProxyLoaderOptions options)
        {
            @namespace = (@namespace ?? $"DynamicService_{GetHandlerName(uri)}");
            options = (options ?? new DynamicProxyLoaderOptions());

            var discoveryClient = new DiscoveryClientProtocol
            {
                AllowAutoRedirect = true
            };

            if (options.UseCredential)
            {
                var credentialCache = new CredentialCache();
                var networkCredential = new NetworkCredential(options.UserName, options.Password, options.Domain);

                var uriPrefix = new Uri(uri.AbsoluteUri.Substring(0, uri.AbsoluteUri.Length - uri.AbsolutePath.Length));

                var registeredModules = AuthenticationManager.RegisteredModules;
                while (registeredModules.MoveNext())
                    credentialCache.Add(uriPrefix, ((IAuthenticationModule)registeredModules.Current).AuthenticationType, networkCredential);

                discoveryClient.Credentials = credentialCache;
            }
            else
            {
                discoveryClient.Credentials = CredentialCache.DefaultCredentials;
            }

            discoveryClient.DiscoverAny(uri.ToString());
            discoveryClient.ResolveAll();

            return new DynamicProxyLoader(discoveryClient, @namespace, options);
        }

        private static String GetHandlerName(Uri uri)
        {
            var filePath = uri.AbsolutePath;
            
            var name = Path.GetFileNameWithoutExtension(filePath);
            return name;
        }
    }
}