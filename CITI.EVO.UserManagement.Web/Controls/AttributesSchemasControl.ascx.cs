using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;

using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Units;
using CITI.EVO.UserManagement.Web.Utils;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class AttributesSchemasControl : BaseUserControlExtend<AttributeSchemasModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnSetModel(object model)
        {
            var attributeSchemasModel = model as AttributeSchemasModel;
            if (attributeSchemasModel == null)
                return;

            var units = UmSchemasUtil.CreateListOfTree(HbSession, attributeSchemasModel);

            tlAttributes.DataSource = units;
            tlAttributes.DataBind();
        }

        protected bool GetEditVisible(object eval)
        {
            var type = DataConverter.ToString(eval);
            if (type == "Project")
                return false;

            return true;
        }

        protected bool GetDeleteVisible(object eval)
        {
            var type = DataConverter.ToString(eval);
            if (type == "Project")
                return false;

            return true;
        }

        protected bool GetNewVisible(object eval)
        {
            var type = DataConverter.ToString(eval);
            if (type == "Project" || type == "Schema")
                return true;

            return false;
        }
    }
}