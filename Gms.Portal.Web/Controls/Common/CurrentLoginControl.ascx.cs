using System;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.Svc.Enums;
using Gms.Portal.Web.Bases;

namespace Gms.Portal.Web.Controls.Common
{
    public partial class CurrentLoginControl : BaseUserControl
    {

        public String UserName
        {
            set { lblUser.Text = String.Format("User: {0}", value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void btLogout_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.Logout();
            Response.Redirect("~/default.aspx");
        }

        protected void btChangePassword_Click(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToChangePassword();
        }
    }
}