using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class FormControl : BaseUserControlExtend<FormModel>
    {
        public FormEntity FormEntity
        {
            get { return ViewState["Form"] as FormEntity; }
            set { ViewState["Form"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillElementsTree();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillElementsTree();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var orderIndex = (from n in elementsControl.Model.List
                              where n.ParentID == FormEntity.ID
                              select n.OrderIndex).Max();

            var model = new ElementModel
            {
                Visible = true,
                ParentType = "Form",
                OrderIndex = orderIndex.GetValueOrDefault() + 10
            };

            elementControl.Model = model;
            mpeElement.Show();
        }

        protected void btnElementOK_Click(object sender, EventArgs e)
        {
            var formEntity = FormEntity;
            if (formEntity == null)
            {
                formEntity = new FormEntity
                {
                    ID = Guid.NewGuid()
                };

                FormEntity = formEntity;
            }

            if (formEntity.Controls == null)
                formEntity.Controls = new List<ControlEntity>();

            var model = elementControl.Model;

            var collection = FormStructureUtil.PreOrderTraversal(formEntity);
            var dictionary = collection.ToDictionary(n => n.ID);

            var parentControl = (ContentEntity)formEntity;
            var childControl = dictionary.GetValueOrDefault(model.ID.GetValueOrDefault());

            if (model.ParentID != null)
                parentControl = (ContentEntity)dictionary.GetValueOrDefault(model.ParentID.GetValueOrDefault());

            parentControl.Controls = (parentControl.Controls ?? new List<ControlEntity>());

            if (childControl == null)
            {
                childControl = CreateNewEntity(model.ElementType);
                parentControl.Controls.Add(childControl);
            }

            if (model.ElementType == "Group")
            {
                var entity = (GroupEntity)childControl;
                entity.Size = model.GroupSize.GetValueOrDefault(12);
            }

            if (model.ElementType == "Field")
            {
                var entity = (FieldEntity)childControl;

                entity.Type = model.ControlType;
                entity.Tag = model.Tag;
                entity.Mask = model.Mask;
                entity.Enabled = model.Enabled;
                entity.Privacy = model.Privacy;
                entity.Mandatory = model.Mandatory;
                entity.Description = model.Description;
                entity.ValidationExp = model.ValidationExp;
                entity.DisplayOnGrid = model.DisplayOnGrid;
                entity.DataSourceID = model.DataSourceID;
                entity.TextExpression = model.TextExpression;
                entity.ValueExpression = model.ValueExpression;
            }

            childControl.Name = model.Name;
            childControl.Visible = model.Visible;
            childControl.OrderIndex = model.OrderIndex.GetValueOrDefault();
        }

        protected void elementControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeElement.Show();
        }

        protected void elementsControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var entity = FormEntity;
            if (entity == null || entity.Controls == null)
                return;

            var parentChildNodes = GetParentChildNode(e.Value, elementsControl.Model);
            if (parentChildNodes == null)
                return;

            var child = parentChildNodes[1];

            var orderIndex = (from n in elementsControl.Model.List
                              where n.ParentID == child.ID
                              select n.OrderIndex).Max();

            var model = new ElementModel
            {
                Visible = true,
                ParentID = child.ID,
                ParentType = child.ElementType,
                OrderIndex = orderIndex.GetValueOrDefault() + 10,
            };

            elementControl.Model = model;
            mpeElement.Show();
        }

        protected void elementsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = FormEntity;
            if (entity == null || entity.Controls == null)
                return;

            var parentChildNodes = GetParentChildNode(e.Value, elementsControl.Model);
            if (parentChildNodes == null)
                return;

            var parentNode = parentChildNodes[0];
            var childNode = parentChildNodes[1];

            var parentChildControls = GetParentChildControl(e.Value, childNode);

            var parentControl = parentChildControls[0];
            var childControl = parentChildControls[1];

            var model = new ElementModel
            {
                ID = childNode.ID,
                Name = childNode.Name,
                Visible = childNode.Visible,
                OrderIndex = childNode.OrderIndex,
                ElementType = childNode.ElementType,
            };

            if (parentNode == null)
            {
                model.ParentID = entity.ID;
                model.ParentType = "Form";
            }
            else
            {
                model.ParentID = parentNode.ID;
                model.ParentType = parentNode.ElementType;
            }

            if (childNode.ElementType == "Group")
            {
                var groupEntity = childControl as GroupEntity;
                if (groupEntity == null)
                    return;

                model.GroupSize = groupEntity.Size;
            }

            if (childNode.ElementType == "Field")
            {
                var fieldEntity = childControl as FieldEntity;
                if (fieldEntity == null)
                    return;

                model.Tag = fieldEntity.Tag;
                model.Mask = fieldEntity.Mask;
                model.Enabled = fieldEntity.Enabled;
                model.Privacy = fieldEntity.Privacy.GetValueOrDefault();
                model.Mandatory = fieldEntity.Mandatory.GetValueOrDefault();
                model.ControlType = fieldEntity.Type;
                model.Description = fieldEntity.Description;
                model.ValidationExp = fieldEntity.ValidationExp;
                model.DisplayOnGrid = fieldEntity.DisplayOnGrid;
                model.DataSourceID = fieldEntity.DataSourceID;
                model.TextExpression = fieldEntity.TextExpression;
                model.ValueExpression = fieldEntity.ValueExpression;
            }

            elementControl.Model = model;
            mpeElement.Show();
        }

        protected void elementsControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = FormEntity;
            if (entity == null || entity.Controls == null)
                return;

            var parentChildNodes = GetParentChildNode(e.Value, elementsControl.Model);
            if (parentChildNodes == null)
                return;

            var child = parentChildNodes[1];

            var items = FormStructureUtil.PreOrderTraversal(FormEntity);

            var parentChildControls = (from n in items
                                       where n.ID == child.ID ||
                                             n.ID == child.ParentID
                                       select n).ToDictionary(n => (Guid?)n.ID);

            var parentControl = (ContentEntity)FormEntity;
            var childControl = parentChildControls.GetValueOrDefault(child.ID.GetValueOrDefault());

            if (child.ParentID != null)
                parentControl = (ContentEntity)parentChildControls.GetValueOrDefault(child.ParentID);

            if (parentControl == null)
                return;

            if (!parentControl.Controls.Remove(childControl))
                return;
        }

        public override FormModel GetModel()
        {
            var model = base.GetModel();
            model.Entity = FormEntity;

            var entity = model.Entity;
            if (entity != null)
            {
                entity.Name = model.Name;
                entity.Visible = model.Visible.GetValueOrDefault();
                entity.OrderIndex = model.OrderIndex.GetValueOrDefault();
            }

            return model;
        }

        public override void SetModel(FormModel model)
        {
            FormEntity = model.Entity;
            base.SetModel(model);

            FillElementsTree();
        }

        protected void FillElementsTree()
        {
            var units = new ElementNodesModel
            {
                List = GetAllTreeNodes().ToList()
            };

            elementsControl.Model = units;
            elementsControl.DataBind();
        }

        protected ControlEntity CreateNewEntity(String elementType)
        {
            if (elementType == "Grid")
            {
                return new GridEntity
                {
                    ID = Guid.NewGuid(),
                };
            }

            if (elementType == "Group")
            {
                return new GroupEntity
                {
                    ID = Guid.NewGuid(),
                };
            }

            if (elementType == "TabPage")
            {
                return new TabPageEntity
                {
                    ID = Guid.NewGuid(),
                };
            }

            if (elementType == "TabContainer")
            {
                return new TabContainerEntity
                {
                    ID = Guid.NewGuid(),
                };
            }

            if (elementType == "Field")
            {
                return new FieldEntity
                {
                    ID = Guid.NewGuid(),
                };
            }

            return null;
        }

        protected IEnumerable<ElementTreeNodeEntity> GetAllTreeNodes()
        {
            var container = FormEntity as ContentEntity;
            if (container == null || container.Controls == null)
                return Enumerable.Empty<ElementTreeNodeEntity>();

            var collection = (from n in FormStructureUtil.CreateTree(container.Controls)
                              orderby n.OrderIndex, n.Name
                              select n);

            return collection;
        }

        protected ElementTreeNodeEntity[] GetParentChildNode(Guid? itemID, ElementNodesModel model)
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

        protected ControlEntity[] GetParentChildControl(Guid? itemID, ElementTreeNodeEntity node)
        {
            if (FormEntity == null || node == null)
                return null;

            var items = FormStructureUtil.PreOrderTraversal(FormEntity);

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