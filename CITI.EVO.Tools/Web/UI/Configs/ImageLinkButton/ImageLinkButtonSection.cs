using System.Configuration;

namespace CITI.EVO.Tools.Web.UI.Configs.ImageLinkButton
{
    internal class ImageLinkButtonSection : ConfigurationSection
    {
        [ConfigurationProperty("imageUrls", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ImageUrlElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        internal ImageUrlElementCollection ImageUrls
        {
            get
            {
                return (ImageUrlElementCollection)base["imageUrls"];
            }
        }
    }
}