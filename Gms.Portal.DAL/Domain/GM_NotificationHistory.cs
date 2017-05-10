using System;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public class GM_NotificationHistory : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid RecipientID { get; set; }
        public virtual Guid UserID { get; set; }
        public virtual String Email { get; set; }
        public virtual String Phone { get; set; }
        public virtual String ContactType { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}
