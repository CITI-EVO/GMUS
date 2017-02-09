using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class RadioButtonList : System.Web.UI.WebControls.RadioButtonList, IPermissionDependent
    {
        #region properties
        
        [Category("Appearance")]
        [DefaultValue(true)]
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
		public RadioButtonList()
        {
            IncludeInResources = true;
        }
        #endregion

        #region events
        protected override void OnLoad(EventArgs e)
        {
        

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
