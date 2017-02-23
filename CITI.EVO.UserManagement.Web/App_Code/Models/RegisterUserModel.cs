using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class RegisterUserModel
    {
        public Guid? GroupID { get; set; }

        public String LoginName { get; set; }

        public String Email { get; set; }

        public String PersonalID { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String BirthDate { get; set; }

        public String Password { get; set; }

        public String ConfirmPassword { get; set; }

    }
}