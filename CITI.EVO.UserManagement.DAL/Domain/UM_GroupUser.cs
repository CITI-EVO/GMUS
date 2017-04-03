using System;
using CITI.EVO.Core.Interfaces;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_GroupUser : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual int? AccessLevel { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual Guid? UserID
        {
            get { return Hb8Util.GetObjectID<Guid?>(User); }
            set { User = Hb8Util.GetObject<UM_User>(value); }
        }
        public virtual Guid? GroupID
        {
            get { return Hb8Util.GetObjectID<Guid?>(Group); }
            set { Group = Hb8Util.GetObject<UM_Group>(value); }
        }

        public virtual UM_User User { get; set; }
        public virtual UM_Group Group { get; set; }
    }
}