using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Security.Common;
using CITI.EVO.Tools.Utils;

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

        [DefaultValue(true)]
        [Category("Accessibility")]
        public bool TableSectionHeader { get; set; }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            PrepareGrid();
            base.RenderContents(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            PrepareGrid();
            base.OnPreRender(e);
        }

        private void PrepareGrid()
        {
            if (TableSectionHeader)
            {
                if (HeaderRow != null)
                    HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            Translate();

            PermissionUtil.ApplyPermission(this);
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
                    var translatable = new DefaultTranslatable(dataColumn.HeaderText);
                    dataColumn.HeaderText = translatable.Text;

                    if (TranslationUtil.TranslationMode)
                        dataColumn.HeaderText = String.Concat(translatable.Text, translatable.Link);
                }
            }
        }
    }
}
