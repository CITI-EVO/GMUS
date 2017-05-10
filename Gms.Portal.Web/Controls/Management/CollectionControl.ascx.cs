using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.CollectionStructure;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Common;
using Gms.Portal.Web.Models.Helpers;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class CollectionControl : BaseUserControlExtend<CollectionModel>
    {
        public CollectionEntity CollectionEntity
        {
            get { return ViewState["Collection"] as CollectionEntity; }
            set { ViewState["Collection"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillFieldsGrid();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillFieldsGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new NamedModel();

            namedControl.Model = model;
            mpeField.Show();
        }

        protected void fieldsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = CollectionEntity;
            if (entity == null || entity.Fields == null)
                return;

            var field = entity.Fields.FirstOrDefault(n => n.ID == e.Value);
            if (field == null)
                return;

            var model = new NamedModel
            {
                ID = field.ID,
                Name = field.Name
            };

            namedControl.Model = model;
            mpeField.Show();
        }

        protected void fieldsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = CollectionEntity;
            if (entity == null || entity.Fields == null)
                return;

            var field = entity.Fields.FirstOrDefault(n => n.ID == e.Value);
            if (field == null)
                return;

            CollectionEntity.Fields.Remove(field);
        }

        protected void namedControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeField.Show();
        }

        protected void btnFieldOK_Click(object sender, EventArgs e)
        {
            var model = namedControl.Model;
            if (String.IsNullOrWhiteSpace(model.Name))
                return;

            var entity = CollectionEntity;
            if (entity == null)
            {
                entity = new CollectionEntity();
                CollectionEntity = entity;
            }

            if (entity.Fields == null)
                entity.Fields = new List<FieldEntity>();

            if (model.ID == null)
            {
                var exists = (from n in entity.Fields
                              where n.Name == model.Name
                              select n).Any();

                if (exists)
                    return;
            }

            var field = entity.Fields.FirstOrDefault(n => n.ID == model.ID);
            if (field == null)
            {
                field = new FieldEntity
                {
                    ID = Guid.NewGuid(),
                };

                entity.Fields.Add(field);
            }

            field.Name = model.Name;

            mpeField.Hide();
        }

        protected void btFieldCancel_OnClick(object sender, EventArgs e)
        {
            mpeField.Hide();
        }

        public override CollectionModel GetModel()
        {
            var model = base.GetModel();
            model.Entity = CollectionEntity;

            if (model.Entity != null)
                model.Entity.Name = model.Name;

            return model;
        }

        public override void SetModel(CollectionModel model)
        {
            CollectionEntity = model.Entity;
            base.SetModel(model);
        }

        protected void FillFieldsGrid()
        {
            var entity = CollectionEntity;
            if (entity == null || entity.Fields == null)
                return;

            var units = new FieldUnitsModel
            {
                List = entity.Fields
            };

            fieldsControl.Model = units;
            fieldsControl.DataBind();
        }

    }
}