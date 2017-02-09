using System;

namespace CITI.EVO.Rpc.Common
{
    [Serializable]
    public class ResponseEntity
    {
        public Guid RequestID { get; set; }

        public int ErrorCode { get; set; }

        public String ErrorMessage { get; set; }

        public Object ResultObject { get; set; }
    }
}
