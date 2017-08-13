using System;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    public class GM_UserMessage : IDbEntity
    {
        public virtual Guid ID { get; set; }

        public virtual String Subject { get; set; }

        public virtual Guid? ParentID { get; set; }

        public virtual Guid? FormID { get; set; }
        public virtual Guid? RecordID { get; set; }

        public virtual Guid? FromUserID { get; set; }
        public virtual Guid? ToUserID { get; set; }
        public virtual Guid? StatusUserID { get; set; }

        public virtual Guid? StatusID { get; set; }

        public virtual bool? Readed { get; set; }

        public virtual String Text { get; set; }

        public virtual String Description { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}