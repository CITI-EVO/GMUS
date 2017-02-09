using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class RemoteMethodAttributeException : Exception
    {
        public RemoteMethodAttributeException(String message)
            : base(message)
        {
        }
    }
}