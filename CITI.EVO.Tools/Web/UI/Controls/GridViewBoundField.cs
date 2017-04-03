using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Web.UI.Helpers;

namespace CITI.EVO.Tools.Web.UI.Controls
{
    public class GridViewBoundField : TemplateField
    {
        public String DataField { get; set; }

        public override bool Initialize(bool sortingEnabled, Control control)
        {
            HeaderTemplate = new GridViewFieldTemplate(GetHeaderControl);
            ItemTemplate = new GridViewFieldTemplate(GetItemControl);

            return base.Initialize(sortingEnabled, control);
        }

        protected Control GetHeaderControl()
        {
            var label = new Label
            {
                Text = HeaderText
            };

            return label;
        }

        protected Control GetItemControl()
        {
            var label = new System.Web.UI.WebControls.Label();
            label.DataBinding += label_DataBinding;

            return label;
        }

        private void label_DataBinding(object sender, EventArgs e)
        {
            var label = sender as System.Web.UI.WebControls.Label;
            if (label == null)
                return;

            var parentGridRow = label.NamingContainer as GridViewRow;
            if (parentGridRow == null)
                return;

            var value = GetFieldValue(parentGridRow.DataItem);
            label.Text = Convert.ToString(value);
        }

        private Object GetFieldValue(Object source)
        {
            var value = DataBinder.Eval(source, DataField);
            return value;
        }
    }
}