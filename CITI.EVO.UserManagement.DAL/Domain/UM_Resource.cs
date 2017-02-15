using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_Resource : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid? ParentID { get; set; }
        public virtual Guid? ProjectID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Description { get; set; }
        public virtual int Type { get; set; }
        public virtual String Value { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_Project Project { get; set; }
        public virtual UM_Resource Parent { get; set; }

        public virtual ICollection<UM_Resource> Children { get; set; }
        public virtual ICollection<UM_Permission> Permissions { get; set; }
    }
}