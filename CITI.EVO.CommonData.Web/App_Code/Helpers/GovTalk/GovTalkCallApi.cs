using System;
using System.Configuration;
using System.Net;
using System.Text;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class GovTalkCallApi
    {
        /// <summary>
        /// Calls service api using request xml string
        /// </summary>
        /// <param name="requestXml">request xml string</param>
        /// <returns></returns>
        public static String GetResponse(String requestXml)
        {
            var apiUrl = ConfigurationManager.AppSettings["GovTalkApiUrl"];

            if (requestXml.IsGovTalkMessage())
            {
                if (requestXml.IsPollMessage())
                    apiUrl += "poll";
                else
                    apiUrl += "submission";
            }

            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/xml";
                client.Encoding = Encoding.UTF8;

                return client.UploadString(apiUrl, requestXml);
            }
        }
    }
}