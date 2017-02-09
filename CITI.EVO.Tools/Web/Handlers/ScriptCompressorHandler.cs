using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Web;

namespace CITI.EVO.Tools.Web.Handlers
{
    public class ScriptCompressorHandler : IHttpHandler
    {
        private const String GZIP = "gzip";
        private const String DEFLATE = "deflate";

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom 
        /// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
        /// references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var cache = context.Cache;
            var request = context.Request;
            var response = context.Response;

            var content = GetContent(cache, request);

            if (!String.IsNullOrEmpty(content))
            {
                response.Write(content);

                Compress(request, response);
            }
        }

        private String GetContent(System.Web.Caching.Cache cache, HttpRequest request)
        {
            var path = request["path"];
            var root = request.Url.GetLeftPart(UriPartial.Authority);

            var buffer = new StringBuilder();

            if (!String.IsNullOrEmpty(path))
            {
                var content = (String)cache[path];

                if (cache[path] == null)
                {
                    var scripts = path.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var script in scripts)
                    {
                        // We only want to serve resource files for security reasons.
                        if (script.ToUpperInvariant().Contains("RESOURCE.AXD"))
                        {
                            var scriptText = RetrieveScript(root + script);
                            buffer.AppendLine(scriptText);
                        }
                    }

                    content = buffer.ToString();

                    cache.Insert(path, content, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5));
                }

                return content;
            }

            return null;
        }

        private String RetrieveScript(String file)
        {
            try
            {
                var url = new Uri(file, UriKind.Absolute);

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                            return reader.ReadToEnd();
                    }
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                // The remote site is currently down. Try again next time.
            }
            catch (UriFormatException)
            {
                // Only valid absolute URLs are accepted
            }

            return null;
        }

        private void Compress(HttpRequest request, HttpResponse response)
        {
            if (request.UserAgent != null && request.UserAgent.Contains("MSIE 6"))
                return;

            if (IsEncodingAccepted(request, GZIP))
            {
                response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                SetEncoding(response, GZIP);
            }
            else if (IsEncodingAccepted(request, DEFLATE))
            {
                response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                SetEncoding(response, DEFLATE);
            }
        }

        private bool IsEncodingAccepted(HttpRequest request, String encoding)
        {
            var acceptEncoding = request.Headers["Accept-encoding"];
            return acceptEncoding != null && acceptEncoding.Contains(encoding);
        }

        private void SetEncoding(HttpResponse response, String encoding)
        {
            response.AppendHeader("Content-encoding", encoding);
        }
    }
}