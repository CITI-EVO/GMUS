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
    }
}
