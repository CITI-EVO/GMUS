using System;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_Rule : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid ProjectID { get; set; }
        public virtual String Name { get; set; }
        public virtual bool CanView { get; set; }
        public virtual bool CanAdd { get; set; }
        public virtual bool CanEdit { get; set; }
        public virtual bool CanDelete { get; set; }
        public virtual int? AccessLevel { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual UM_Project Project { get; set; }
    }
}