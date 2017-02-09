using System;
using CITI.EVO.UserManagement.DAL.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_GroupOrganization : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid? GroupID { get; set; }
        public virtual Guid? OrganizationID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual UM_Group Group { get; set; }
    }
}