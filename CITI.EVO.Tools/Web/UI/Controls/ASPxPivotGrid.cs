using System;
using System.ComponentModel;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using DevExpress.XtraPivotGrid;

namespace CITI.EVO.Tools.Web.UI.Controls
{
	public class ASPxPivotGrid : DevExpress.Web.ASPxPivotGrid.ASPxPivotGrid, IPermissionDependent
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
			if (Fields != null)
			{
				foreach (var field in Fields)
				{
					var dxField = field as PivotGridFieldBase;
					if (dxField != null && !String.IsNullOrWhiteSpace(dxField.Caption))
					{
						var columnCaptionTrn = new DefaultTranslatable(dxField.Caption);
						dxField.Caption = columnCaptionTrn.Text;
					}
				}
			}

			if (SettingsLoadingPanel != null)
			{
				var loadingPanelTrn = new DefaultTranslatable(SettingsLoadingPanel.Text);
				SettingsLoadingPanel.Text = loadingPanelTrn.Text;
			}

			var summaryTextTrn = new DefaultTranslatable(SummaryText);
			SummaryText = summaryTextTrn.Text;
		}
	}
}
