using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Converters.EntityToModel;
using CITI.EVO.UserManagement.Web.Converters.ModelToEntity;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class AttributesSchemasList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();
            FillAttributesTree();
        }

        protected void attributesSchemasControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var units = UmSchemasUtil.CreateListOfTree(HbSession);

            var item = units.FirstOrDefault(n => n.Key == e.Value);
            if (item == null)
                return;

            if (item.Type == "Project")
            {
                var entity = HbSession.Get<UM_Project>(item.ID);

                var model = new AttributeSchemaModel
                {
                    ProjectID = entity.ID
                };

                attributeSchemaControl.Model = model;
                mpeAttributeSchema.Show();
            }
            else if (item.Type == "Schema")
            {
                var entity = HbSession.Get<UM_AttributeSchema>(item.ID.GetValueOrDefault());

                var model = new AttributeFieldModel
                {
                    SchemaID = entity.ID
                };

                attributeFieldControl.Model = model;

                mpeAttributeSchemaNode.Show();
            }
        }

        protected void attributesSchemasControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var schema = HbSession.Query<UM_AttributeSchema>().FirstOrDefault(n => n.ID == e.Value);
            if (schema != null)
            {
                var converter = new AttributeSchemaEntityModelConverter(HbSession);
                var model = converter.Convert(schema);

                attributeSchemaControl.Model = model;
                mpeAttributeSchema.Show();
            }

            var field = HbSession.Query<UM_AttributeField>().FirstOrDefault(n => n.ID == e.Value);
            if (field != null)
            {
                var converter = new AttributeFieldEntityModelConverter(HbSession);
                var model = converter.Convert(field);

                attributeFieldControl.Model = model;

                mpeAttributeSchemaNode.Show();
            }
        }
        
        protected void attributesSchemasControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var units = UmSchemasUtil.CreateListOfTree(HbSession);

            var item = units.FirstOrDefault(n => n.Key == e.Value);
            if (item == null)
                return;

            if (item.Type == "Schema")
            {
                var attributeSchema = HbSession.Query<UM_AttributeSchema>().FirstOrDefault(n => n.ID == item.ID);
                if (attributeSchema != null)
                {
                    using (var transaction = HbSession.BeginTransaction())
                    {
                        attributeSchema.DateDeleted = DateTime.Now;
                        HbSession.Update(attributeSchema);

                        foreach (var attributeSchemaNode in attributeSchema.AttributeFields)
                        {
                            attributeSchemaNode.DateDeleted = (attributeSchemaNode.DateDeleted ?? attributeSchema.DateDeleted);
                            HbSession.Update(attributeSchemaNode);
                        }

                        transaction.Commit();
                    }
                }
            }
            else if (item.Type == "Field")
            {
                var attributeSchemaNode = HbSession.Query<UM_AttributeField>().FirstOrDefault(n => n.ID == item.ID);
                if (attributeSchemaNode != null)
                {
                    attributeSchemaNode.DateDeleted = DateTime.Now;
                    HbSession.SubmitChanges(attributeSchemaNode);
                }
            }

            FillAttributesTree();
        }

        protected void btAttributeSchemaOK_Click(object sender, EventArgs e)
        {
            var model = attributeSchemaControl.Model;

            var entity = HbSession.Get<UM_AttributeSchema>(model.ID.GetValueOrDefault());
            if (entity == null)
            {
                entity = new UM_AttributeSchema
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                };
            }

            var converter = new AttributeSchemaModelEntityConverter(HbSession);
            converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillAttributesTree();
        }

        protected void btAttributeSchemaCancel_OnClick(object sender, EventArgs e)
        {
            mpeAttributeSchema.Hide();
        }

        protected void btAttributeSchemaNodeOK_Click(object sender, EventArgs e)
        {
            var model = attributeFieldControl.Model;

            var entity = HbSession.Get<UM_AttributeField>(model.ID.GetValueOrDefault());
            if (entity == null)
            {
                entity = new UM_AttributeField
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                };
            }

            var converter = new AttributeFieldModelEntityConverter(HbSession);
            converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillAttributesTree();
        }

        protected void btAttributeSchemaNodeCancel_OnClick(object sender, EventArgs e)
        {
            mpeAttributeSchemaNode.Hide();
        }

        #region Methods

        protected void ApplyPermissions()
        {
            if (!UmUtil.Instance.HasAccess("AttributesSchemasList"))
                Response.Redirect("~/Pages/Management/UsersList.aspx");
        }

        protected void FillAttributesTree()
        {
            using (var session = Hb8Factory.CreateSession())
            {
                var attributesSchemas = (from n in session.Query<UM_AttributeSchema>()
                                         where n.DateDeleted == null
                                         select n).ToList();

                var converter = new AttributeSchemaEntityModelConverter(session);

                var model = new AttributeSchemasModel
                {
                    List = attributesSchemas.Select(n => converter.Convert(n)).ToList()
                };

                attributesSchemasControl.Model = model;
                attributesSchemasControl.DataBind();
            }
        }

        #endregion
    }
}