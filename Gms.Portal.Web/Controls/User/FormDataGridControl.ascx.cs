using System;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FormDataGridControl : BaseUserControlExtend<FormDataGridModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public override void SetModel(FormDataGridModel model)
        {
            var columns = gvData.Columns;

            var existFields = columns.OfType<BoundField>().Select(n => n.DataField).ToHashSet();

            foreach (var field in model.Fields)
            {
                var dataField = Convert.ToString(field.ID);
                if (existFields.Contains(dataField))
                    continue;

                var column = new BoundField
                {
                    HeaderText = field.Name.TrimLen(25),
                    DataField = Convert.ToString(field.ID)
                };

                gvData.Columns.Add(column);
            }

            base.SetModel(model);
        }
    }
}