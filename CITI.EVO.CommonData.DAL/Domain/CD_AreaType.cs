using System;
using System.Collections.Generic;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_AreaType : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual String GeoName { get; set; }
        public virtual String EngName { get; set; }
        public virtual int Code { get; set; }
        public virtual int Level { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<CD_Area> Areas { get; set; }
    }
}