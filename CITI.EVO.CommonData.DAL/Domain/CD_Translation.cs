using System;

namespace CITI.EVO.CommonData.DAL.Domain
{
    public class CD_Translation 
    {
        public virtual Guid ID { get; set; }
        public virtual String ModuleName { get; set; }
        public virtual String TrnKey { get; set; }
        public virtual String LanguagePair { get; set; }
        public virtual String DefaultText { get; set; }
        public virtual String TranslatedText { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime? DateChanged { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
    }
}