using System;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class ResourcesControl : BaseUserControlExtend<ResourcesModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool NewEnabled(Object value)
        {
            return true;
        }

        protected bool EditEnabled(Object value)
        {
            return true;
        }

        protected bool DeleteEnabled(Object value)
        {
            return true;
        }

        protected String GetTypeName(object eval)
        {
            var value = DataConverter.ToNullableInt(eval);
            if (value == null)
                return null;

            var name = Enum.GetName(typeof(RuleTypesEnum), value.Value);
            return name;
        }
    }
}