using System;

namespace CITI.EVO.UserManagement.Web.Entities
{
    /// <summary>
    /// Summary description for IdNameEntity
    /// </summary>
    public class ParentChildEntity
    {
        public Guid? ID { get; set; }
        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public Object Tag { get; set; }
    }
}