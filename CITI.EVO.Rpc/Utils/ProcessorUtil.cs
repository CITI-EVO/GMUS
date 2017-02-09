using System;
using System.Runtime.CompilerServices;
using System.Web;
using CITI.EVO.Rpc.Processing;

namespace CITI.EVO.Rpc.Utils
{
    public static class ProcessorUtil
    {
        private const String clientRequestProcessorCacheKey = "ClientRequestProcessor";
        private const String serverRequestProcessorCacheKey = "ServerRequestProcessor";

        private static RequestProcessorBase clidntRequestProcessor;
        private static RequestProcessorBase serverRequestProcessor;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static RequestProcessorBase GetClientRequestProcessor()
        {
            if (clidntRequestProcessor == null)
            {
                var context = HttpContext.Current;
                if (context == null || context.Cache == null)
                {
                    clidntRequestProcessor = (clidntRequestProcessor ?? new ClientRequestProcessor());
                }
                else
                {
                    clidntRequestProcessor = context.Cache[clientRequestProcessorCacheKey] as RequestProcessorBase;

                    if (clidntRequestProcessor == null)
                    {
                        clidntRequestProcessor = new ClientRequestProcessor();

                        context.Cache.Insert(clientRequestProcessorCacheKey, clidntRequestProcessor);
                    }
                }
            }

            return clidntRequestProcessor;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static RequestProcessorBase GetServerRequestProcessor()
        {
            if (serverRequestProcessor == null)
            {
                var context = HttpContext.Current;
                if (context == null || context.Cache == null)
                {
                    serverRequestProcessor = (serverRequestProcessor ?? new ServerRequestProcessor());
                }
                else
                {
                    serverRequestProcessor = context.Cache[serverRequestProcessorCacheKey] as RequestProcessorBase;

                    if (serverRequestProcessor == null)
                    {
                        serverRequestProcessor = new ServerRequestProcessor();

                        context.Cache.Insert(serverRequestProcessorCacheKey, serverRequestProcessor);
                    }
                }
            }

            return serverRequestProcessor;
        }
    }
}
