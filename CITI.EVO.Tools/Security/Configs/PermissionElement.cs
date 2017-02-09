using System;
using System.Configuration;

namespace CITI.EVO.Tools.Security.Configs
{
	public class PermissionElement : ConfigurationElement
	{
		[ConfigurationProperty("permissionKey", IsRequired = true)]
		public String PermissionKey
		{
			get
			{
				return (String)this["permissionKey"];
			}
			set
			{
				this["permissionKey"] = value;
			}
		}

		[ConfigurationProperty("resourcePath", IsRequired = true)]
		public String ResourcePath
		{
			get
			{
				return (String)this["resourcePath"];
			}
			set
			{
				this["resourcePath"] = value;
			}
		}

		[ConfigurationProperty("ruleValue", IsRequired = true)]
		public String RuleValue
		{
			get
			{
				return (String)this["ruleValue"];
			}
			set
			{
				this["ruleValue"] = value;
			}
		}
	}

}
