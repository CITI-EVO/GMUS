using System;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using CITI.EVO.CommonData.Web.Utils;

namespace CITI.EVO.CommonData.Web.Helpers
{
    public static class HttpSmsHelper
    {
        private static readonly Regex _regex;
        private static readonly StringComparer _comparer;

        static HttpSmsHelper()
        {
            _regex = new Regex(@"\{(?<key>.+?)\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _comparer = StringComparer.OrdinalIgnoreCase;
        }

        public static bool SendSms(String number, String text)
        {
            var urlFormat = ConfigurationManager.AppSettings["SmsUrlFormat"];

            var smsUrl = _regex.Replace(urlFormat, m => Replacer(m, number, text));
            smsUrl = Uri.EscapeUriString(smsUrl);

            using (var client = new WebClient())
            {
                var response = client.DownloadString(smsUrl);
                LogUtil.Log.Info(smsUrl);
                if (_comparer.Equals(response, "Y"))
                    return true;

                throw new Exception("Unable to send SMS");
            }
        }

        private static String Replacer(Match match, String number, String text)
        {
            var key = match.Groups["key"].Value;

            if (_comparer.Equals(key, "number"))
                return number;

            if (_comparer.Equals(key, "text"))
                return text;

            return key;
        }
    }
}