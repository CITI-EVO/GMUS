using System;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using System.Linq;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class ServicesList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditService.aspx")
            {
                ["Mode"] = "Edit",
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void servicesControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditService.aspx")
            {
                ["Mode"] = "View",
                ["ServiceID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void servicesControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditService.aspx")
            {
                ["Mode"] = "Edit",
                ["ServiceID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void servicesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Service>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillDataGrid();
        }

        private void FillDataGrid()
        {
            var entities = (from n in HbSession.Query<GM_Service>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new ServiceEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new ServicesModel
            {
                List = models
            };

            servicesControl.Model = model;
            servicesControl.DataBind();
        }
    }
}