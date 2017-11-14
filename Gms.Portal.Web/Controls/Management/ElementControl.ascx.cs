using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementControl : BaseUserControlExtend<ElementModel>
    {
        public List<ParameterEntity> Parameters
        {
            get { return ViewState["Parameters"] as List<ParameterEntity>; }
            set { ViewState["Parameters"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        protected void comboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnSaveParam_OnClick(object sender, EventArgs e)
        {
            var name = cbxParameters.TryGetStringValue();
            var expression = tbxParameterExp.Text;

            var entity = new ParameterEntity
            {
                ID = name.ComputeMd5Guid(),
                Name = name,
                Expression = expression
            };

            Parameters = (Parameters ?? new List<ParameterEntity>());

            if (Parameters.Any(n => n.ID == entity.ID))
                return;

            Parameters.Add(entity);

            BindParametersList();
        }

        protected void btnDeleteParam_OnCommand(object sender, CommandEventArgs e)
        {
            if (Parameters == null)
                return;

            var paramID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (paramID == null)
                return;

            Parameters.RemoveAll(n => n.ID == paramID);

            BindParametersList();
        }

        public override ElementModel GetModel()
        {
            var model = base.GetModel();
            model.Parameters = Parameters;

            var properties = (from n in lstProperties.Items.OfType<ListItem>()
                              where n.Selected
                              select n.Value).ToHashSet();

            model.Unique = properties.Contains("Unique");
            model.Visible = properties.Contains("Visible");
            model.Privacy = properties.Contains("Privacy");
            model.ReadOnly = properties.Contains("ReadOnly");
            model.Inversion = properties.Contains("Inversion");
            model.Mandatory = properties.Contains("Mandatory");
            model.FilterByUser = properties.Contains("FilterByUser");
            model.NotPrintable = properties.Contains("NotPrintable");
            model.FirstTimeFill = properties.Contains("FirstTimeFill");
            model.AllowBulkFill = properties.Contains("AllowBulkFill");
            model.ResetDataOnHide = properties.Contains("ResetDataOnHide");
            model.DisplayOnFilter = properties.Contains("DisplayOnFilter");
            model.RequiresApproval = properties.Contains("RequiresApproval");

            return model;
        }

        public override void SetModel(ElementModel model)
        {
            Parameters = model.Parameters;

            ApplyViewMode();

            base.SetModel(model);
        }

        protected void ApplyViewMode()
        {
            var model = Model;

            var properties = new HashSet<String>();

            if (model.Unique.GetValueOrDefault())
                properties.Add("Unique");

            if (model.Visible.GetValueOrDefault())
                properties.Add("Visible");

            if (model.Privacy.GetValueOrDefault())
                properties.Add("Privacy");

            if (model.ReadOnly.GetValueOrDefault())
                properties.Add("ReadOnly");

            if (model.Inversion.GetValueOrDefault())
                properties.Add("Inversion");

            if (model.Mandatory.GetValueOrDefault())
                properties.Add("Mandatory");

            if (model.FilterByUser.GetValueOrDefault())
                properties.Add("FilterByUser");

            if (model.NotPrintable.GetValueOrDefault())
                properties.Add("NotPrintable");

            if (model.FirstTimeFill.GetValueOrDefault())
                properties.Add("FirstTimeFill");

            if (model.AllowBulkFill.GetValueOrDefault())
                properties.Add("AllowBulkFill");

            if (model.DisplayOnFilter.GetValueOrDefault())
                properties.Add("DisplayOnFilter");

            if (model.ResetDataOnHide.GetValueOrDefault())
                properties.Add("ResetDataOnHide");

            if (model.RequiresApproval.GetValueOrDefault())
                properties.Add("RequiresApproval");

            foreach (var listItem in lstProperties.Items.OfType<ListItem>())
                listItem.Selected = properties.Contains(listItem.Value);

            var elementTypes = GetSubElementTypes(model.ParentType);
            cbxElementType.BindData(elementTypes);

            var dataSources = LoadDataSources().OrderBy(n => n.Key);
            cbxDataSource.BindData(dataSources);

            var currDataSource = cbxDataSource.TryGetStringValue();
            var match = RegexUtil.DataSourceParserRx.Match(currDataSource);

            var serviceDs = false;

            var parentID = DataConverter.ToNullableGuid(match.Groups["parentID"].Value);
            var childID = DataConverter.ToNullableGuid(match.Groups["childID"].Value);

            if (parentID != null && childID != null)
            {
                var dbService = HbSession.Query<GM_Service>().FirstOrDefault(n => n.ID == parentID);
                if (dbService != null)
                {
                    serviceDs = true;

                    var converter = new ServiceEntityModelConverter(HbSession);
                    var svcModel = converter.Convert(dbService);

                    if (svcModel.Entity != null)
                    {
                        var method = svcModel.Entity.Methods.FirstOrDefault(n => n.ID == childID);
                        if (method != null)
                            cbxParameters.BindData(method.Parameters);
                    }
                }
            }

            pnlTag.Visible = false;
            pnlType.Visible = false;
            pnlMask.Visible = false;
            pnlGroupSize.Visible = false;
            pnlDataSource.Visible = false;
            pnlTotalSize.Visible = false;
            pnlCaptionSize.Visible = false;
            pnlControlSize.Visible = false;
            pnlErrorMessage.Visible = false;
            pnlGroupBgColor.Visible = false;
            pnlTreeMaxLevel.Visible = false;
            pnlValidationExp.Visible = false;
            pnlTextExpression.Visible = false;
            pnlGroupTextColor.Visible = false;
            pnlValueExpression.Visible = false;
            pnlDependentFillExp.Visible = false;
            pnlGridFieldSummary.Visible = false;
            pnlDataSourceSortExp.Visible = false;
            pnlServiceParameters.Visible = false;
            pnlDataSourceFilterExp.Visible = false;
            pnlFieldValueExpression.Visible = false;

            if (model.ElementType == "Group")
            {
                pnlGroupSize.Visible = true;
                pnlGroupBgColor.Visible = true;
                pnlGroupTextColor.Visible = true;
            }

            if (model.ElementType == "Grid")
            {
                pnlValidationExp.Visible = true;
                pnlErrorMessage.Visible = true;
                pnlDependentFillExp.Visible = true;
            }

            if (model.ElementType == "Tree")
            {
                pnlValidationExp.Visible = true;
                pnlErrorMessage.Visible = true;
                pnlTreeMaxLevel.Visible = true;
                pnlDependentFillExp.Visible = true;
            }

            if (model.ElementType == "Field")
            {
                pnlTag.Visible = true;
                pnlType.Visible = true;
                pnlMask.Visible = true;
                pnlOrderIndex.Visible = true;
                pnlTotalSize.Visible = true;
                pnlCaptionSize.Visible = true;
                pnlControlSize.Visible = true;
                pnlErrorMessage.Visible = true;
                pnlValidationExp.Visible = true;
                pnlDependentFillExp.Visible = true;
                pnlDataSourceSortExp.Visible = true;
                pnlFieldValueExpression.Visible = true;

                if (model.ControlType == "Number")
                {
                    pnlGridFieldSummary.Visible = model.ParentType == "Grid";
                }

                if (model.ControlType == "ComboBox")
                {
                    pnlDataSource.Visible = true;
                    pnlTextExpression.Visible = true;
                    pnlValueExpression.Visible = true;
                    pnlDataSourceFilterExp.Visible = true;
                }

                if (model.ControlType == "CheckBoxList")
                {
                    pnlDataSource.Visible = true;
                    pnlTextExpression.Visible = true;
                    pnlValueExpression.Visible = true;
                    pnlDataSourceFilterExp.Visible = true;
                }

                if (model.ControlType == "Lookup")
                {
                    pnlDataSource.Visible = true;
                    pnlTextExpression.Visible = true;
                    pnlValueExpression.Visible = true;
                    pnlServiceParameters.Visible = serviceDs;
                    pnlDataSourceFilterExp.Visible = true;
                }
            }
        }

        protected void BindParametersList()
        {
            if (Parameters != null)
            {
                gvParameters.DataSource = Parameters;
                gvParameters.DataBind();
            }
        }

        public void FillDependentFields(ContentEntity entity)
        {
            var treeFields = FormStructureUtil.CreateTree(entity).ToList();
            var elementsLp = treeFields.ToLookup(n => n.ParentID);

            var parents = elementsLp[null];
            CorrectNamesByLevel(parents, elementsLp, 0);

            cbxDependentField.BindData(treeFields);
        }

        protected IEnumerable<KeyValueEntity> LoadDataSources()
        {
            var collectionsDataSource = (from n in HbSession.Query<GM_Collection>()
                                         where n.DateDeleted == null
                                         select n);

            foreach (var item in collectionsDataSource)
            {
                var entity = new KeyValueEntity
                {
                    Key = item.Name,
                    Value = $"{item.ID}/@"
                };

                yield return entity;
            }

            var formsDataSource = (from n in HbSession.Query<GM_Form>()
                                   where n.DateDeleted == null
                                   select n);

            var formConverter = new FormEntityModelConverter(HbSession);

            foreach (var item in formsDataSource)
            {
                var entity = new KeyValueEntity
                {
                    Key = item.Name,
                    Value = $"{item.ID}/@"
                };

                yield return entity;

                var model = formConverter.Convert(item);
                if (model.Entity != null)
                {
                    var allControls = FormStructureUtil.PreOrderFirstLevelTraversal(model.Entity);

                    var treesAndGrids = (from n in allControls
                                         where n is GridEntity || n is TreeEntity
                                         select n);

                    foreach (var control in treesAndGrids)
                    {
                        var subEntity = new KeyValueEntity
                        {
                            Key = $"{item.Name}/{control.Name} - {control.Alias}",
                            Value = $"{item.ID}/{control.ID}"
                        };

                        yield return subEntity;
                    }
                }
            }

            var servicesDataSource = (from n in HbSession.Query<GM_Service>()
                                      where n.DateDeleted == null
                                      select n);

            var serviceConverter = new ServiceEntityModelConverter(HbSession);

            foreach (var item in servicesDataSource)
            {
                var model = serviceConverter.Convert(item);
                if (model.Entity != null)
                {
                    foreach (var method in model.Entity.Methods)
                    {
                        var subEntity = new KeyValueEntity
                        {
                            Key = $"{item.Name}/{method.Name}",
                            Value = $"{item.ID}/{method.ID}"
                        };

                        yield return subEntity;
                    }
                }
            }
        }

        protected IEnumerable<String> GetSubElementTypes(String parentType)
        {
            switch (parentType)
            {
                case "Field":
                    return null;
                case "Grid":
                case "Tree":
                    return new[] { "Field" };
                case "TabContainer":
                    return new[] { "TabPage" };
                case "Form":
                case "Group":
                case "TabPage":
                    return new[] { "Field", "Grid", "Tree", "Group", "TabContainer" };
                default:
                    return new[] { "Form" };
            }
        }

        protected void CorrectNamesByLevel(IEnumerable<ElementTreeNodeEntity> parents, ILookup<Guid?, ElementTreeNodeEntity> elementsLp, int level)
        {
            foreach (var entity in parents)
            {
                entity.Name = $"{entity.Name} - {entity.Alias} ({entity.ControlType})";

                var children = elementsLp[entity.ID];
                CorrectNamesByLevel(children, elementsLp, level + 1);
            }
        }
    }
}