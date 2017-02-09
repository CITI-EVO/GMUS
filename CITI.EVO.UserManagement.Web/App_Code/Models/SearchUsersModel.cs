using System;

namespace CITI.EVO.UserManagement.Web.Models
{
    public class SearchUsersModel
    {
        public Guid? ParentID { get; set; }

        public Guid? UserID { get; set; }
    }
}