using System;
using System.Collections.Generic;
using CITI.EVO.Proxies;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class FormElementControl : BaseUserControlExtend<FormElementModel>
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var languages = CommonProxy.GetLanguages();

            cbxLanguage.DataSource = languages;
            cbxLanguage.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyViewMode();
        }

        protected void cbxElementType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }

        protected override void OnSetModel(object model)
        {
            ApplyViewMode();
        }

        protected void ApplyViewMode()
        {
            var model = Model;

            trNumber.Visible = false;
            trLanguage.Visible = false;
            trOrderIndex.Visible = false;
            trType.Visible = false;
            trMask.Visible = false;
            trEnabled.Visible = false;
            trValidationExp.Visible = false;
            trTag.Visible = false;

            var elementTypes = GetElementTypes(model.ParentType);

            cbxElementType.DataSource = elementTypes;
            cbxElementType.DataBind();

            if (model.ElementType == "Form")
            {
                trNumber.Visible = true;
                trLanguage.Visible = true;
            }

            if (model.ElementType == "Field")
            {
                trOrderIndex.Visible = true;
                trType.Visible = true;
                trMask.Visible = true;
                trEnabled.Visible = true;
                trValidationExp.Visible = true;
                trTag.Visible = true;
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