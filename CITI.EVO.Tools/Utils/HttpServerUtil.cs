using System;
using System.Web;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Utils
{
    public static class HttpServerUtil
    {
        private const String ItemKey = "@{RequestUrl}";

        public static UrlHelper RequestUrl
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null)
                    return null;

                var urlHelper = context.Items[ItemKey] as UrlHelper;
                if (urlHelper == null)
                {
                    urlHelper = new UrlHelper(context.Request.Url);
                    context.Items[ItemKey] = urlHelper;
                }

                return urlHelper;
            }
        }

        public static String ResolveAbsoluteUrl(String relativeUrl)
        {
            var context = HttpContext.Current;
            if (context == null)
                return null;

            var requestUrl = context.Request.Url;
            var absoluteUrl = VirtualPathUtility.ToAbsolute(relativeUrl);

            if (requestUrl.IsDefaultPort)
            {
                var result = $"{requestUrl.Scheme}://{requestUrl.Host}{absoluteUrl}";
                return result;
            }
            else
            {
                var result = $"{requestUrl.Scheme}://{requestUrl.Host}:{requestUrl.Port}{absoluteUrl}";
                return result;
            }
        }
    }
}
