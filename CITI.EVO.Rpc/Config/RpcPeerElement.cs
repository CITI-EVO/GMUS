using System;
using System.Configuration;

namespace CITI.EVO.Rpc.Config
{
    public class RpcPeerElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("requestTimeout", IsRequired = false, DefaultValue = 0)]
        public int RequestTimeout
        {
            get
            {
                return (int)this["requestTimeout"];
            }
            set
            {
                this["requestTimeout"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public String Url
        {
            get
            {
                return (String)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }

        [ConfigurationProperty("userName", IsRequired = true)]
        public String UserName
        {
            get
            {
                return (String)this["userName"];
            }
            set
            {
                this["userName"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public String Password
        {
            get
            {
                return (String)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("encryption", IsRequired = false)]
        public String Encryption
        {
            get
            {
                return (String)this["encryption"];
            }
            set
            {
                this["encryption"] = value;
            }
        }

        [ConfigurationProperty("compression", IsRequired = false)]
        public bool Compression
        {
            get
            {
                return (bool)this["compression"];
            }
            set
            {
                this["compression"] = value;
            }
        }
    }

}
