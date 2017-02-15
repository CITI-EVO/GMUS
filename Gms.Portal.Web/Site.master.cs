using System;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web
{
    public partial class Site : MasterPageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UmUtil.Instance.IsLogged)
            {
                var loginToken = Convert.ToString(UmUtil.Instance.CurrentToken);

                foreach (var control in UserInterfaceUtil.TraverseChildren(this))
                {
                    var link = control as HyperLink;
                    if (link != null)
                        link.NavigateUrl = link.NavigateUrl.Replace("{loginToken}", loginToken);
                }
            }
            else
            {
                UmUtil.Instance.GoToLogin();
            }

            var list = (from n in HbSession.Query<GM_Form>()
                        where n.DateDeleted == null
                        orderby n.Name
                        select n).ToList();

            rptLinks.DataSource = list;
            rptLinks.DataBind();
        }

        protected void lnkLogout_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToLogout();
        }

        protected void lnkChangePassword_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToChangePassword();
        }

        protected void DisplayCurrentUserData()
        {
            var instance = UmUtil.Instance;
            if (instance != null && instance.IsLogged)
            {
                var currentUser = instance.CurrentUser;
                if (currentUser != null)
                {
                    lblLoginName.Text = currentUser.LoginName;
                    lblFirstName.Text = currentUser.FirstName;
                    lblLastName.Text = currentUser.LastName;
                }
            }
        }

        protected String GetLinkUrl(object eval)
        {
            var urlHelper = new UrlHelper("~/Pages/User/FormDataGrid.aspx");
            urlHelper["FormID"] = eval;
            urlHelper["OwnerID"] = eval;

            return urlHelper.ToEncodedUrl();
        }
    }
}
