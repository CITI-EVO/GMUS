using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringProjectFlawControl : BaseUserControlExtend<MonitoringProjectFlawItemModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        public void BindData(IEnumerable<MonitoringFlewEntity> flews)
        {
            if (flews == null)
                return;

            var list = (from n in flews
                        where n.Type == "Program"
                        orderby n.Name
                        select n);

            cbxFlaw.BindData(list);
        }

        public override MonitoringProjectFlawItemModel GetModel()
        {
            var model = base.GetModel();

            var items = (from n in cbxFlaw.Items.Cast<ListItem>()
                         let m = DataConverter.ToNullableGuid(n.Value)
                         where m != null && n.Selected
                         select m);

            model.FlawsID = items.ToHashSet();

            return model;
        }

        public override void SetModel(MonitoringProjectFlawItemModel model)
        {
            if (model.FlawsID != null)
            {
                foreach (var listItem in cbxFlaw.Items.Cast<ListItem>())
                {
                    var flewID = DataConverter.ToNullableGuid(listItem.Value);
                    listItem.Selected = model.FlawsID.Contains(flewID);
                }
            }

            base.SetModel(model);
        }

        protected void ApplyViewMode()
        {
            var status = cbxStatus.TryGetStringValue();

            pnlFlaw.Visible = (status == MonitoringItemStatuses.Rejected);
            pnlExpireDate.Visible = (status == MonitoringItemStatuses.Rejected);
        }
    }

}