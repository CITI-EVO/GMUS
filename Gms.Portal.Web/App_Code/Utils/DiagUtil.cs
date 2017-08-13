using System.Web;
using CITI.EVO.Tools.Diagnostics;

namespace Gms.Portal.Web.Utils
{
    public static class DiagUtil
    {
        public static MultiStopwatch Current
        {
            get
            {
                var context = HttpContext.Current;

                var stopwatch = context.Items["MultiStopwatch"] as MultiStopwatch;
                if (stopwatch == null)
                {
                    stopwatch = new MultiStopwatch();
                    context.Items["MultiStopwatch"] = stopwatch;
                }

                return stopwatch;
            }
        }
    }
}