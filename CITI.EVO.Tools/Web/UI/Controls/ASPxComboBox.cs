using System;
using System.ComponentModel;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Web.UI.Common;
using DevExpress.Web;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ASPxComboBox : DevExpress.Web.ASPxComboBox, IPermissionDependent
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
		public ASPxComboBox()
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
            if (Items != null && Items.Count > 0 && DataSource == null)
            {
                foreach (ListEditItem item in Items)
                {
                    var listItemTrn = new DefaultTranslatable(item.Text);
                    item.Text = listItemTrn.Text;
                }
            }

            if (Columns != null && Columns.Count > 0)
            {
                foreach (ListBoxColumn column in Columns)
                {
                    var caption = column.Caption;

                    if (String.IsNullOrWhiteSpace(caption))
                    {
                        caption = column.FieldName;
                    }

                    var captionTrn = new DefaultTranslatable(caption);
                    column.Caption = captionTrn.Text;
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