using System;
using System.Collections.Generic;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_Area 
    {
       
        public virtual Guid ID { get; set; }
        public virtual decimal? OLD_ID { get; set; }
        public virtual Guid? ParentID { get; set; }
        public virtual string Code { get; set; }
        public virtual string CraCode { get; set; }
        public virtual string GeoName { get; set; }
        public virtual string EngName { get; set; }
        public virtual Guid TypeID { get; set; }
        public virtual string NewCode { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<CD_Area> Children { get; set; }
        public virtual ICollection<CD_PhoneIndex> PhoneIndexes { get; set; }
        public virtual CD_Area Parent { get; set; }
        public virtual CD_AreaType AreaType { get; set; }

    }
}