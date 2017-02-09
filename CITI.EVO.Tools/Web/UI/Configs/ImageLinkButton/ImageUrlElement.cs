using System;
using System.Configuration;

namespace CITI.EVO.Tools.Web.UI.Configs.ImageLinkButton
{
    internal class ImageUrlElement : ConfigurationElement
    {
        [ConfigurationProperty("imageKey", IsRequired = true)]
        public String ImageKey
        {
            get
            {
                return (String)this["imageKey"];
            }
            set
            {
                this["imageKey"] = value;
            }
        }

        [ConfigurationProperty("defaultImageUrl", IsRequired = true)]
        public String DefaultImageUrl
        {
            get
            {
                return (String)this["defaultImageUrl"];
            }
            set
            {
                this["defaultImageUrl"] = value;
            }
        }

        [ConfigurationProperty("disabledImageUrl", IsRequired = true)]
        public String DisabledImageUrl
        {
            get
            {
                return (String)this["disabledImageUrl"];
            }
            set
            {
                this["disabledImageUrl"] = value;
            }
        }
    }
}