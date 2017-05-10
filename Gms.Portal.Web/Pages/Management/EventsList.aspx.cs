using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class EventsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillEventsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/AddEditEvent.aspx");
        }

        protected void eventsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditEvent.aspx")
            {
                ["EventID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void eventsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Event>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillEventsGrid();
        }

        protected void eventsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            //var urlHelper = new UrlHelper("~/Pages/Management/CollectionDataList.aspx")
            //{
            //    ["EventID"] = e.Value
            //};

            //Response.Redirect(urlHelper.ToEncodedUrl());
        }

        private void FillEventsGrid()
        {
            var entities = (from n in HbSession.Query<GM_Event>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new EventEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new EventsModel();
            model.List = models;

            eventsControl.Model = model;
            eventsControl.DataBind();
        }
    }
}