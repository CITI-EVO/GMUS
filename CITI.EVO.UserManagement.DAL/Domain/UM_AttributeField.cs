using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_AttributeField : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual Guid AttributeSchemaID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_AttributeSchema AttributeSchema { get; set; }
        public virtual ICollection<UM_AttributeValue> AttributeValues { get; set; }
    }
}
