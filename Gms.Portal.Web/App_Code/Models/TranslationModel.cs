using System;

namespace Gms.Portal.Web.Models
{
    [Serializable]
    public class TranslationModel
    {
        public String TrnKey { get; set; }
        public String ModuleName { get; set; }
        public String LanguagePair { get; set; }
        public String DefaultText { get; set; }
        public String TranslatedText { get; set; }
    }
}