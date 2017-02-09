using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class AttributeSchemaModel
    {
        public Guid? ID { get; set; }

        public Guid? ProjectID { get; set; }

        public String Name { get; set; }

        public AttributeFieldsModel Fields { get; set; }
    }
}