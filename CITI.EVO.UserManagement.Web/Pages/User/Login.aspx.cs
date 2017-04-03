using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Utils;
using DevExpress.Utils.OAuth.Provider;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Login : BasePage
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

        protected void btnGoToLicPage_OnClick(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lstErrorMessages.Text = String.Empty;

            if (Instance.IsLogged)
                return;

            var redirectUrl = GetReturnUrl();
            if (String.IsNullOrEmpty(redirectUrl))
                redirectUrl = ConfigurationManager.AppSettings["DefaultPage"];

            if (Instance.Login())
            {
                var uriHelper = new UrlHelper(redirectUrl);
                uriHelper["loginToken"] = Convert.ToString(Instance.CurrentToken);

                redirectUrl = uriHelper.ToString();
                Response.Redirect(redirectUrl);
            }

            if (Instance.IsPasswordExpired)
                Instance.GoToChangePassword(redirectUrl);
        }

        protected void btOK_Click(object sender, EventArgs e)
        {

            pnlRecovery.Visible = false;

            var model = loginControl.Model;

            if (String.IsNullOrWhiteSpace(model.LoginName))
            {
                lstErrorMessages.Text = "გთხოვთ შეიყვანეთ მომხმარებლის სახელი";
                return;
            }

            var dbUser = (from n in HbSession.Query<UM_User>()
                          where n.DateDeleted == null &&
                                n.IsActive == true &&
                                (
                                    n.LoginName.ToLower() == model.LoginName.ToLower() ||
                                    n.Email.ToLower() == model.LoginName.ToLower()
                                )
                          select n).FirstOrDefault();

            if (dbUser == null)
            {
                lstErrorMessages.Text = "თქვენ არასწორად შეიყვანეთ მომხმარებლის სახელი ან პაროლი";
                //lstErrorMessages.Text = "Incorrect login name";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;
                return;
            }

            bool login = Instance.Login(dbUser.LoginName, model.Password);

            if (!login)
            {
                lstErrorMessages.Text = "თქვენ არასწორად შეიყვანეთ მომხმარებლის სახელი ან პაროლი";
                //lstErrorMessages.Text = "Incorrect login name";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;
                return;
            }

            if (model.RememberMe.GetValueOrDefault())
                Instance.SaveCurrentLoginCookies();
            else
                Instance.ClearCurrentLoginCookies();

            var redirectUrl = GetReturnUrl();
            if (String.IsNullOrEmpty(redirectUrl))
                redirectUrl = "~/Default.aspx";

            var uriHelper = new UrlHelper(redirectUrl);
            uriHelper["loginToken"] = Convert.ToString(Instance.CurrentToken);
            uriHelper[LanguageUtil.RequestLanguageKey] = LanguageUtil.GetLanguage();

            redirectUrl = uriHelper.ToString();
            Response.Redirect(redirectUrl);
        }

        protected void btnRecovery_Click(object sender, EventArgs e)
        {
            lstErrorMessages.ForeColor = Color.Red;

            var model = loginControl.Model;

            if (String.IsNullOrWhiteSpace(model.LoginName))
            {
                lstErrorMessages.Text = "გთხოვთ შეიყვანეთ მომხმარებლის სახელი";
                //lstErrorMessages.Text = "Incorrect login name";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;

                return;
            }

            var dbUser = (from n in HbSession.Query<UM_User>()
                          where n.DateDeleted == null &&
                                n.IsActive == true &&
                                (
                                    n.LoginName.ToLower() == model.LoginName.ToLower() ||
                                    n.Email.ToLower() == model.LoginName.ToLower()
                                )
                          select n).FirstOrDefault();

            if (dbUser == null || String.IsNullOrWhiteSpace(dbUser.Email))
            {
                lstErrorMessages.Text = "არასწორია ელ.ფოსტის მისამართი";
                //lstErrorMessages.Text = "Invalid email address";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;

                return;
            }

            dbUser.UserCode = Convert.ToString(Guid.NewGuid());
            HbSession.SubmitUpdate(dbUser);

            try
            {
                EmailUtil.SendRecoveryEmail(dbUser);
            }
            catch (Exception ex)
            {
                lstErrorMessages.Text = ex.Message;
                return;
            }

            lstErrorMessages.ForeColor = Color.Green;
            lstErrorMessages.Text = "თქვენ მიიღებთ პაროლის აღდგენის ბმულს ელ.ფოსტაზე";
            //lstErrorMessages.Text = "Please check your email to finish password recovery";
            pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;
        }

        protected String GetReturnUrl()
        {
            var returnUrl = Convert.ToString(RequestUrl["ReturnUrl"]);
            if (String.IsNullOrEmpty(returnUrl))
                return null;

            try
            {
                var bytes = HttpServerUtility.UrlTokenDecode(returnUrl);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception)
            {
            }

            try
            {
                var bytes = Convert.FromBase64String(returnUrl);
                return Encoding.ASCII.GetString(bytes);
            }
            catch (Exception)
            {
            }

            return returnUrl;
        }

        protected void btnGeo_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnEng_OnClick(object sender, EventArgs e)
        {

        }
    }
}