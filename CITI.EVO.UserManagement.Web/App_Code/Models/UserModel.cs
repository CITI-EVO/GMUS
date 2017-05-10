using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class UserModel
    {
        public Guid? ID { get; set; }
        public String LoginName { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Address { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public String UserCode { get; set; }
        public Guid? UserCategoryID { get; set; }

        public bool? ChangePassword { get; set; }

        public DateTime? PasswordExpire { get; set; }
    }
}