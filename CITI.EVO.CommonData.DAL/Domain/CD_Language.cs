using System;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_Language 
    {
        public virtual Guid ID { get; set; }
        public virtual String DisplayName { get; set; }
        public virtual String EngName { get; set; }
        public virtual String NativeName { get; set; }
        public virtual String Pair { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}