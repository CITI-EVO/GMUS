using System;
using System.Configuration;

namespace CITI.EVO.Tools.Web.UI.Configs.ImageLinkButton
{
    internal class ImageUrlElementCollection : ConfigurationElementCollection
    {
        public ImageUrlElementCollection()
        {
        }

        public ImageUrlElement this[int index]
        {
            get
            {
                return (ImageUrlElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(ImageUrlElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ImageUrlElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ImageUrlElement)element).ImageKey;
        }

        public void Remove(ImageUrlElement element)
        {
            BaseRemove(element.ImageKey);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}