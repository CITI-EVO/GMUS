using System;
using System.Reflection;
using System.Web;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Extensions
{
    public static class HttpExtensions
    {
        public static UrlHelper RequestUrl(this HttpRequest request)
        {
            if (request == null)
                return null;

            var urlHelper = new UrlHelper(request.Url);
            return urlHelper;
        }

        public static bool HeadersWritten(this HttpResponse response)
        {
            var type = response.GetType();
            var field = type.GetField("_headersWritten", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var headersWritten = (bool)field.GetValue(response);
            return headersWritten;
        }

        public static void RedirectNoCache(this HttpResponse response, String url)
        {
            //response.Clear();
            //response.ClearHeaders();
            //response.ClearContent();

            //response.Cache.SetAllowResponseInBrowserHistory(false);
            //response.Cache.SetCacheability(HttpCacheability.NoCache);
            //response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            //response.Cache.SetNoStore();

            const String timeParam = "__uthc";

            var urlHelper = new UrlHelper(url);
            urlHelper[timeParam] = DateTime.Now.Ticks;

            response.Redirect(urlHelper.ToString());
        }
    }
}
