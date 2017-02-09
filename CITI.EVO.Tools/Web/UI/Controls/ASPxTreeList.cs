using System;
using System.ComponentModel;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using DevExpress.Web;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ASPxTreeList : DevExpress.Web.ASPxTreeList.ASPxTreeList, IPermissionDependent
    {
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
            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    var dxColumn = column as WebColumnBase;
                    if (dxColumn != null && !String.IsNullOrWhiteSpace(dxColumn.Caption))
                    {
                        var columnCaptionTrn = new DefaultTranslatable(dxColumn.Caption);
                        dxColumn.Caption = columnCaptionTrn.Text;
                    }
                }
            }

            if (SettingsLoadingPanel != null)
            {
                var loadingPanelTrn = new DefaultTranslatable(SettingsLoadingPanel.Text);
                SettingsLoadingPanel.Text = loadingPanelTrn.Text;
            }

            if (SettingsText != null)
            {
                var loadingPanelTextTrn = new DefaultTranslatable(SettingsText.LoadingPanelText);
                SettingsText.LoadingPanelText = loadingPanelTextTrn.Text;

                var commandCancelTrn = new DefaultTranslatable(SettingsText.CommandCancel);
                SettingsText.CommandCancel = commandCancelTrn.Text;

                var commandUpdateTrn = new DefaultTranslatable(SettingsText.CommandUpdate);
                SettingsText.CommandUpdate = commandUpdateTrn.Text;

                var commandEditTrn = new DefaultTranslatable(SettingsText.CommandEdit);
                SettingsText.CommandEdit = commandEditTrn.Text;

                var commandDeleteTrn = new DefaultTranslatable(SettingsText.CommandDelete);
                SettingsText.CommandDelete = commandDeleteTrn.Text;
            }

            if (SettingsPager != null && SettingsPager.Summary != null)
            {
                var summaryTextTrn = new DefaultTranslatable(SettingsPager.Summary.Text);
                SettingsPager.Summary.Text = summaryTextTrn.Text;
            }
        }
    }
}
