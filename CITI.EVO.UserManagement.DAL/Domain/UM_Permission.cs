using System;
using System.Collections.Generic;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_Permission : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid GroupID { get; set; }
        public virtual Guid ResourceID { get; set; }
        public virtual int? RuleValue { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_Group Group { get; set; }
        public virtual UM_Resource Resource { get; set; }

        public virtual ICollection<UM_PermissionParameter> PermissionParameters { get; set; }
    }
}