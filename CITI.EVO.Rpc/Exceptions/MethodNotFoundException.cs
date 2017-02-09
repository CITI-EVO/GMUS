using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException(String message)
            : base(message)
        {
        }
    }
}