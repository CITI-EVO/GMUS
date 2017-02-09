using System;
using System.ComponentModel;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class Panel : System.Web.UI.WebControls.Panel, IPermissionDependent
    {
        [Category("Appearance"), DefaultValue(true)]
        public bool IncludeInResources { get; set; }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		public bool DisableIfNoAccess { get; set; }

		protected override void OnPreRender(EventArgs e)
        {
			PermissionUtil.ApplyPermission(this);

			base.OnPreRender(e);
        }

		public bool HasAccess()
		{
			return PermissionUtil.HasAccess(this);
		}
	}
}
