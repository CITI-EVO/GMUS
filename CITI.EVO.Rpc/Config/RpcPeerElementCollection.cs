using System;
using System.Configuration;

namespace CITI.EVO.Rpc.Config
{
    public class RpcPeerElementCollection : ConfigurationElementCollection
    {
        public RpcPeerElement this[int index]
        {
            get
            {
                return (RpcPeerElement)BaseGet(index);
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

        public void Add(RpcPeerElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RpcPeerElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RpcPeerElement)element).Name;
        }

        public void Remove(RpcPeerElement element)
        {
            BaseRemove(element.Name);
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
