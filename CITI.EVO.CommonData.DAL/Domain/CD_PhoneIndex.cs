using System;
using CITI.EVO.Core.Interfaces;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_PhoneIndex : IDbEntity
    {
        public virtual Guid ID { get; set; }
        public virtual Guid PhoneIndexTypeID{ get; set; }
        public virtual int Value { get; set; }
        public virtual Guid AreaID { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual CD_Area Area { get; set; }
        public virtual CD_PhoneIndexType PhoneIndexType { get; set; }
    }
}