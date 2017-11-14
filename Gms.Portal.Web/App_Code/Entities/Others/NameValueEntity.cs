using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class NameValueEntity<TValue>
    {
        public String Name { get; set; }

        public TValue Value { get; set; }
    }
}