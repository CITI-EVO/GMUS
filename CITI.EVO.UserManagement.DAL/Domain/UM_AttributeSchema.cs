using System;
using System.Collections.Generic;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_AttributeSchema : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid? ProjectID { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_Project Project { get; set; }
        public virtual ICollection<UM_AttributeField> AttributeFields { get; set; }
    }
}