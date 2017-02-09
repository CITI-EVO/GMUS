using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class ServerInvocationException : Exception
    {
        public ServerInvocationException(int code, String message)
            : base(message)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}