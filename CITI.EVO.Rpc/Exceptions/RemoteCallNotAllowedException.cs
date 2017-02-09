using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class RemoteCallNotAllowedException : Exception
    {
        public RemoteCallNotAllowedException(String message)
            : base(message)
        {
        }
    }
}