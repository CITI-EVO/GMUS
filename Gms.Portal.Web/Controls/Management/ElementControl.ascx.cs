using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;
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

        protected void ApplyViewMode()
        {
            var model = Model;

            var elementTypes = GetElementTypes(model.ParentType);

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
            pnlDataSource.Visible = false;
            pnlGroupAlign.Visible = false;
            pnlValidationExp.Visible = false;
            pnlDisplayOnGrid.Visible = false;
            pnlTextExpression.Visible = false;
            pnlValueExpression.Visible = false;

            if (model.ElementType == "Group")
            {
                pnlGroupAlign.Visible = true;
            }

            if (model.ElementType == "Field")
            {
                pnlTag.Visible = true;
                pnlType.Visible = true;
                pnlMask.Visible = true;
                pnlEnabled.Visible = true;
                pnlPrivacy.Visible = true;
                pnlOrderIndex.Visible = true;
                pnlValidationExp.Visible = true;
                pnlDisplayOnGrid.Visible = true;

                if (model.ControlType == "ComboBox")
                {
                    pnlDataSource.Visible = true;
                    pnlTextExpression.Visible = true;
                    pnlValueExpression.Visible = true;
                }
            }
        }



        protected IEnumerable<String> GetElementTypes(String parentType)
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
    }
}