using System;

namespace CITI.EVO.Rpc.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RpcRemoteMethodAttribute : Attribute
    {
        public RpcRemoteMethodAttribute(String fullName)
        {
            FullName = fullName;
        }

        public String FullName { get; private set; }
    }
}
