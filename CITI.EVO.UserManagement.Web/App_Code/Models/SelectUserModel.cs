using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class SelectUserModel
    {
        public Guid? ParentID { get; set; }

        public int? AccessLevel { get; set; }

        public SearchUsersModel User { get; set; }
    }
}