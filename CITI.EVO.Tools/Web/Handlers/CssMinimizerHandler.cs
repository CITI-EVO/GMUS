using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace CITI.EVO.Tools.Web.Handlers
{
    public class CssMinimizerHandler : IHttpHandler
    {
        private static readonly Regex regex = new Regex(@"[\n\r ]+", RegexOptions.Compiled);

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            response.ContentType = "text/css";

            var filePath = request.PhysicalPath;
            var fileText = File.ReadAllText(filePath);

            var minimized = regex.Replace(fileText, " ");

            response.Write(minimized);
        }
    }
}
