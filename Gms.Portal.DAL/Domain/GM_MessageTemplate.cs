using System;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    [Serializable]
    public partial class GM_MessageTemplate : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Subject { get; set; }
        public virtual String Body { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}
