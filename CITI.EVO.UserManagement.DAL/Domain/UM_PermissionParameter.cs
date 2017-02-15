using System;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_PermissionParameter : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid PermissionID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual UM_Permission Permission { get; set; }
    }
}