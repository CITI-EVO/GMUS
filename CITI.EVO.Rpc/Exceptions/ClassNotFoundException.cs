using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class ClassNotFoundException : Exception
    {
        public ClassNotFoundException(String message)
            : base(message)
        {
        }
    }
}