using System;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_MobileIndex : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual int? Value { get; set; }
        public virtual String GeoOperatorName { get; set; }
        public virtual String EngOperatorName { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}