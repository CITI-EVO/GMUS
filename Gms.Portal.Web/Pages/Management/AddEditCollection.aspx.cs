using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using NHibernate.Linq;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Entities.DataContainer;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditCollection : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var collectionID = DataConverter.ToNullableGuid(RequestUrl["CollectionID"]);
                if (collectionID == null)
                    return;

                var entity = HbSession.Query<GM_Collection>().FirstOrDefault(n => n.ID == collectionID);
                if (entity == null)
                    return;

                var converter = new CollectionEntityModelConverter(HbSession);
                var model = converter.Convert(entity);

                collectionControl.Model = model;
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/CollectionsList.aspx");
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var collectionID = DataConverter.ToNullableGuid(RequestUrl["CollectionID"]);

            var dbEntity = HbSession.Query<GM_Collection>().FirstOrDefault(n => n.ID == collectionID);

            var converter = new CollectionModelEntityConverter(HbSession);

            var model = collectionControl.Model;

            var entity = model.Entity;
            if (entity == null)
            {
                entity = new CollectionEntity();
                model.Entity = entity;
            }

            if (entity.Fields == null)
                entity.Fields = new List<FieldEntity>();

            if (dbEntity == null)
                dbEntity = converter.Convert(model);
            else
                converter.FillObject(dbEntity, model);

            HbSession.SubmitChanges(dbEntity);

            Response.Redirect("~/Pages/Management/CollectionsList.aspx");
        }
    }
}