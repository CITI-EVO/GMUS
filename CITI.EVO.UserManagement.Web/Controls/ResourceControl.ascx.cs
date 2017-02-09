using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class ResourceControl : BaseUserControlExtend<ResourceModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillResourceType();
        }

        protected void FillResourceType()
        {
            var values = Enum.GetValues(typeof(RuleTypesEnum)).Cast<int>();

            var ruleTypes = (from value in values
                             let name = Enum.GetName(typeof(RuleTypesEnum), value)
                             select new KeyValuePair<String, int>(name, value)).ToList();

            cmbResourceType.DataSource = ruleTypes;
            cmbResourceType.DataBind();
        }
    }
}