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
    public partial class FormsList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillFormsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/AddEditForm.aspx");
        }

        protected void formsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditForm.aspx");
            urlHelper["FormID"] = e.Value;

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formsControl_OnCopy(object sender, GenericEventArgs<Guid> e)
        {
            using (var transaction = HbSession.BeginTransaction())
            {
                var sourceEntity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == e.Value);
                if (sourceEntity == null)
                    return;

                var entityConverter = new FormEntityModelConverter(HbSession);
                var modelConverter = new FormModelEntityConverter(HbSession);

                var formModel = entityConverter.Convert(sourceEntity);

                var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity);
                foreach (var control in controls)
                    control.ID = Guid.NewGuid();

                formModel.Name = String.Format("{0} - Copy {1:dd.MM.yyyy HH:mm:ss}", formModel.Name, DateTime.Now);

                var newEntity = modelConverter.Convert(formModel);
                newEntity.ID = formModel.Entity.ID;

                HbSession.Save(newEntity);

                try
                {
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            FillFormsGrid();
        }

        protected void formsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillFormsGrid();
        }

        protected void formsControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/Management/AddEditForm.aspx");
            urlHelper["FormID"] = e.Value;

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        private void FillFormsGrid()
        {
            var entities = (from n in HbSession.Query<GM_Form>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new FormsModel();
            model.List = models;

            formsControl.Model = model;
            formsControl.DataBind();
        }

    }
}