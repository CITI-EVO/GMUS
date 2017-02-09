using System;

namespace CITI.EVO.Rpc.Common
{
    [Serializable]
    public class MethodParam
    {
        public String TypeName { get; set; }
        public Object ParamValue { get; set; }
    }
}