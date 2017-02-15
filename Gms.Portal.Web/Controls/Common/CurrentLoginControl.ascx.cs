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
            set
            {
                lblUser.Text = String.Format("user: {0}", value);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (UmUtil.Instance.IsPasswordExpired)
            {
                mpeChangePassword.Show();
            }

        }


        protected void btLogout_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.Logout();
            Response.Redirect("~/default.aspx");
        }

        protected void btChangePasswordOK_Click(object sender, EventArgs e)
        {
            if (tbNewPassword.Text != tbConfirmNewPassword.Text)
            {
                lblChangePasswordError.Text = "invalid confirm password";

                upnlChangePassword.Update();
                mpeChangePassword.Show();

                return;
            }

            var result = UmUtil.Instance.ChangePassword(tbNewPassword.Text.Trim(), tbCurrentPassword.Text.Trim());
            if (result == PasswordChangeResultEnum.Success)
            {
                var currentLoginName = UmUtil.Instance.CurrentUser.LoginName;
                var newPassword = tbNewPassword.Text.Trim();
                UmUtil.Instance.Logout();
                UmUtil.Instance.Login(currentLoginName, newPassword);

                upnlChangePassword.Update();
                mpeChangePassword.Hide();

                return;
            }

            String errorMessage = null;
            switch (result)
            {
                case PasswordChangeResultEnum.TokenNotFound:
                case PasswordChangeResultEnum.UserNotFound:
                    errorMessage = "User not found";
                    break;
                case PasswordChangeResultEnum.InvalidPattern:
                    errorMessage = "Invalid pattern";
                    break;
                case PasswordChangeResultEnum.PasswordMismatch:
                    errorMessage = "Password mismatch";
                    break;
            }

            lblChangePasswordError.Text = errorMessage;
            upnlChangePassword.Update();
            mpeChangePassword.Show();
        }

        protected void btChangePasswordCancel_Click(object sender, EventArgs e)
        {
            mpeChangePassword.Hide();
        }

        protected void btChangePassword_Click(object sender, EventArgs e)
        {
            lblChangePasswordError.Text = string.Empty;

            mpeChangePassword.Show();
            upnlChangePassword.Update();
        }

    }
}