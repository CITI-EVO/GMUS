using System;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_CityPhoneCode
    {
        public virtual Guid ID { get; set; }
        public virtual String CityName { get; set; }
        public virtual int? PhoneCode { get; set; }

        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}