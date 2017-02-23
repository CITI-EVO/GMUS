using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class RegisterUserControl : BaseUserControlExtend<RegisterUserModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var groups = (from n in HbSession.Query<UM_Group>()
                          where Enumerable.Contains(ConfigUtil.UserRegisterGroupID, n.ID)
                          select n).ToList();

            BindData(cbxGroups, groups);
        }

        protected void BindData(ListControl control, IEnumerable source)
        {
            var selValue = control.TryGetStringValue();

            control.Items.Clear();

            control.DataSource = source;
            control.DataBind();

            control.Items.Insert(0, new ListItem("Select an Option", String.Empty));

            control.TrySetSelectedValue(selValue);
        }
    }
}