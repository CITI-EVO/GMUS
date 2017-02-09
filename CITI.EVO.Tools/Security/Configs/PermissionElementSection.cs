using System.Configuration;

namespace CITI.EVO.Tools.Security.Configs
{
	public class PermissionElementSection : ConfigurationSection
	{
		[ConfigurationProperty("settings")]
		public PermissionSettingsElement Settings
		{
			get
			{
				return (PermissionSettingsElement)base["settings"];
			}
		}

		[ConfigurationProperty("permissions", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(PermissionElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
		public PermissionElementCollection Permissions
		{
			get
			{
				return (PermissionElementCollection)base["permissions"];
			}
		}
	}
}