using System;
using System.Configuration;

namespace CITI.EVO.Tools.Security.Configs
{
	public class PermissionSettingsElement : ConfigurationElement
	{
		[ConfigurationProperty("moduleID", IsRequired = true)]
		public Guid? ModuleID
		{
			get
			{
				return (Guid?)this["moduleID"];
			}
			set
			{
				this["moduleID"] = value;
			}
		}

		[ConfigurationProperty("loginPage", IsRequired = true)]
		public String LoginPage
		{
			get
			{
				return (String)this["loginPage"];
			}
			set
			{
				this["loginPage"] = value;
			}
		}

		[ConfigurationProperty("logoutPage", IsRequired = true)]
		public String LogoutPage
		{
			get
			{
				return (String)this["logoutPage"];
			}
			set
			{
				this["logoutPage"] = value;
			}
		}

		[ConfigurationProperty("changePasswordPage", IsRequired = true)]
		public String ChangePasswordPage
		{
			get
			{
				return (String)this["changePasswordPage"];
			}
			set
			{
				this["changePasswordPage"] = value;
			}
		}

		[ConfigurationProperty("moduleName", IsRequired = false)]
		public String ModuleName
		{
			get
			{
				return (String)this["moduleName"];
			}
			set
			{
				this["moduleName"] = value;
			}
		}

		[ConfigurationProperty("preloadPermissions", IsRequired = false, DefaultValue = false)]
		public bool PreloadPermissions
		{
			get
			{
				return (bool)this["preloadPermissions"];
			}
			set
			{
				this["preloadPermissions"] = value;
			}
		}

		[ConfigurationProperty("resourcePathAutoGeneration", IsRequired = false, DefaultValue = false)]
		public bool ResourcePathAutoGeneration
		{
			get
			{
				return (bool)this["resourcePathAutoGeneration"];
			}
			set
			{
				this["resourcePathAutoGeneration"] = value;
			}
		}

		[ConfigurationProperty("defaulteRuleValue", IsRequired = false)]
		public String DefaulteRuleValue
		{
			get
			{
				return (String)this["defaulteRuleValue"];
			}
			set
			{
				this["defaulteRuleValue"] = value;
			}
		}

		[ConfigurationProperty("ignoreGroupMembership", IsRequired = false, DefaultValue = false)]
		public bool IgnoreGroupMembership
		{
			get
			{
				return (bool)this["ignoreGroupMembership"];
			}
			set
			{
				this["ignoreGroupMembership"] = value;
			}
		}

		[ConfigurationProperty("enablePermissions", IsRequired = false, DefaultValue = true)]
		public bool EnablePermissions
		{
			get
			{
				return (bool)this["enablePermissions"];
			}
			set
			{
				this["enablePermissions"] = value;
			}
		}

		[ConfigurationProperty("enabledHierachycalSearch", IsRequired = false, DefaultValue = true)]
		public bool EnabledHierachycalSearch
		{
			get
			{
				return (bool)this["enabledHierachycalSearch"];
			}
			set
			{
				this["enabledHierachycalSearch"] = value;
			}
		}

		[ConfigurationProperty("enabledPermissionsCache", IsRequired = false, DefaultValue = true)]
		public bool EnabledPermissionsCache
		{
			get
			{
				return (bool)this["enabledPermissionsCache"];
			}
			set
			{
				this["enabledPermissionsCache"] = value;
			}
		}

	}
}