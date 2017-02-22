using System;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class NamedModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }
    }
}