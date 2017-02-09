using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class PeerNotFoundException : Exception
    {
        public PeerNotFoundException(String message)
            : base(message)
        {
        }
    }
}
