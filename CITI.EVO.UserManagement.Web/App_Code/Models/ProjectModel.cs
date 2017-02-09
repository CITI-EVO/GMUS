using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class ProjectModel
    {
        public Guid? ID { get; set; }

        public String Name { get; set; }

        public bool? IsActive { get; set; }
    }
}