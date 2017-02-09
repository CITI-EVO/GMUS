using System;

namespace CITI.EVO.Rpc.Exceptions
{
    [Serializable]
    public class GenericTypeNotFoundException : Exception
    {
        public GenericTypeNotFoundException(String message)
            : base(message)
        {
        }
    }
}