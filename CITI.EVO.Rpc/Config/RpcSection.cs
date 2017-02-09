using System.Configuration;

namespace CITI.EVO.Rpc.Config
{
    public class RpcSection : ConfigurationSection
    {
        [ConfigurationProperty("client")]
        public RpcClientElement Client
        {
            get
            {
                return (RpcClientElement)base["client"];
            }
        }

        [ConfigurationProperty("peers", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RpcPeerElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public RpcPeerElementCollection Peers
        {
            get
            {
                return (RpcPeerElementCollection)base["peers"];
            }
        }
    }
}
