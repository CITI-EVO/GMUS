using System;
using CITI.EVO.Tools.Security;
using Gms.Portal.Web.Bases;

namespace Gms.Portal.Web.Controls.Common
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