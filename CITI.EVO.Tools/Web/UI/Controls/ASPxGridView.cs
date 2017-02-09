using System;
using System.ComponentModel;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Web.UI.Common;
using DevExpress.Web;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class ASPxGridView : DevExpress.Web.ASPxGridView, IPermissionDependent
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

            ApplyPermission();

            base.OnPreRender(e);
        }

        public bool HasAccess()
        {
            return PermissionUtil.HasAccess(this);
        }

        private void ApplyPermission()
        {
            PermissionUtil.ApplyPermission(this);

            foreach (var column in Columns)
            {
                var dependent = column as IPermissionDependent;
                if (dependent != null)
                    PermissionUtil.ApplyPermission(dependent);
            }
        }

        private void Translate()
        {
            if (Columns != null)
            {
                foreach (var column in Columns)
                {
                    var dxColumn = column as WebColumnBase;
                    if (dxColumn != null && !(dxColumn is ITranslatable) && !String.IsNullOrWhiteSpace(dxColumn.Caption))
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
                var filterBarClearTrn = new DefaultTranslatable(SettingsText.FilterBarClear);
                SettingsText.FilterBarClear = filterBarClearTrn.Text;

                var filterBarCreateFilterTrn = new DefaultTranslatable(SettingsText.FilterBarCreateFilter);
                SettingsText.FilterBarCreateFilter = filterBarCreateFilterTrn.Text;

                var filterControlPopupCaptionTrn = new DefaultTranslatable(SettingsText.FilterControlPopupCaption);
                SettingsText.FilterControlPopupCaption = filterControlPopupCaptionTrn.Text;

                var emptyDataRowTrn = new DefaultTranslatable(SettingsText.EmptyDataRow);
                SettingsText.EmptyDataRow = emptyDataRowTrn.Text;

                var headerFilterShowAllTrn = new DefaultTranslatable(SettingsText.HeaderFilterShowAll);
                SettingsText.HeaderFilterShowAll = headerFilterShowAllTrn.Text;

                var headerFilterShowBlanksTrn = new DefaultTranslatable(SettingsText.HeaderFilterShowBlanks);
                SettingsText.HeaderFilterShowBlanks = headerFilterShowBlanksTrn.Text;

                var headerFilterShowNonBlanksTrn = new DefaultTranslatable(SettingsText.HeaderFilterShowNonBlanks);
                SettingsText.HeaderFilterShowNonBlanks = headerFilterShowNonBlanksTrn.Text;

                var groupPanelTrn = new DefaultTranslatable(SettingsText.GroupPanel);
                SettingsText.GroupPanel = groupPanelTrn.Text;

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
