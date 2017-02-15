using System;

namespace Gms.Portal.Web.Entities.Helpers
{
    public class TreeNodeEntity
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }
    }
}