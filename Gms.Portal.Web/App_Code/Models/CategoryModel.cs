using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class CategoryModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public int? OrderIndex { get; set; }

        public bool? Visible { get; set; }
    }
}