using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class LogicControl : BaseUserControlExtend<LogicModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var model = Model;

            FillLists(model);
            ApplyViewMode(model);
        }

        private void FillLists(LogicModel model)
        {
            if (model.SourceType == "Form")
            {
                var query = (from n in HbSession.Query<GM_Form>()
                             where n.DateDeleted == null
                             orderby n.Name
                             select n);

                var list = query.ToList();

                cbxSource.DataSource = list;
                cbxSource.DataBind();
            }

            if (model.SourceType == "Logic")
            {
                var query = (from n in HbSession.Query<GM_Logic>()
                             where n.DateDeleted == null
                             orderby n.Name
                             select n);

                var list = query.ToList();

                cbxSource.DataSource = list;
                cbxSource.DataBind();
            }

            cbxSource.TrySetSelectedValue(model.SourceID);
        }

        private void ApplyViewMode(LogicModel model)
        {
            if (model.Type == "Logic")
            {
                pnlLogic.Visible = true;
                pnlQuery.Visible = false;
            }
            else if (model.Type == "Query")
            {
                pnlLogic.Visible = false;
                pnlQuery.Visible = true;
            }
        }

        public override void SetModel(object model)
        {
            var logicModel = (LogicModel)model;

            FillLists(logicModel);
            ApplyViewMode(logicModel);
        }
    }
}