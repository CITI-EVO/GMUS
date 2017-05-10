using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
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
using NHibernate.Linq;

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
            FillCategoreisList();
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

            elementControl.FillDependentFields(FormEntity);
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
                entity.BgColor = model.GroupBgColor;
                entity.TextColor = model.GroupTextColor;
            }

            if (model.ElementType == "Grid")
            {
                var entity = (GridEntity)childControl;
                entity.ValidationExp = model.ValidationExp;
                entity.ErrorMessage = model.ErrorMessage;
            }

            if (model.ElementType == "Tree")
            {
                var entity = (TreeEntity)childControl;
                entity.ValidationExp = model.ValidationExp;
                entity.ErrorMessage = model.ErrorMessage;
            }

            if (model.ElementType == "Field")
            {
                var entity = (FieldEntity)childControl;

                entity.Tag = model.Tag;
                entity.Type = model.ControlType;
                entity.Mask = model.Mask;
                entity.Unique = model.Unique;
                entity.Privacy = model.Privacy;
                entity.ReadOnly = model.ReadOnly;
                entity.Inversion = model.Inversion;
                entity.Mandatory = model.Mandatory;
                entity.Parameters = model.Parameters;
                entity.Description = model.Description;
                entity.CaptionSize = model.CaptionSize;
                entity.ControlSize = model.ControlSize;
                entity.ErrorMessage = model.ErrorMessage;
                entity.FilterByUser = model.FilterByUser;
                entity.DependentExp = model.DependentExp;
                entity.DataSourceID = model.DataSourceID;
                entity.ValidationExp = model.ValidationExp;
                entity.DisplayOnGrid = model.DisplayOnGrid;
                entity.TextExpression = model.TextExpression;
                entity.DisplayOnFilter = model.DisplayOnFilter;
                entity.ValueExpression = model.ValueExpression;
                entity.RequiresApproval = model.RequiresApproval;
                entity.DependentFillExp = model.DependentFillExp;
                entity.GridFieldSummary = model.GridFieldSummary;
                entity.DependentFieldID = model.DependentFieldID;
                entity.DataSourceSortExp = model.DataSourceSortExp;
                entity.DataSourceFilterExp = model.DataSourceFilterExp;
                entity.FieldValueExpression = model.FieldValueExpression;
            }

            childControl.Name = model.Name;
            childControl.Alias = model.Alias;
            childControl.Visible = model.Visible.GetValueOrDefault();
            childControl.OrderIndex = model.OrderIndex.GetValueOrDefault();
            childControl.DependentExp = model.DependentExp;
            childControl.NotPrintable = childControl.NotPrintable;
            childControl.FirstTimeFill = model.FirstTimeFill.GetValueOrDefault();
            childControl.DependentFieldID = model.DependentFieldID;

            mpeElement.Hide();
        }

        protected void elementsControl_OnNew(object sender, GenericEventArgs<Guid> e)
        {
            var entity = FormEntity;
            if (entity == null || entity.Controls == null)
                return;

            var parentChildNodes = GetParentChildNode(e.Value, elementsControl.Model);
            if (parentChildNodes == null)
                return;

            var child = parentChildNodes.Child;

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

            elementControl.FillDependentFields(entity);
            elementControl.Model = model;

            mpeElement.Show();
        }

        protected void elementsControl_Copy(object sender, GenericEventArgs<Guid> e)
        {
            var tree = FormStructureUtil.CreateTree(FormEntity);

            var collection = FormStructureUtil.PreOrderTraversal(FormEntity);
            var dictionary = collection.ToDictionary(n => n.ID);

            var sourceControl = tree.FirstOrDefault(n => n.ID == e.Value);

            var childControl = dictionary.GetValueOrDefault(e.Value) as ControlEntity;
            var xmlData = XmlUtil.Serialize(childControl);

            var newControl = XmlUtil.Deserialize<ControlEntity>(xmlData);
            newControl.Name = $"{childControl.Name} - Copy {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
            newControl.OrderIndex = childControl.OrderIndex + 10;

            if (newControl is ContentEntity)
            {
                var controls = FormStructureUtil.PreOrderTraversal(newControl);
                foreach (var control in controls)
                    control.ID = Guid.NewGuid();
            }
            else
            {
                newControl.ID = Guid.NewGuid();
            }
            var parentControl = (ContentEntity)FormEntity;
            if (sourceControl.ParentID != null)
                parentControl = (ContentEntity)dictionary.GetValueOrDefault(sourceControl.ParentID.GetValueOrDefault());

            parentControl.Controls = (parentControl.Controls ?? new List<ControlEntity>());
            parentControl.Controls.Add(newControl);
        }

        protected void elementsControl_Paste(object sender, GenericEventArgs<Guid> e)
        {
            var model = new ElementPasteModel();
            model.DestinationId = e.Value;
            var forms = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         orderby n.OrderIndex
                         select n).ToList();

            model.Forms = forms;
            elementPasteControl.Model = model;
            mpeElementPaste.Show();
        }
        protected void elementPasteControl_OnDataChanged(object sender, EventArgs e)
        {
            var model = elementPasteControl.Model;
            model.FormTree = GetPossibleMovingElements(model.FormId).ToList();
            elementPasteControl.Model = model;

            mpeElementPaste.Show();
        }
        protected void btnElementPasteOK_Click(object sender, EventArgs e)
        {
            var model = elementPasteControl.Model;

            var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == model.FormId);
            if (entity == null)
                return;

            if (model.ElementId == null)
                return;

            if (model.DestinationId == null)
                return;

            var elementId = model.ElementId.Value;
            var destinationId = model.DestinationId.Value;

            var converter = new FormEntityModelConverter(HbSession);
            var formModel = converter.Convert(entity);

            var sourceCollection = FormStructureUtil.PreOrderTraversal(formModel.Entity).ToList();
            var sourceDictionary = sourceCollection.ToDictionary(n => n.ID);

            var destinationCollection = FormStructureUtil.PreOrderTraversal(FormEntity).ToList();
            var destinationDictionary = destinationCollection.ToDictionary(n => n.ID);

            var childControl = sourceDictionary.GetValueOrDefault(elementId) as ControlEntity;
            var xmlData = XmlUtil.Serialize(childControl);

            var newControl = XmlUtil.Deserialize<ControlEntity>(xmlData);
            newControl.Name = $"{childControl.Name} - Copy {DateTime.Now:dd.MM.yyyy HH:mm:ss}";

            if (newControl is ContentEntity)
            {
                var controls = FormStructureUtil.PreOrderTraversal(newControl);
                foreach (var control in controls)
                    control.ID = Guid.NewGuid();
            }
            else
            {
                newControl.ID = Guid.NewGuid();
            }

            var parentControl = (ContentEntity)destinationDictionary.GetValueOrDefault(destinationId);

            var result = CanMoveElement(parentControl, newControl);
            if (result)
            {
                parentControl.Controls = (parentControl.Controls ?? new List<ControlEntity>());
                parentControl.Controls.Add(newControl);
            }

            mpeElementMove.Hide();
        }
        protected void btnElementPasteCancel_Click(object sender, EventArgs e)
        {
            mpeElementPaste.Hide();
        }

        protected void elementsControl_OnMove(object sender, GenericEventArgs<Guid> e)
        {
            var formTree = GetPossibleMovingElements(FormEntity).ToList();

            var model = new ElementMoveModel
            {
                ElementId = e.Value,
                FormTree = formTree
            };

            elementMoveControl.Model = model;
            mpeElementMove.Show();
        }
        protected void btnElementMoveOK_OnClick(object sender, EventArgs e)
        {
            var model = elementMoveControl.Model;

            var tree = FormStructureUtil.CreateTree(FormEntity);
            var fields = FormStructureUtil.InOrderTraversal(FormEntity).ToList();

            var childControl = tree.FirstOrDefault(n => n.ID == model.ElementId);
            var destinationControl = fields.FirstOrDefault(n => n.ID == model.DestinationId) as ContentEntity;

            if (childControl == null || destinationControl == null)
                return;

            var oldParent = fields.FirstOrDefault(n => n.ID == childControl.ParentID) as ContentEntity;
            var child = fields.FirstOrDefault(n => n.ID == childControl.ID);

            if (oldParent == null || child == null)
                return;

            var result = CanMoveElement(destinationControl, child);
            if (result)
            {
                destinationControl.Controls = destinationControl.Controls ?? new List<ControlEntity>();
                destinationControl.Controls.Add(child);
                oldParent.Controls.RemoveAll(n => n.ID == child.ID);
            }

            mpeElementMove.Hide();
        }
        protected void btnElementMoveCancel_OnClick(object sender, EventArgs e)
        {
            mpeElementMove.Hide();
        }
        private bool CanMoveElement(ContentEntity parent, ControlEntity child)
        {
            if (child is TabPageEntity)
            {
                if (!(parent is TabContainerEntity))
                    return false;
            }

            if (parent is TabContainerEntity && !(child is TabPageEntity))
                return false;

            return true;
        }

        protected void btElementCancel_OnClick(object sender, EventArgs e)
        {
            mpeElement.Hide();
        }
        protected void elementsControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = FormEntity;
            if (entity == null || entity.Controls == null)
                return;

            var parentChildNodes = GetParentChildNode(e.Value, elementsControl.Model);
            if (parentChildNodes == null)
                return;

            var parentNode = parentChildNodes.Parent;
            var childNode = parentChildNodes.Child;

            var parentChildControls = GetParentChildControl(e.Value, childNode);

            var parentControl = parentChildControls.Parent;
            var childControl = parentChildControls.Child;

            var model = new ElementModel
            {
                ID = childNode.ID,
                Name = childNode.Name,
                Alias = childControl.Alias,
                Visible = childControl.Visible,
                OrderIndex = childControl.OrderIndex,
                ElementType = childNode.ElementType,
                NotPrintable = childControl.NotPrintable,
                DependentExp = childControl.DependentExp,
                FirstTimeFill = childControl.FirstTimeFill,
                DependentFieldID = childControl.DependentFieldID,
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
                model.GroupBgColor = groupEntity.BgColor;
                model.GroupTextColor = groupEntity.TextColor;
            }

            if (childNode.ElementType == "Grid")
            {
                var groupEntity = childControl as GridEntity;
                if (groupEntity == null)
                    return;

                model.ValidationExp = groupEntity.ValidationExp;
                model.ErrorMessage = groupEntity.ErrorMessage;
            }

            if (childNode.ElementType == "Tree")
            {
                var groupEntity = childControl as TreeEntity;
                if (groupEntity == null)
                    return;

                model.ValidationExp = groupEntity.ValidationExp;
                model.ErrorMessage = groupEntity.ErrorMessage;
            }

            if (childNode.ElementType == "Field")
            {
                var fieldEntity = childControl as FieldEntity;
                if (fieldEntity == null)
                    return;

                model.Tag = fieldEntity.Tag;
                model.Mask = fieldEntity.Mask;
                model.Unique = fieldEntity.Unique;
                model.ReadOnly = fieldEntity.ReadOnly.GetValueOrDefault();
                model.Privacy = fieldEntity.Privacy.GetValueOrDefault();
                model.Mandatory = fieldEntity.Mandatory.GetValueOrDefault();
                model.Inversion = fieldEntity.Inversion.GetValueOrDefault();
                model.Parameters = fieldEntity.Parameters;
                model.CaptionSize = fieldEntity.CaptionSize;
                model.ControlSize = fieldEntity.ControlSize;
                model.ControlType = fieldEntity.Type;
                model.Description = fieldEntity.Description;
                model.FilterByUser = fieldEntity.FilterByUser;
                model.ErrorMessage = fieldEntity.ErrorMessage;
                model.DataSourceID = fieldEntity.DataSourceID;
                model.ValidationExp = fieldEntity.ValidationExp;
                model.DisplayOnGrid = fieldEntity.DisplayOnGrid;
                model.DisplayOnFilter = fieldEntity.DisplayOnFilter;
                model.TextExpression = fieldEntity.TextExpression;
                model.ValueExpression = fieldEntity.ValueExpression;
                model.DependentFillExp = fieldEntity.DependentFillExp;
                model.RequiresApproval = fieldEntity.RequiresApproval;
                model.GridFieldSummary = fieldEntity.GridFieldSummary;
                model.DataSourceSortExp = fieldEntity.DataSourceSortExp;
                model.DataSourceFilterExp = fieldEntity.DataSourceFilterExp;
                model.FieldValueExpression = fieldEntity.FieldValueExpression;
            }

            elementControl.FillDependentFields(entity);
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

            var child = parentChildNodes.Child;

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
        protected void elementControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeElement.Show();
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
                entity.VisibleExpression = model.VisibleExpression;
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

        protected void FillCategoreisList()
        {
            var categories = (from n in HbSession.Query<GM_Category>()
                              where n.DateDeleted == null
                              orderby n.OrderIndex
                              select n).ToList();

            cbxCategory.BindData(categories);
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

            if (elementType == "Tree")
            {
                return new TreeEntity
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

        private IEnumerable<ListItem> GetPossibleMovingElements(Guid? formId)
        {
            var entity = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formId);
            if (entity != null)
            {
                var converter = new FormEntityModelConverter(HbSession);
                var model = converter.Convert(entity);
                return GetPossibleMovingElements(model.Entity);
            }
            return Enumerable.Empty<ListItem>();
        }
        private IEnumerable<ListItem> GetPossibleMovingElements(ControlEntity control)
        {
            return GetPossibleMovingElements(control, 0);
        }
        private IEnumerable<ListItem> GetPossibleMovingElements(ControlEntity control, int level)
        {
            if (!(control is FieldEntity))
            {
                var name = string.Empty;
                name += new string('-', level);
                name = $"{name} {control.Name}";
                yield return new ListItem(name, DataConverter.ToString(control.ID));
            }

            if (control is ContentEntity)
            {
                ++level;
                var content = control as ContentEntity;
                if (content.Controls != null)
                {
                    foreach (var item in content.Controls)
                    {
                        foreach (var listItem in GetPossibleMovingElements(item, level))
                        {
                            yield return listItem;
                        }
                    }
                }
            }
        }




        protected ParentChildEntity<ControlEntity> GetParentChildControl(Guid? itemID, ElementTreeNodeEntity node)
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
                return new ParentChildEntity<ControlEntity>(null, child);

            var parent = parentChild.GetValueOrDefault(node.ParentID);

            return new ParentChildEntity<ControlEntity>(parent, child);
        }
        protected ParentChildEntity<ElementTreeNodeEntity> GetParentChildNode(Guid? itemID, ElementNodesModel model)
        {
            var list = model.List;
            if (list == null)
                return null;

            var child = list.FirstOrDefault(n => n.ID == itemID);
            if (child == null)
                return null;

            var parent = list.FirstOrDefault(n => n.ID == child.ParentID);
            return new ParentChildEntity<ElementTreeNodeEntity>(parent, child);
        }




    }
}