using System;

namespace CITI.EVO.Rpc.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RpcAllowRemoteCallAttribute : Attribute
    {
    }
}
