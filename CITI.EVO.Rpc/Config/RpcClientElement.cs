using System;
using System.Configuration;

namespace CITI.EVO.Rpc.Config
{
    public class RpcClientElement : ConfigurationElement
    {
        [ConfigurationProperty("serverUrl", IsRequired = true)]
        public String ServerUrl
        {
            get
            {
                return (String)this["serverUrl"];
            }
            set
            {
                this["serverUrl"] = value;
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

        [ConfigurationProperty("userName", IsRequired = false, DefaultValue = null)]
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

        [ConfigurationProperty("password", IsRequired = false, DefaultValue = null)]
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

        [ConfigurationProperty("compression", IsRequired = false, DefaultValue = null)]
        public String Compression
        {
            get
            {
                return (String)this["compression"];
            }
            set
            {
                this["compression"] = value;
            }
        }

		[ConfigurationProperty("lazyCollection", IsRequired = false, DefaultValue = false)]
		public bool LazyCollection
		{
			get
			{
				return (bool)this["lazyCollection"];
			}
			set
			{
				this["lazyCollection"] = value;
			}
		}
	}
}
