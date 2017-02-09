using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class DropDownList : System.Web.UI.WebControls.DropDownList, IPermissionDependent
    {
        #region properties

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

		#endregion

		#region constructors
		public DropDownList()
        {
            IncludeInResources = true;
        }
        #endregion

        #region events
        protected override void OnLoad(EventArgs e)
        {
            //base.OnLoad(e);
            //if (!IncludeInResources)
            //{
            //    return;
            //}

            //if (!(ViewState["Permissions"] is RulePermissionsEnum))
            //{
            //    var controlPath = FullPath(this);

            //    var resourcePermission = UmUtil.GetResourcePermission(controlPath);
            //    if (resourcePermission != null)
            //    {
            //        ViewState["Permissions"] = resourcePermission.RuleValue;
            //    }
            //}

        }
        #endregion

        #region Methods

        protected override void OnPreRender(EventArgs e)
        {
            if (Items.Count > 0 && DataSource == null)
            {
                foreach (ListItem item in Items)
                {
                    var listItemTrn = new DefaultTranslatable(item.Text);
                    item.Text = listItemTrn.Text;
                }
            }

			PermissionUtil.ApplyPermission(this);

			base.OnPreRender(e);
        }

		public bool HasAccess()
		{
			return PermissionUtil.HasAccess(this);
		}

		#endregion
	}
}
