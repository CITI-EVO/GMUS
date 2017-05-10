using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class RecipientGroupList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillRecipientGroupsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new RecipientGroupModel();

            recipientGroupControl.Model = model;
            mpeRecipientGroup.Show();
        }

        protected void recipientGroupsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditRecipient.aspx")
            {
                ["GroupID"] = e.Value
            };

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void recipientGroupsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == e.Value);
            if (entity == null)
                return;

            var converter = new RecipientGroupEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            recipientGroupControl.Model = model;
            mpeRecipientGroup.Show();
        }

        protected void recipientGroupsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillRecipientGroupsGrid();
        }

        protected void btnRecipientGroupOK_OnClick(object sender, EventArgs e)
        {
            var model = recipientGroupControl.Model;

            var converter = new RecipientGroupModelEntityConverter(HbSession);

            var entity = HbSession.Query<GM_RecipientGroup>().FirstOrDefault(n => n.ID == model.ID);
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillRecipientGroupsGrid();

            mpeRecipientGroup.Hide();
        }

        protected void btnRecipientGroupCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecipientGroup.Hide();
        }

        protected void FillRecipientGroupsGrid()
        {
            var entities = (from n in HbSession.Query<GM_RecipientGroup>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new RecipientGroupEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new RecipientGroupsModel();
            model.List = models;

            recipientGroupsControl.Model = model;
            recipientGroupsControl.DataBind();
        }
    }
}