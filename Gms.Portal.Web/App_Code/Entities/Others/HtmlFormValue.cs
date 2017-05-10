using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class HtmlFormValue
    {
        public String Type { get; set; }

        public Object Key { get; set; }

        public Object Value { get; set; }
    }
}