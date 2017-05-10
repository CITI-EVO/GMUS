using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class ServiceInvokeResult
    {
        public Object Return { get; set; }

        public IDictionary<String, Object> Outs { get; set; }
    }
}