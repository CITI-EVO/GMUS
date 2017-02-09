using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class MethodFullNameEmptyException : RemoteMethodAttributeException
    {
        public MethodFullNameEmptyException(String message)
            : base(message)
        {
        }
    }
}
