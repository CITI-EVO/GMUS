using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class ChangePasswordModel
    {
        public String OldPassword { get; set; }

        public String NewPassword { get; set; }

        public String ConfirmPassword { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }

    }
}