using System;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.UserManagement.Web.Bases;

namespace CITI.EVO.UserManagement.Web
{
    public partial class Site : MasterPageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper(RequestUrl);

            urlHelper[LanguageUtil.RequestLanguageKey] = "en-US";
            btEngLang.NavigateUrl = urlHelper.ToEncodedUrl();

            urlHelper[LanguageUtil.RequestLanguageKey] = "ka-GE";
            btGeoLang.NavigateUrl = urlHelper.ToEncodedUrl();
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
        }

        protected void lnkLogout_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToLogout();
        }

        protected void lnkChangePassword_OnClick(object sender, EventArgs e)
        {
            UmUtil.Instance.GoToChangePassword();
        }
    }
}
