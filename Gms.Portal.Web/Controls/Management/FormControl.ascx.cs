using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using DevExpress.Web.ASPxTreeList;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using ASPxTreeList = CITI.EVO.Tools.Web.UI.Controls.ASPxTreeList;
using LinkButton = CITI.EVO.Tools.Web.UI.Controls.LinkButton;

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
            FillRatesTree();
            FillElementsTree();
            FillCategoreisList();
            FillTemplatesGrid();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillRatesTree();
            FillElementsTree();
            FillCategoreisList();
            FillTemplatesGrid();
        }

        protected void btnNewElement_OnClick(object sender, EventArgs e)
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
        protected void btnElementOK_OnClick(object sender, EventArgs e)
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
                if (childControl == null)
                    return;

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
                entity.AllowBulkFill = model.AllowBulkFill;
            }

            if (model.ElementType == "Tree")
            {
                var entity = (TreeEntity)childControl;
                entity.MaxLevel = model.TreeMaxLevel;
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
                entity.ResetDataOnHide = model.ResetDataOnHide;
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
            childControl.VisibleExpression = model.VisibleExpression;

            mpeElement.Hide();
        }
        protected void btnElementCancel_OnClick(object sender, EventArgs e)
        {
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
        protected void elementsControl_OnCopy(object sender, GenericEventArgs<Guid> e)
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
        protected void elementsControl_OnPaste(object sender, GenericEventArgs<Guid> e)
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
                VisibleExpression = childControl.VisibleExpression,
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
                var gridEntity = childControl as GridEntity;
                if (gridEntity == null)
                    return;

                model.ValidationExp = gridEntity.ValidationExp;
                model.ErrorMessage = gridEntity.ErrorMessage;
                model.AllowBulkFill = gridEntity.AllowBulkFill;
            }

            if (childNode.ElementType == "Tree")
            {
                var treeEntity = childControl as TreeEntity;
                if (treeEntity == null)
                    return;

                model.TreeMaxLevel = treeEntity.MaxLevel;
                model.ValidationExp = treeEntity.ValidationExp;
                model.ErrorMessage = treeEntity.ErrorMessage;
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
                model.ResetDataOnHide = fieldEntity.ResetDataOnHide;
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

        protected void btnElementPasteOK_OnClick(object sender, EventArgs e)
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

            var childControl = sourceDictionary.GetValueOrDefault(elementId);
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
        protected void btnElementPasteCancel_OnClick(object sender, EventArgs e)
        {
            mpeElementPaste.Hide();
        }
        protected void elementPasteControl_OnDataChanged(object sender, EventArgs e)
        {
            var model = elementPasteControl.Model;
            model.FormTree = GetPossibleMovingElements(model.FormId).ToList();
            elementPasteControl.Model = model;

            mpeElementPaste.Show();
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

        protected void btnNewValidation_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnNewRate_OnClick(object sender, EventArgs e)
        {
            var model = new RateModel();
            rateControl.Model = model;

            mpeRate.Show();
        }
        protected void btnAddRate_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                return;

            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            var rating = formEntity.Rating;
            if (rating == null || rating.Rates == null)
                return;

            var rate = rating.Rates.FirstOrDefault(n => n.ID == itemID);
            if (rate == null)
                return;

            var model = new RateModel
            {
                ParentID = rate.ID,
            };

            rateControl.Model = model;
            mpeRate.Show();
        }
        protected void btnEditRate_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                return;

            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            var rating = formEntity.Rating;
            if (rating == null || rating.Rates == null)
                return;

            var rate = rating.Rates.FirstOrDefault(n => n.ID == itemID);
            if (rate == null)
                return;

            var model = new RateModel
            {
                ID = rate.ID,
                ParentID = rate.ParentID,
                Name = rate.Name,
                Number = rate.Number,
                MaxScore = rate.MaxScore,
                MinScore = rate.MinScore,
            };

            rateControl.Model = model;
            mpeRate.Show();
        }
        protected void btnDeleteRate_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                return;

            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            var rating = formEntity.Rating;
            if (rating == null || rating.Rates == null)
                return;

            rating.Rates.RemoveAll(n => n.ID == itemID);
        }

        protected void btnRateOK_OnClick(object sender, EventArgs e)
        {
            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            formEntity.Rating = (formEntity.Rating ?? new RatingEntity());

            var rating = formEntity.Rating;
            if (rating == null)
            {
                rating = new RatingEntity();
                formEntity.Rating = rating;
            }

            var rates = rating.Rates;
            if (rates == null)
            {
                rates = new List<RateEntity>();
                rating.Rates = rates;
            }

            var model = rateControl.Model;

            var rate = rates.FirstOrDefault(n => n.ID == model.ID);
            if (rate == null)
            {
                rate = new RateEntity
                {
                    ID = Guid.NewGuid()
                };

                rates.Add(rate);
            }

            rate.Name = model.Name;
            rate.Number = model.Number;
            rate.MaxScore = model.MaxScore;
            rate.MinScore = model.MinScore;
            rate.ParentID = model.ParentID;

            mpeRate.Hide();
        }
        protected void btnRateCancel_OnClick(object sender, EventArgs e)
        {
            mpeRate.Hide();
        }
        protected void rateControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeRate.Show();
        }

        protected void btnNewTemplate_OnClick(object sender, EventArgs e)
        {
            var model = new PrintTemplateModel();
            printTemplateControl.Model = model;

            mpePrintTemplate.Show();
        }
        protected void btnEditTemplate_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                return;

            var formEntity = FormEntity;
            if (formEntity == null || formEntity.Templates == null)
                return;

            var template = formEntity.Templates.FirstOrDefault(n => n.ID == itemID);
            if (template == null)
                return;

            var model = new PrintTemplateModel
            {
                ID = template.ID,
                Name = template.Name,
                Role = template.Role,
                Type = template.Type,
                Layout = template.Layout,
                Template = template.Template
            };

            printTemplateControl.Model = model;
            mpePrintTemplate.Show();
        }
        protected void btnDeleteTemplate_OnCommand(object sender, CommandEventArgs e)
        {
            var itemID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (itemID == null)
                return;

            var formEntity = FormEntity;
            if (formEntity == null || formEntity.Templates == null)
                return;

            formEntity.Templates.RemoveAll(n => n.ID == itemID);
        }

        protected void btnPrintTemplateOK_OnClick(object sender, EventArgs e)
        {
            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            formEntity.Templates = (formEntity.Templates ?? new List<TemplateEntity>());

            var templates = formEntity.Templates;
            if (templates == null)
            {
                templates = new List<TemplateEntity>();
                formEntity.Templates = templates;
            }

            var model = printTemplateControl.Model;

            var template = templates.FirstOrDefault(n => n.ID == model.ID);
            if (template == null)
            {
                template = new TemplateEntity
                {
                    ID = Guid.NewGuid()
                };

                templates.Add(template);
            }

            template.Name = model.Name;
            template.Role = model.Role;
            template.Type = model.Type;
            template.Layout = model.Layout;
            template.Template = model.Template;

            mpePrintTemplate.Hide();
        }
        protected void btnPrintTemplateCancel_OnClick(object sender, EventArgs e)
        {
            mpePrintTemplate.Hide();
        }
        protected void printTemplateControl_OnDataChanged(object sender, EventArgs e)
        {
            mpePrintTemplate.Show();
        }

        protected void elementControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeElement.Show();
        }

        protected void tlRates_OnHtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
        {
            var treeList = (ASPxTreeList)sender;
            if (treeList == null)
                return;

            if (e.Level <= 1)
                return;

            var buttons = UserInterfaceUtil.TraverseControls(e.Row).OfType<LinkButton>();

            var btnNew = buttons.FirstOrDefault(n => n.ID == "btnAddRate");
            if (btnNew != null)
                btnNew.Visible = false;
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

                entity.Rating = entity.Rating ?? new RatingEntity();
                entity.Rating.MailTemplate = tbxMailTemplate.Text;
                entity.Rating.PrintTemplate = tbxPrintTemplate.Text;
                entity.Rating.SelectorExpression = tbxSelectorExpression.Text;
                entity.Rating.SummaryExpression = tbxSummaryExpression.Text;
            }

            return model;
        }

        public override void SetModel(FormModel model)
        {
            FormEntity = model.Entity;

            if (FormEntity != null && FormEntity.Rating != null)
            {
                tbxMailTemplate.Text = FormEntity.Rating.MailTemplate;
                tbxPrintTemplate.Text = FormEntity.Rating.PrintTemplate;
                tbxSummaryExpression.Text = FormEntity.Rating.SummaryExpression;
                tbxSelectorExpression.Text = FormEntity.Rating.SelectorExpression;
            }

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

        protected void FillTemplatesGrid()
        {
            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            gvTemplates.DataSource = formEntity.Templates;
            gvTemplates.DataBind();
        }

        protected void FillRatesTree()
        {
            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            var rating = FormEntity.Rating;
            if (rating == null)
                return;

            tlRates.DataSource = rating.Rates;
            tlRates.DataBind();
        }

        protected void FillValidationsTree()
        {
            var formEntity = FormEntity;
            if (formEntity == null)
                return;

            var validations = FormEntity.Validations;
            if (validations == null)
                return;

            tlRates.DataSource = CreateValidationsTree(validations);
            tlRates.DataBind();
        }

        private IEnumerable<TreeNodeEntity> CreateValidationsTree(IEnumerable<ValidationEntity> validations)
        {
            yield break;
            //var validationsDict = validations.ToDictionary(n => n.ID);
            //var validationsLp = validations.ToLookup(n => n.ParentID);

            //foreach (var entity in validationsLp[null])
            //{
            //    var mainNode = new TreeNodeEntity
            //    {
            //        ID = entity.ID,
            //        Name = entity.Name,
            //        ParentID = entity.ParentID,
            //    };

            //    var children = childrenLp[entity.ID];
            //    var byTypeLp = children.ToLookup(n => n.ExecType);

            //    foreach (var VARIABLE in byTypeLp)
            //    {

            //    }

            //    yield return mainNode;
            //}
        }

        private IEnumerable<TreeNodeEntity> CreateValidationsTree(Guid? parentID, ValidationEntity entity, ILookup<Guid?, ValidationEntity> validationsLp)
        {
            yield break;
            //var mainNode = new TreeNodeEntity
            //{
            //    ID = entity.ID,
            //    Name = entity.Name,
            //    ParentID = entity.ParentID,
            //};

            //var children = validationsLp[entity.ID];
            //var byTypeLp = children.ToLookup(n => n.ExecType);

            //foreach (var byTypeGrp in byTypeLp)
            //{
            //    var childNode = new TreeNodeEntity
            //    {
            //        ID = null,
            //        ParentID = entity.ID,
            //        Name = byTypeGrp.Key
            //    };

            //    yield return childNode;

            //    foreach (var VARIABLE in CreateValidationsTree(pa))
            //    {

            //    }
            //}
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

        protected bool CanMoveElement(ContentEntity parent, ControlEntity child)
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

        protected IEnumerable<ListItem> GetPossibleMovingElements(Guid? formId)
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
        protected IEnumerable<ListItem> GetPossibleMovingElements(ControlEntity control)
        {
            return GetPossibleMovingElements(control, 0);
        }
        protected IEnumerable<ListItem> GetPossibleMovingElements(ControlEntity control, int level)
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