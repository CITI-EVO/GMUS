using System;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_AttributeValue : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid ParentID { get; set; }
        public virtual Guid AttributeFieldID { get; set; }
        public virtual String Value { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_AttributeField AttributeField { get; set; }
    }
}