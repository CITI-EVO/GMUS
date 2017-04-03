using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Activate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlError.Visible = false;
            pnlUser.Visible = false;

            if (!ConfigUtil.UserActivationEnabled)
            {
                lblError.Text = "მომხმარებელის ავტომატური აქტივაცია გათიშულია, გთხოვთ დაელოდოთ ადმინისტრატორს ხელოვნური აქტივირებისთვის";
                pnlError.Visible = true;
                return;
            }

            var userCode = Request["UserCode"];
            if (String.IsNullOrWhiteSpace(userCode))
            {
                lblError.Text = "მომხმარებელი ვერ მოიძებნა";
                pnlError.Visible = true;
                return;
            }

            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.UserCode == userCode.Trim()
                        select n).FirstOrDefault();

            if (user == null)
            {
                lblError.Text = "მომხმარებელი ვერ მოიძებნა";
                pnlError.Visible = true;
                return;
            }

            user.IsActive = true;
            user.UserCode = null;

            EmailUtil.SendActivatedEmail(user);

            HbSession.SubmitUpdate(user);

            if (!UmUtil.Instance.Login(user.LoginName, user.Password))
                throw new Exception();

            var url = new UrlHelper("/Rnsf/Gms/Gms.Portal.Web/");
            url["loginToken"] = UmUtil.Instance.CurrentToken;

            lnkAuth.NavigateUrl = url.ToString();

            lblUser.Text = String.Format("({0} - {1} {2})", user.LoginName, user.FirstName, user.LastName);
            pnlUser.Visible = true;
        }
    }
}