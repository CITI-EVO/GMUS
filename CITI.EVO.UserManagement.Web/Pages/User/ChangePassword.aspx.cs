using System;
using System.Linq;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Svc.Enums;
using CITI.EVO.UserManagement.Web.Bases;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class ChangePassword : BasePage
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

        protected void Page_Init(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper(Request.Url);

            urlHelper[LanguageUtil.RequestLanguageKey] = "en-US";
            btEngLang.NavigateUrl = urlHelper.ToEncodedUrl();

            urlHelper[LanguageUtil.RequestLanguageKey] = "ka-GE";
            btGeoLang.NavigateUrl = urlHelper.ToEncodedUrl();
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

            if (String.IsNullOrWhiteSpace(model.NewPassword))
            {
                lstErrorMessages.Text = "გთხოვთ შეიყვანეთ პაროლი";
                return;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                lstErrorMessages.Text = "პაროლები არ ემთხვევა ერთმანეთს";
                return;
            }


            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.ID == Instance.CurrentUser.ID
                        select n).FirstOrDefault();

            if (user == null)
            {
                lstErrorMessages.Text = "მომხმარებელი ვერ მოიძებნა";
                return;
            }

            user.Password = model.NewPassword;

            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                var emailUser = (from n in HbSession.Query<UM_User>()
                                 where n.DateDeleted == null &&
                                       n.Email == model.Email
                                 select n).FirstOrDefault();

                if (emailUser != null && emailUser.ID != Instance.CurrentUser.ID)
                {
                    lstErrorMessages.Text = "მითითებული ელ-ფოსტა უკვე რეგისტრირებულია";
                    return;
                }

                user.Email = model.Email;
            }


            HbSession.SubmitUpdate(user);

            Instance.Logout();
            Instance.Login(user.LoginName, model.NewPassword);

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