using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementControl : BaseUserControlExtend<ElementModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        protected void comboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }

        public override void SetModel(ElementModel model)
        {
            ApplyViewMode();
            base.SetModel(model);
        }

        public void FillDependentFields(ContentEntity entity)
        {
            var treeFields = FormStructureUtil.CreateTree(entity).ToList();
            var elementsLp = treeFields.ToLookup(n => n.ParentID);

            var parents = elementsLp[null];
            CorrectNamesByLevel(parents, elementsLp, 0);

            cbxDependentField.BindData(treeFields);
        }

        protected void ApplyViewMode()
        {
            var model = Model;

            var elementTypes = GetSubElementTypes(model.ParentType);

            cbxElementType.BindData(elementTypes);

            var dataSources = (from n in HbSession.Query<GM_Collection>()
                               where n.DateDeleted == null
                               orderby n.Name
                               select n).ToList();

            cbxDataSource.BindData(dataSources);

            pnlTag.Visible = false;
            pnlType.Visible = false;
            pnlMask.Visible = false;
            pnlEnabled.Visible = false;
            pnlPrivacy.Visible = false;
            pnlMandatory.Visible = false;
            pnlInversion.Visible = false;
            pnlDataSource.Visible = false;
            pnlGroupAlign.Visible = false;
            pnlCaptionSize.Visible = false;
            pnlControlSize.Visible = false;
            pnlErrorMessage.Visible = false;
            pnlValidationExp.Visible = false;
            pnlDisplayOnGrid.Visible = false;
            pnlTextExpression.Visible = false;
            pnlValueExpression.Visible = false;
            pnlDependentFillExp.Visible = false;
            pnlDataSourceFilterExp.Visible = false;

            if (model.ElementType == "Group")
            {
                pnlGroupAlign.Visible = true;
            }

            if (model.ElementType == "Grid")
            {
                pnlValidationExp.Visible = true;
                pnlErrorMessage.Visible = true;
            }

            if (model.ElementType == "Field")
            {
                pnlTag.Visible = true;
                pnlType.Visible = true;
                pnlMask.Visible = true;
                pnlEnabled.Visible = true;
                pnlPrivacy.Visible = true;
                pnlInversion.Visible = true;
                pnlMandatory.Visible = true;
                pnlOrderIndex.Visible = true;
                pnlCaptionSize.Visible = true;
                pnlControlSize.Visible = true;
                pnlErrorMessage.Visible = true;
                pnlValidationExp.Visible = true;
                pnlDisplayOnGrid.Visible = true;
                pnlDependentFillExp.Visible = true;

                if (model.ControlType == "ComboBox")
                {
                    pnlDataSource.Visible = true;
                    pnlTextExpression.Visible = true;
                    pnlValueExpression.Visible = true;
                    pnlDataSourceFilterExp.Visible = true;
                }
            }
        }

        protected IEnumerable<String> GetSubElementTypes(String parentType)
        {
            if (parentType == "Field")
                return null;

            if (parentType == "Form")
                return new[] { "Field", "Grid", "Group", "TabContainer" };

            if (parentType == "Grid")
                return new[] { "Field" };

            if (parentType == "Group")
                return new[] { "Field", "Grid", "Group", "TabContainer" };

            if (parentType == "TabContainer")
                return new[] { "TabPage" };

            if (parentType == "TabPage")
                return new[] { "Field", "Grid", "Group", "TabContainer" };

            return new[] { "Form" };
        }

        protected void CorrectNamesByLevel(IEnumerable<ElementTreeNodeEntity> entites, ILookup<Guid?, ElementTreeNodeEntity> elementsLp, int level)
        {
            foreach (var entity in entites)
            {
                entity.Name = String.Format("{0}({1})", entity.Name, entity.ControlType);

                var children = elementsLp[entity.ID];
                CorrectNamesByLevel(children, elementsLp, level + 1);
            }
        }
    }
}