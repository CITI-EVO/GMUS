using System;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_LoginToken : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid LoginToken { get; set; }
        public virtual Guid? UserID { get; set; }
        public virtual DateTime? ExpireDate { get; set; }
        public virtual DateTime? LastAccessDate { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual int? DeleteReason { get; set; }

        public virtual UM_User User { get; set; }
    }
}