using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Utils;
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

            bool login = Instance.Login(model.LoginName, model.Password);

            if (!login)
            {
                lstErrorMessages.Text = "თქვენ არასწორად შეიყვანეთ მომხმარებლის სახელი ან პაროლი";
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

            redirectUrl = uriHelper.ToString();
            Response.Redirect(redirectUrl);
        }

        protected void btnRecovery_Click(object sender, EventArgs e)
        {
            var model = loginControl.Model;

            if (String.IsNullOrWhiteSpace(model.LoginName))
            {
                lstErrorMessages.Text = "თქვენ არასწორად შეიყვანეთ მომხმარებლის სახელი";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;

                return;
            }

            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.LoginName == model.LoginName
                        select n).FirstOrDefault();

            if (user == null || String.IsNullOrWhiteSpace(user.Email))
            {
                lstErrorMessages.Text = "თქვენ არასწორად შეიყვანეთ მომხმარებლის სახელი ან თქვენს მომხმარებელზე არ არის მითითებული ელ.ფოსტის მისამართი. გთხოვთ მიმართეთ საიტის ადმინსიტრაციას";
                pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;

                return;
            }

            user.UserCode = Convert.ToString(Guid.NewGuid());
            HbSession.SubmitUpdate(user);

            EmailUtil.SendRecoveryEmail(user);

            lstErrorMessages.Text = "თქვენ მიიღებთ პაროლის აღდგენის ბმულს ელ.ფოსტაზე";
            pnlRecovery.Visible = ConfigUtil.UserRecoveryEnabled;
        }

        protected String GetReturnUrl()
        {
            if (String.IsNullOrEmpty(Request["ReturnUrl"]))
                return null;

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