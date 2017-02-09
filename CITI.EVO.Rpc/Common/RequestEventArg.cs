using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace CITI.EVO.Rpc.Common
{
    public class RequestEventArg : EventArgs
    {
        internal RequestEventArg(HttpRequest request, XElement xElement)
        {
            HttpMethod = request.HttpMethod;

            Url = request.Url;
            UserAgent = request.UserAgent;
            UrlReferrer = request.UrlReferrer;
            UserHostAddress = request.UserHostAddress;
            UserHostName = request.UserHostName;
            UserLanguages = request.UserLanguages;
            RawUrl = request.RawUrl;
            QueryString = request.QueryString;

            Content = xElement;
        }

        internal RequestEventArg(HttpListenerRequest request, XElement xElement)
        {
            HttpMethod = request.HttpMethod;

            Url = request.Url;
            UserAgent = request.UserAgent;
            UrlReferrer = request.UrlReferrer;
            UserHostAddress = request.UserHostAddress;
            UserHostName = request.UserHostName;
            UserLanguages = request.UserLanguages;
            RawUrl = request.RawUrl;
            QueryString = request.QueryString;

            Content = xElement;
        }

        public XElement Content { get; private set; }

        public String HttpMethod { get; private set; }

        public Uri Url { get; private set; }

        public String UserAgent { get; private set; }

        public Uri UrlReferrer { get; private set; }

        public String UserHostAddress { get; private set; }

        public String UserHostName { get; private set; }

        public String[] UserLanguages { get; private set; }

        public String ServiceName { get; private set; }

        public String RawUrl { get; private set; }

        public NameValueCollection QueryString { get; private set; }

        public bool Cancel { get; set; }
    }
}
