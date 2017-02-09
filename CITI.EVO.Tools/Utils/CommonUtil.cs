using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CITI.EVO.Tools.Utils
{
    public static class CommonUtil
    {
        public static string RngRandomText(int length)
        {
            const String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            var crypto = new RNGCryptoServiceProvider();

            var data = new byte[length];
            crypto.GetNonZeroBytes(data);

            var result = new StringBuilder(length);

            foreach (byte b in data)
                result.Append(chars[b % (chars.Length - 1)]);

            return result.ToString();
        }

		public static String GetEncodedUrl()
		{
			return GetEncodedUrl(HttpContext.Current.Request.Url);
		}
		public static String GetEncodedUrl(Uri url)
		{
			return GetEncodedUrl(Convert.ToString(url));
		}
		public static String GetEncodedUrl(String url)
	    {
		    var bytes = Encoding.UTF8.GetBytes(url);
		    var token = HttpServerUtility.UrlTokenEncode(bytes);
		    return token;
	    }

		public static String GetDecodedUrl(String base64)
		{
			var bytes = HttpServerUtility.UrlTokenDecode(base64);
			var url = Encoding.UTF8.GetString(bytes);
			return url;

		}
	}
}
