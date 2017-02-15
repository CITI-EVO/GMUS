using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.UserManagement.DAL.Domain
{
    public partial class UM_Message : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Text { get; set; }
        public virtual Guid ObjectID { get; set; }
        public virtual int Type { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }

        public virtual ICollection<UM_MessageViewer> MessageViewers { get; set; }
    }
}