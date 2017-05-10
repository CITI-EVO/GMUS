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
    public partial class CategoriesList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillCategoriesGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new CategoryModel();

            categoryControl.Model = model;
            mpeCategory.Show();
        }

        protected void categoriesControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Get<GM_Category>(e.Value);
            if (entity == null)
                return;

            var converter = new CategoryEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            categoryControl.Model = model;
            mpeCategory.Show();
        }

        protected void categoriesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Get<GM_Category>(e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillCategoriesGrid();
        }

        protected void categoriesControl_OnView(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void categoryControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeCategory.Show();
        }

        protected void btnCategoryOK_Click(object sender, EventArgs e)
        {
            var model = categoryControl.Model;

            var converter = new CategoryModelEntityConverter(HbSession);

            var entity = HbSession.Get<GM_Category>(model.ID);
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillCategoriesGrid();

            mpeCategory.Hide();
        }

        protected void btCategoryCancel_OnClick(object sender, EventArgs e)
        {
            mpeCategory.Hide();
        }

        private void FillCategoriesGrid()
        {
            var entities = (from n in HbSession.Query<GM_Category>()
                            where n.DateDeleted == null
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new CategoryEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new CategoriesModel();
            model.List = models;

            categoriesControl.Model = model;
            categoriesControl.DataBind();
        }
    }
}