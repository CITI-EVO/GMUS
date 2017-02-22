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

namespace Gms.Portal.Web.Pages.Management
{
    public partial class CollectionsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillCollectionsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/AddEditCollection.aspx");
        }

        protected void collectionsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditCollection.aspx");
            urlHelper["CollectionID"] = e.Value;

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void collectionsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Collection>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillCollectionsGrid();
        }

        protected void collectionsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/CollectionDataList.aspx");
            urlHelper["CollectionID"] = e.Value;

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        private void FillCollectionsGrid()
        {
            var entities = (from n in HbSession.Query<GM_Collection>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new CollectionEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new CollectionsModel();
            model.List = models;

            collectionsControl.Model = model;
            collectionsControl.DataBind();
        }
    }
}