using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_User : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual string LoginName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsSuperAdmin { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual String UserCode { get; set; }
        public virtual Guid? UserCategoryID { get; set; }
        public virtual DateTime? PasswordExpirationDate { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_UserCategory UserCategory { get; set; }

        public virtual ICollection<UM_UserLog> UserLogs { get; set; }
        public virtual ICollection<UM_GroupUser> GroupUsers { get; set; }
        public virtual ICollection<UM_LoginToken> LoginTokens { get; set; }
        public virtual ICollection<UM_MessageViewer> MessageViewers { get; set; }
    }
}