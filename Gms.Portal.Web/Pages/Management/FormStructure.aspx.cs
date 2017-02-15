using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.Web.Bases;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class FormStructure : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillFormTree();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new FormElementModel
            {
                ElementType = "Form"
            };

            formElementControl.Model = model;
            mpeFormElement.Show();
        }

        protected void btnFormElementOK_Click(object sender, EventArgs e)
        {
            var model = formElementControl.Model;

            var entityConverter = new FormEntityModelConverter(HbSession);
            var modelConverter = new FormModelEntityConverter(HbSession);

            var dbForm = (GM_Form)null;
            var formModel = (FormModel)null;

            if (model.ElementType == "Form")
            {
                dbForm = HbSession.Get<GM_Form>(model.ID.GetValueOrDefault());
                if (dbForm == null)
                {
                    dbForm = new GM_Form
                    {
                        ID = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                    };
                }

                dbForm.Name = model.Name;
                dbForm.Number = model.Number;
                dbForm.Language = model.Language;

                formModel = entityConverter.Convert(dbForm);

                var formEntity = formModel.FormEntity;
                if (formEntity == null)
                {
                    formEntity = new FormEntity
                    {
                        ID = dbForm.ID,
                        Name = dbForm.Name,
                    };

                    formModel.FormEntity = formEntity;
                }

                formEntity.Visible = model.Visible;

                modelConverter.FillObject(dbForm, formModel);

                HbSession.SubmitChanges(dbForm);

                FillFormTree();
                return;
            }

            var parentNode = formStructureControl.Model.List.First(n => n.ID == model.ParentID);

            dbForm = HbSession.Get<GM_Form>(parentNode.FormID.GetValueOrDefault());
            formModel = entityConverter.Convert(dbForm);

            var collection = FormStructureUtil.PreOrderTraversal(formModel.FormEntity);

            var childControl = collection.FirstOrDefault(n => n.ID == model.ID);

            var parentControl = collection.FirstOrDefault(n => n.ID == parentNode.ID);
            var parentContainer = (ContainerControlEntity)parentControl;

            parentContainer.Controls = (parentContainer.Controls ?? new List<ControlEntity>());

            if (model.ElementType == "Grid")
            {
                var entity = (GridEntity)childControl;
                if (entity == null)
                {
                    entity = new GridEntity
                    {
                        ID = Guid.NewGuid(),
                    };

                    parentContainer.Controls.Add(entity);
                }

                entity.Name = model.Name;
                entity.Visible = model.Visible;
            }
            else if (model.ElementType == "Field")
            {
                var entity = (FieldEntity)childControl;
                if (entity == null)
                {
                    entity = new FieldEntity
                    {
                        ID = Guid.NewGuid(),
                    };

                    parentContainer.Controls.Add(entity);
                }

                entity.Name = model.Name;
                entity.Type = model.ControlType;
                entity.Tag = model.Tag;
                entity.Mask = model.Mask;
                entity.Enabled = model.Enabled;
                entity.Visible = model.Visible;
                entity.OrderIndex = model.OrderIndex.GetValueOrDefault();
                //entity.IsMandatory = model.IsMandatory;
                entity.ValidationExp = model.ValidationExp;
            }
            else if (model.ElementType == "Group")
            {
                var entity = (GroupEntity)childControl;
                if (entity == null)
                {
                    entity = new GroupEntity
                    {
                        ID = Guid.NewGuid(),
                    };

                    parentContainer.Controls.Add(entity);
                }

                entity.Name = model.Name;
                entity.Visible = model.Visible;
            }
            else if (model.ElementType == "TabPage")
            {
                var entity = (TabPageEntity)childControl;
                if (entity == null)
                {
                    entity = new TabPageEntity
                    {
                        ID = Guid.NewGuid(),
                    };

                    parentContainer.Controls.Add(entity);
                }

                entity.Name = model.Name;
                entity.Visible = model.Visible;
            }
            else if (model.ElementType == "TabContainer")
            {
                var entity = (TabContainerEntity)childControl;
                if (entity == null)
                {
                    entity = new TabContainerEntity
                    {
                        ID = Guid.NewGuid(),
                    };

                    parentContainer.Controls.Add(entity);
                }

                entity.Name = model.Name;
                entity.Visible = model.Visible;
            }

            modelConverter.FillObject(dbForm, formModel);

            HbSession.SubmitChanges(dbForm);

            FillFormTree();
        }

        protected void formElementControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeFormElement.Show();
        }

        protected void formStructureControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var parentChildNodes = GetParentChildNode(e.Value, formStructureControl.Model);
            if (parentChildNodes == null)
                return;

            var child = parentChildNodes[1];

            var model = new FormElementModel
            {
                ParentID = child.ID,
                ParentType = child.Type
            };

            formElementControl.Model = model;
            mpeFormElement.Show();
        }

        protected void formStructureControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var parentChildNodes = GetParentChildNode(e.Value, formStructureControl.Model);
            if (parentChildNodes == null)
                return;

            var parentNode = parentChildNodes[0];
            var childNode = parentChildNodes[1];

            var parentChildControls = GetParentChildControl(e.Value, childNode);

            var parentControl = parentChildControls[0];
            var childControl = parentChildControls[1];

            var model = new FormElementModel
            {
                ID = childNode.ID,
                ElementType = childNode.Type,
                Name = childNode.Name,
            };

            if (parentNode != null)
            {
                model.ParentID = parentNode.ID;
                model.ParentType = parentNode.Type;
            }

            if (childNode.Type == "Form")
            {
                var form = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == childNode.ID);
                if (form == null)
                    return;

                model.Number = form.Number;
                model.Language = form.Language;
            }

            if (childNode.Type == "Field")
            {
                var fieldEntity = childControl as FieldEntity;
                if (fieldEntity == null)
                    return;

                model.ControlType = fieldEntity.Type;
                model.OrderIndex = fieldEntity.OrderIndex;
                model.Visible = fieldEntity.Visible;
                model.Enabled = fieldEntity.Enabled;
                model.Mask = fieldEntity.Mask;
                model.Tag = fieldEntity.Tag;
                model.ValidationExp = fieldEntity.ValidationExp;
            }

            formElementControl.Model = model;
            mpeFormElement.Show();
        }

        protected void formStructureControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var parentChildNodes = GetParentChildNode(e.Value, formStructureControl.Model);
            if (parentChildNodes == null)
                return;

            var child = parentChildNodes[1];
            if (child.Type == "Form")
            {
                var form = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == child.ID);
                if (form == null)
                    return;

                form.DateDeleted = DateTime.Now;

                HbSession.SubmitUpdate(form);
            }
            else
            {
                var form = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == child.FormID);
                if (form == null)
                    return;

                var entityConverter = new FormEntityModelConverter(HbSession);

                var model = entityConverter.Convert(form);
                if (model.FormEntity == null)
                    return;

                var items = FormStructureUtil.PreOrderTraversal(model.FormEntity);

                var parentChildControls = (from n in items
                                           where n.ID == child.ID ||
                                                 n.ID == child.ParentID
                                           select n).ToDictionary(n => (Guid?)n.ID);

                var childControl = parentChildControls.GetValueOrDefault(child.ID);
                var parentControl = parentChildControls.GetValueOrDefault(child.ParentID);

                var container = parentControl as ContainerControlEntity;
                if (container == null)
                    return;

                if (!container.Controls.Remove(childControl))
                    return;

                var modelConverter = new FormModelEntityConverter(HbSession);
                modelConverter.FillObject(form, model);

                HbSession.SubmitUpdate(form);
            }
        }

        protected void FillFormTree()
        {
            var units = new FormUnitsModel
            {
                List = GetAllTreeNodes().ToList()
            };

            formStructureControl.Model = units;
            formStructureControl.DataBind();
        }

        protected IEnumerable<FormTreeNodeEntity> GetAllTreeNodes()
        {
            var forms = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         orderby n.Name
                         select n).ToList();

            var converter = new FormEntityModelConverter(HbSession);
            var models = forms.Select(n => converter.Convert(n)).ToList();

            var collection = FormStructureUtil.CreateTree(models);
            return collection;
        }

        protected FormTreeNodeEntity[] GetParentChildNode(Guid? itemID, FormUnitsModel model)
        {
            var list = model.List;
            if (list == null)
                return null;

            var child = list.FirstOrDefault(n => n.ID == itemID);
            if (child == null)
                return null;

            var parent = list.FirstOrDefault(n => n.ID == child.ParentID);
            return new[] { parent, child };
        }

        protected ControlEntity[] GetParentChildControl(Guid? itemID, FormTreeNodeEntity node)
        {
            if (node == null)
                return null;

            var form = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == node.FormID);
            if (form == null)
                return null;

            var converter = new FormEntityModelConverter(HbSession);

            var entity = converter.Convert(form);
            if (entity.FormEntity == null)
                return null;

            var items = FormStructureUtil.PreOrderTraversal(entity.FormEntity);

            var parentChild = (from n in items
                               where n.ID == node.ID ||
                                     n.ID == node.ParentID
                               select n).ToDictionary(n => (Guid?)n.ID);

            var child = parentChild.GetValueOrDefault(node.ID);

            if (node.ParentID == null)
                return new[] { null, child };

            var parent = parentChild.GetValueOrDefault(node.ParentID);

            return new[] { parent, child };
        }
    }
}