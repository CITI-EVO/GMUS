using System;
using System.ComponentModel;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
	public class GridViewDataColumn : DevExpress.Web.GridViewDataColumn, IPermissionDependent
	{
		public GridViewDataColumn()
		{
			PermissionUtil.ApplyPermission(this);
		}

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		public String PermissionKey { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		public bool DisableIfNoAccess { get; set; }

		public bool Enabled { get; set; }

		public bool HasAccess()
		{
			return PermissionUtil.HasAccess(this);
		}
	}
}
