using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class MethodAttributeNotDefinedException : RemoteMethodAttributeException
    {
        public MethodAttributeNotDefinedException(String message)
            : base(message)
        {
        }
    }
}