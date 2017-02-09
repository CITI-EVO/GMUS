using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Security.Configs;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.Tools.Security
{
	public static class PermissionUtil
	{
		private static readonly StringComparer defComparer;

		private static readonly Lazy<RulePermissionsEnum?> defaultRuleValue;
		private static readonly Lazy<PermissionElementSection> permissionElementSectionLazy;
		private static readonly Lazy<ILookup<String, PermissionElement>> permissionElementsLazy;

		static PermissionUtil()
		{
			defComparer = StringComparer.OrdinalIgnoreCase;

			defaultRuleValue = new Lazy<RulePermissionsEnum?>(LoadDefaulteRuleValue);
			permissionElementSectionLazy = new Lazy<PermissionElementSection>(LoadPermissionElementSection);
			permissionElementsLazy = new Lazy<ILookup<String, PermissionElement>>(LoadPermissionElements);
		}

		public static Guid? ModuleID
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.ModuleID;

				return null;
			}
		}

		public static String ModuleName
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.ModuleName;

				return null;
			}
		}

		public static String LoginPage
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.LoginPage;

				return null;
			}
		}

		public static String LogoutPage
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.LogoutPage;

				return null;
			}
		}

		public static String ChangePasswordPage
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.ChangePasswordPage;

				return null;
			}
		}

		public static bool IgnoreGroupMembership
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.IgnoreGroupMembership;

				return false;
			}
		}

		public static bool EnablePermissions
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.EnablePermissions;

				return false;
			}
		}

		public static bool EnabledPermissionsCache
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.EnabledPermissionsCache;

				return false;
			}
		}

		public static bool EnabledHierachycalSearch
		{
			get
			{
				if (ConfigSettings != null)
					return ConfigSettings.EnabledHierachycalSearch;

				return false;
			}
		}

		public static bool ResourcePathAutoGeneration
		{
			get
			{
				var configSection = ConfigSection;
				if (configSection != null && configSection.Settings != null)
					return configSection.Settings.ResourcePathAutoGeneration;

				return false;
			}
		}

		public static RulePermissionsEnum? DefaultRuleValue
		{
			get { return defaultRuleValue.Value; }
		}

		public static PermissionSettingsElement ConfigSettings
		{
			get
			{
				if (ConfigSection != null)
					return ConfigSection.Settings;

				return null;
			}
		}

		public static PermissionElementSection ConfigSection
		{
			get { return permissionElementSectionLazy.Value; }
		}

		public static void ApplyPermission(IPermissionDependent permissionDependent)
		{
			if (!EnablePermissions)
				return;

			if (permissionDependent == null)
				return;

			if (String.IsNullOrWhiteSpace(permissionDependent.PermissionKey))
			{
				if (DefaultRuleValue == null || !ResourcePathAutoGeneration)
					return;
			}

			if (permissionDependent.DisableIfNoAccess)
			{
				if (!permissionDependent.Enabled)
					return;
			}
			else
			{
				if (!permissionDependent.Visible)
					return;
			}

			var hasAccess = HasAccess(permissionDependent);
			if (permissionDependent.DisableIfNoAccess)
				permissionDependent.Enabled = hasAccess;
			else
				permissionDependent.Visible = hasAccess;
		}

		public static PermissionElement GetPermissionElement(String key)
		{
			var lookup = permissionElementsLazy.Value;
			if (lookup == null)
				return null;

			return lookup[key].FirstOrDefault();
		}

		public static void PreloadPermissionsIfEnabled(Control control)
		{
			if (ConfigSettings == null)
				return;

			if (!ConfigSettings.PreloadPermissions)
				return;

			PreloadPermissions(control);
		}

		public static void PreloadPermissions(Control control)
		{
			var permissions = GetAllPermissions(control).ToHashSet();
			UmUtil.Instance.PreloadPermissions(permissions);
		}

		public static RulePermissionsEnum ParseRuleValue(String ruleValue)
		{
			if (String.IsNullOrWhiteSpace(ruleValue))
				return RulePermissionsEnum.None;

			var array = ruleValue.Split('|');

			var permission = RulePermissionsEnum.None;
			foreach (var item in array)
				permission |= (RulePermissionsEnum)Enum.Parse(typeof(RulePermissionsEnum), item);

			return permission;
		}

		public static bool HasAccess(IPermissionDependent permissionDependent)
		{
			if (!EnablePermissions)
				return true;

			if (permissionDependent == null)
				throw new ArgumentNullException("permissionDependent");

			if (!String.IsNullOrWhiteSpace(permissionDependent.PermissionKey))
				return UmUtil.Instance.HasAccess(permissionDependent.PermissionKey);

			if (ResourcePathAutoGeneration && DefaultRuleValue != null)
			{
				var control = permissionDependent as Control;
				if (control != null)
				{
					var fullName = GetControlFullName(control);
					if (!String.IsNullOrWhiteSpace(fullName))
					{
						var hasAccess = UmUtil.Instance.HasAccess(fullName, DefaultRuleValue.Value);
						return hasAccess;
					}
				}
			}

			return true;
		}

		//public static bool HasAccess(IPermissionDependent permissionDependent)
		//{
		//	if (permissionDependent == null)
		//		throw new ArgumentNullException("permissionDependent");

		//	if (!String.IsNullOrWhiteSpace(permissionDependent.PermissionKey))
		//		return UmUtil.Instance.HasAccess(permissionDependent.PermissionKey);

		//	if (ResourcePathAutoGeneration && DefaultRuleValue != null)
		//	{
		//		var control = permissionDependent as Control;
		//		if (control == null)
		//			throw new Exception("Unable to genedate resource path for non Control inscances");

		//		var fullName = GetControlFullName(control);
		//		if (String.IsNullOrWhiteSpace(fullName))
		//			return true;

		//		var hasAccess = UmUtil.Instance.HasAccess(fullName, DefaultRuleValue.Value);
		//		return hasAccess;
		//	}

		//	throw new Exception("No permission key and DefaultRuleValue is null or ResourcePathAutoGeneration is off");
		//}

		private static String GetControlFullName(Control control)
		{
			var fullName = String.Empty;

			if (control.Page != null)
			{
				fullName = control.Page.AppRelativeVirtualPath;
				fullName = fullName.Replace("~/", String.Empty);
				fullName += "/";
			}


			fullName += control.UniqueID;
			fullName = fullName.Replace('$', '/');

			return fullName;
		}

		private static RulePermissionsEnum? LoadDefaulteRuleValue()
		{
			var configSection = ConfigSection;
			if (configSection == null)
				return null;

			var settings = configSection.Settings;
			if (settings == null)
				return null;

			var ruleValue = ParseRuleValue(settings.DefaulteRuleValue);
			return ruleValue;
		}

		private static PermissionElementSection LoadPermissionElementSection()
		{
			var configSection = (PermissionElementSection)ConfigurationManager.GetSection("permissionConfig");
			return configSection;
		}

		private static ILookup<String, PermissionElement> LoadPermissionElements()
		{
			if (ConfigSection == null || ConfigSection.Permissions == null)
				return null;

			var lookup = ConfigSection.Permissions.Cast<PermissionElement>().ToLookup(n => n.PermissionKey, defComparer);
			return lookup;
		}

		private static IEnumerable<String> GetAllPermissions(Control control)
		{
			var query = from n in ConfigSection.Permissions.Cast<PermissionElement>()
						where !string.IsNullOrWhiteSpace(n.ResourcePath)
						select n.ResourcePath;

			foreach (var path in query)
				yield return path;

			var configSection = ConfigSection;
			if (configSection == null)
				yield break;

			if (ConfigSettings == null)
				yield break;

			if (!ConfigSettings.ResourcePathAutoGeneration)
				yield break;

			query = from n in UserInterfaceUtil.TraverseChildren(control)
					let m = n as IPermissionDependent
					where m != null && string.IsNullOrWhiteSpace(m.PermissionKey)
					let fullName = GetControlFullName(n)
					where !string.IsNullOrWhiteSpace(fullName)
					select fullName;

			foreach (var path in query)
				yield return path;
		}
	}

}
