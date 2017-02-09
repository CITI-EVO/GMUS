using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class LoginModel
    {
        public String LoginName { get; set; }

        public String Password { get; set; }

        public bool? RememberMe { get; set; }
    }
}