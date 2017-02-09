using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class InvalidPeerUrlException : Exception
    {
        public InvalidPeerUrlException(String message)
            : base(message)
        {
        }
    }
}