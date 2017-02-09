using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace CITI.EVO.Tools.Web.Modules
{
    /// <summary>
    /// Find scripts and change the src to the ScriptCompressorHandler.
    /// </summary>
    public class ScriptCompressorModule : IHttpModule
    {

        #region IHttpModule Members

        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += context_BeginRequest;
        }

        #endregion

        void context_BeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app != null)
            {
                if (app.Context.CurrentHandler is Page && !app.Request.RawUrl.Contains("serviceframe"))
                {
                    if (!app.Context.Request.Url.Scheme.Contains("https"))
                    {
                        app.Response.Filter = new WebResourceFilter(app.Response.Filter);
                    }
                }
            }
        }


        private class WebResourceFilter : Stream
        {

            private readonly Stream _stream;

            public WebResourceFilter(Stream stream)
            {
                _stream = stream;
            }


            #region Properites

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override void Flush()
            {
                _stream.Flush();
            }

            public override long Length
            {
                get { return 0; }
            }

            private long _position;
            public override long Position
            {
                get { return _position; }
                set { _position = value; }
            }

            #endregion

            #region Methods

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _stream.Read(buffer, offset, count);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var html = Encoding.Default.GetString(buffer);

                var index = 0;
                var list = new List<String>();

                var regex = new Regex("<script\\s*src=\"((?=[^\"]*(webresource.axd|scriptresource.axd))[^\"]*)\"\\s*type=\"text/javascript\"[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase);
                foreach (Match match in regex.Matches(html))
                {
                    if (index == 0)
                        index = html.IndexOf(match.Value);

                    var relative = match.Groups[1].Value;

                    list.Add(relative);

                    html = html.Replace(match.Value, String.Empty);
                }

                if (index > 0)
                {
                    var script = "<script type=\"text/javascript\" src=\"js.axd?path={0}\"></script>";

                    var path = String.Empty;
                    foreach (var s in list)
                        path += HttpUtility.UrlEncode(s) + ",";

                    html = html.Insert(index, string.Format(script, path));
                }

                var outdata = Encoding.Default.GetBytes(html);

                _stream.Write(outdata, 0, outdata.GetLength(0));
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return _stream.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                _stream.SetLength(value);
            }

            public override void Close()
            {
                _stream.Close();
            }

            #endregion
        }


    }
}