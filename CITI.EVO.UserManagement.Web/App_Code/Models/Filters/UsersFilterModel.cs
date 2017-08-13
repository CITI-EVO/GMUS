using System;

namespace CITI.EVO.UserManagement.Web.Models.Filters
{
    public class UsersFilterModel
    {
        public bool? LoginName { get; set; }

        public bool? FirstName { get; set; }

        public bool? LastName { get; set; }

        public bool? Address { get; set; }

        public bool? Password { get; set; }

        public bool? Status { get; set; }

        public Guid? CategoryID { get; set; }

        public String Keyword { get; set; }
    }
}