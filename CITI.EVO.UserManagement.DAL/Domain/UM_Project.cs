using System;
using System.Collections.Generic;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_Project : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<UM_Rule> Rules { get; set; }
        public virtual ICollection<UM_Group> Groups { get; set; }
        public virtual ICollection<UM_UserLog> UserLogs { get; set; }
        public virtual ICollection<UM_Resource> Resources { get; set; }
        public virtual ICollection<UM_AttributeSchema> AttributesSchemas { get; set; }
    }
}