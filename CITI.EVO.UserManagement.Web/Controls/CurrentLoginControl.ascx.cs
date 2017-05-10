using System;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Bases;

namespace CITI.EVO.UserManagement.Web.Controls
{
    public partial class CurrentLoginControl : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UmUtil.Instance.IsLogged)
            {
                var user = UmUtil.Instance.CurrentUser;
                lblUser.Text = $"{user.LoginName} - {user.FirstName} - {user.LastName}";
            }
        }
    }
}