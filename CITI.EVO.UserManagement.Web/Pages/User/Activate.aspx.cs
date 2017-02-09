using System;
using System.Linq;
using CITI.EVO.Tools.Extensions;
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
            if (!ConfigUtil.UserActivationEnabled)
            {
                lblMessage.Text = "მომხმარებელის ავტომატური აქტივაცია გათიშულია, გთხოვთ დაელოდოთ ადმინისტრატორს ხელოვნური აქტივირებისთვის";
                return;
            }

            var userCode = Request["UserCode"];
            if (String.IsNullOrWhiteSpace(userCode))
            {
                lblMessage.Text = "მომხმარებელი ვერ მოიძებნა";
                return;
            }

            var user = (from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null &&
                              n.UserCode == userCode.Trim()
                        select n).FirstOrDefault();

            if (user == null)
            {
                lblMessage.Text = "მომხმარებელი ვერ მოიძებნა";
                return;
            }

            user.IsActive = true;
            user.UserCode = null;

            EmailUtil.SendActivatedEmail(user);

            HbSession.SubmitUpdate(user);

            lblMessage.Text = String.Format("მომხმარებელი ({0} - {1} {2}) აქტივირებულია", user.LoginName, user.FirstName, user.LastName);
        }
    }
}