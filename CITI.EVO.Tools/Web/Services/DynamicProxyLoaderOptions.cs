using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CITI.EVO.Tools.Web.Services
{
    public class DynamicProxyLoaderOptions
    {
        public enum LanguageOptions { CS, VB };
        public enum FormatModeOptions { Auto, XmlSerializer, DataContractSerializer }

        public DynamicProxyLoaderOptions()
        {
            Language = LanguageOptions.CS;
            FormatMode = FormatModeOptions.Auto;

            CodeModifier = null;

            Namespaces = new Dictionary<string, string>();
        }

        public LanguageOptions Language { get; set; }
        public FormatModeOptions FormatMode { get; set; }
        public DynamicProxyCodeModifier CodeModifier { get; set; }

        public bool UseCredential { get; set; }

        public String UserName { get; set; }
        public String Password { get; set; }
        public String Domain { get; set; }

        public Dictionary<String, String> Namespaces { get; set; }

        public override String ToString()
        {
            var text = $"DynamicProxyFactoryOptions[Language={Language},FormatMode={FormatMode},CodeModifier={CodeModifier}]";
            return text;
        }
    }
}
