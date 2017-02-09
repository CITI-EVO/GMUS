using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class GridView : System.Web.UI.WebControls.GridView, IPermissionDependent
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        public String PermissionKey { get; set; }

		[Bindable(true)]
		[Category("Appearance")]
		[DefaultValue(true)]
		public bool DisableIfNoAccess { get; set; }

		protected override void RenderContents(HtmlTextWriter writer)
        {
            Translate();

			PermissionUtil.ApplyPermission(this);

			base.RenderContents(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            Translate();

			PermissionUtil.ApplyPermission(this);

			base.OnPreRender(e);
        }

		public bool HasAccess()
		{
			return PermissionUtil.HasAccess(this);
		}

		private void Translate()
        {
            foreach (var column in Columns)
            {
                var dataColumn = column as DataControlField;
                if (dataColumn != null && !String.IsNullOrWhiteSpace(dataColumn.HeaderText))
                {
                    var columnCaptionTrn = new DefaultTranslatable(dataColumn.HeaderText);
                    dataColumn.HeaderText = columnCaptionTrn.Text;
                }
            }
        }
    }
}
