using System;
using CITI.EVO.Core.Interfaces;

namespace Gms.Portal.DAL.Domain
{
    public class GM_DataStatus : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String Code { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}