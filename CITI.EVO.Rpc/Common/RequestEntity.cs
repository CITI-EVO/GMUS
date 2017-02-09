using System;
using System.Collections.Generic;

namespace CITI.EVO.Rpc.Common
{
    [Serializable]
    public class RequestEntity
    {
        public Guid RequestID { get; set; }

        public String Peer { get; set; }

        public String ClassName { get; set; }
        public String MethodName { get; set; }

        public String Compression { get; set; }

        public List<String> GenericTypes { get; set; }

        public List<MethodParam> MethodParams { get; set; }
    }
}
