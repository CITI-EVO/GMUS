using System;

namespace CITI.EVO.UserManagement.Web.Models.Helpers
{
    public class ObjectAttributeUnitModel
    {
        public Guid? ID { get; set; }

        public String Schema { get; set; }

        public String Node { get; set; }

        public String Value { get; set; }
    }
}