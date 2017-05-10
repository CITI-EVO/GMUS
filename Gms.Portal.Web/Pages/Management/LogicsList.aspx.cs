using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Bases;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class LogicsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillLogicsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/AddEditLogic.aspx");
        }

        protected void logicsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditLogic.aspx")
            {
                ["LogicID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void logicsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditLogic.aspx")
            {
                ["LogicID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void logicsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Logic>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillLogicsGrid();
        }

        private void FillLogicsGrid()
        {
            var entities = (from n in HbSession.Query<GM_Logic>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new LogicEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new LogicsModel();
            model.List = models;

            logicsControl.Model = model;
            logicsControl.DataBind();
        }
    }
}