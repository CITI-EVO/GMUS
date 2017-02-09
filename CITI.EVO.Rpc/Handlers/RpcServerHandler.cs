﻿using System.Web;
using CITI.EVO.Rpc.Common;
using CITI.EVO.Rpc.Processing;
using CITI.EVO.Rpc.Utils;

namespace CITI.EVO.Rpc.Handlers
{
    public class RpcServerHandler : IHttpHandler, IRequestProcessor
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public RequestProcessorBase RequestProcessor
        {
            get { return ProcessorUtil.GetServerRequestProcessor(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            RequestProcessor.ProcessRequest(context.Request, context.Response);
        }
    }
}
