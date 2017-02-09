using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class GroupModel
    {
        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public Guid? ProjectID { get; set; }

        public String Name { get; set; }
    }
}