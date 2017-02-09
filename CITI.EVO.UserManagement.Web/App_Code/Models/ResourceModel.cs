using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    [Serializable]
    public class ResourceModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public int? Type { get; set; }

        public String Value { get; set; }

        public Guid? ParentID { get; set; }

        public Guid? ProjectID { get; set; }

        public String Description { get; set; }
    }
}