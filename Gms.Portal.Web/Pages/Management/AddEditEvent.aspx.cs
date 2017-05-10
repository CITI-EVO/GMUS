using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.CollectionStructure;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Entities.EventStructure;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditEvent : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var eventID = DataConverter.ToNullableGuid(RequestUrl["EventID"]);
                if (eventID == null)
                    return;

                var entity = HbSession.Query<GM_Event>().FirstOrDefault(n => n.ID == eventID);
                if (entity == null)
                    return;

                var converter = new EventEntityModelConverter(HbSession);
                var model = converter.Convert(entity);

                regEventControl.Model = model;
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/EventsList.aspx");
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            var eventID = DataConverter.ToNullableGuid(RequestUrl["EventID"]);

            var dbEntity = HbSession.Query<GM_Event>().FirstOrDefault(n => n.ID == eventID);

            var converter = new EventModelEntityConverter(HbSession);

            var model = regEventControl.Model;

            var entity = model.Entity;
            if (entity == null)
            {
                entity = new EventEntity();
                model.Entity = entity;
            }

            if (entity.Phases == null)
                entity.Phases = new List<PhaseEntity>();

            if (dbEntity == null)
                dbEntity = converter.Convert(model);
            else
                converter.FillObject(dbEntity, model);

            HbSession.SubmitChanges(dbEntity);

            Response.Redirect("~/Pages/Management/EventsList.aspx");
        }
    }
}