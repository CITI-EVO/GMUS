using System;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.Svc.Enums;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private UmUtil instance;
        public UmUtil Instance
        {
            get
            {
                instance = (instance ?? UmUtil.Instance);
                return instance;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lstErrorMessages.Text = String.Empty;

            if (!Instance.Login())
            {
                Instance.GoToLogin();
            }
        }

        protected void btOK_Click(object sender, EventArgs e)
        {
            if (!Instance.IsLogged)
            {
                lstErrorMessages.Text = "";
                return;
            }

            var model = changePassword.Model;

            if (Instance.CurrentUser.Password != model.OldPassword)
            {
                lstErrorMessages.Text = "ძველი პაროლი არასწორია";
                return;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                lstErrorMessages.Text = "ახალი პაროლი და ახალი პაროლი გამეორებით არ ემთხვევა ერთმანეთს";
                return;
            }

            var result = Instance.ChangePassword(model.NewPassword, model.OldPassword);
            if (result != PasswordChangeResultEnum.Success)
            {
                lstErrorMessages.Text = "ახალი ან ძველი პაროლი არასწორია";
                return;
            }

            var redirectUrl = GetReturnUrl();
            if (String.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "~/Default.aspx";
            }

            var uriHelper = new UrlHelper(redirectUrl);
            uriHelper["loginToken"] = Convert.ToString(Instance.CurrentToken);

            redirectUrl = uriHelper.ToString();
            Response.Redirect(redirectUrl);
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            var redirectUrl = GetReturnUrl();
            if (String.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "~/Default.aspx";
            }

            var uriHelper = new UrlHelper(redirectUrl);
            uriHelper["loginToken"] = Convert.ToString(Instance.CurrentToken);

            redirectUrl = uriHelper.ToString();
            Response.Redirect(redirectUrl);
        }

        protected String GetReturnUrl()
        {
            if (String.IsNullOrEmpty(Request["ReturnUrl"]))
            {
                return null;
            }

            try
            {
                var bytes = HttpServerUtility.UrlTokenDecode(Request["ReturnUrl"]);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
            }

            try
            {
                var bytes = Convert.FromBase64String(Request["ReturnUrl"]);
                return Encoding.ASCII.GetString(bytes);
            }
            catch (Exception)
            {
            }

            return Request["ReturnUrl"];
        }
    }
}