using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using System.Linq;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class CollectionDatasControl : BaseUserControlExtend<CollectionDatasModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(CollectionDatasModel model)
        {
            var columns = gvData.Columns;

            var existFields = columns.OfType<BoundField>().Select(n => n.DataField).ToHashSet();

            foreach (var pair in model.Fields)
            {
                var dataField = Convert.ToString(pair.Key);
                var visible = (dataField != FormDataConstants.IDField);

                if (existFields.Contains(dataField))
                    continue;

                var column = new BoundField
                {
                    HeaderText = pair.Value.TrimLen(25),
                    DataField = dataField,
                    Visible = visible,
                };

                gvData.Columns.Add(column);
            }

            base.SetModel(model);
        }
    }
}