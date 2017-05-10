using System;
using System.Collections.Generic;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User
{
    public partial class CloneFormGridControl : BaseUserControlExtend<CloneFormGridModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindDataGrids(IEnumerable<GridEntity> dataGrids)
        {
            cbxCloneDataGrid.DataSource = dataGrids;
            cbxCloneDataGrid.DataBind();
        }
    }
}