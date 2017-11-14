using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Utils;
using DevExpress.Web.ASPxTreeList;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class MonitoringPrintFieldControl : BaseUserControl<MonitoringPrintFieldsModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlExcel.Visible = ddlPrintType.SelectedValue == "Excel";
            pnlTemplate.Visible = ddlPrintType.SelectedValue == "PDF";

        }

    }
}