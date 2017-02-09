using System;
using System.Collections.Generic;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_PhoneIndexType 
    {
        public virtual Guid ID { get; set; }
        public virtual String Name { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public virtual ICollection<CD_PhoneIndex> PhoneIndexes { get; set; }
    }
}