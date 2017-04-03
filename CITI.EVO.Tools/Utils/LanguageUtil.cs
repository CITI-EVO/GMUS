using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using CITI.EVO.Proxies;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Utils
{
    public class LanguageUtil
    {
        public const String UserLanguageKey = "user_language";
        public const String RequestLanguageKey = "languagePair";

        public static void SetLanguage()
        {
            var context = HttpContext.Current;
            if (context == null)
                return;

            var request = context.Request;
            var urlHelper = HttpServerUtil.RequestUrl;

            var langPair = Convert.ToString(urlHelper[RequestLanguageKey]);
            if (!String.IsNullOrWhiteSpace(langPair))
                SetLanguage(langPair);
            else
            {
                var cookie = request.Cookies[UserLanguageKey];
                if (cookie != null && cookie.Value != null)
                    langPair = cookie.Value;
            }

            SetLanguage(langPair);
        }

        public static void SetLanguage(String languagePair)
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var response = context.Response;

                var cookie = new HttpCookie(UserLanguageKey, languagePair);
                response.Cookies.Set(cookie);
            }

            var culture = TryGetCultureInfo(languagePair);
            culture = (culture ?? CultureInfo.CurrentUICulture);

            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
        }

        public static String GetLanguage()
        {
            var context = HttpContext.Current;
            if (context == null)
                return null;

            var request = context.Request;

            String langPair = null;

            var cookie = request.Cookies[UserLanguageKey];
            if (cookie != null && cookie.Value != null)
                langPair = cookie.Value;

            var currentCulture = Thread.CurrentThread.CurrentUICulture;
            if (langPair != currentCulture.Name)
                return currentCulture.Name;

            return langPair;
        }

        public static IDictionary<String, String> GetLanguages()
        {
            var languages = CommonProxy.GetLanguages();
            languages = languages.OrderBy(n => n.EngName).ToList();

            return languages.ToDictionary(k => k.NativeName, v => v.Pair);
        }

        private static CultureInfo TryGetCultureInfo(String languagePair)
        {
            if (String.IsNullOrWhiteSpace(languagePair))
                return null;

            try
            {
                return CultureInfo.GetCultureInfo(languagePair);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
