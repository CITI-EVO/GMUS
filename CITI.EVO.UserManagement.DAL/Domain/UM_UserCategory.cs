using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_UserCategory : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual ICollection<UM_User> Users { get; set; }
    }
}