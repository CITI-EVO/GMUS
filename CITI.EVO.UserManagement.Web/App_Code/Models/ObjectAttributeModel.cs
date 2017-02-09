using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class ObjectAttributeModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public Guid? ProjectID { get; set; }

        public Guid? SchemaID { get; set; }

        public Guid? FieldID { get; set; }

        public String Value { get; set; }
    }
}