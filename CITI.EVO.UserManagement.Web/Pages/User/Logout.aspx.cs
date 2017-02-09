using System;
using System.Text;
using System.Web;
using CITI.EVO.Tools.Security;

namespace CITI.EVO.UserManagement.Web.Pages.User
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UmUtil.Instance.Logout();

            var redirectUrl = GetReturnUrl();
            if (String.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "~/Pages/Login.aspx";
            }

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