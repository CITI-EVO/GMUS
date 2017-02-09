using log4net;
using log4net.Config;

namespace CITI.EVO.CommonData.Web.Utils
{
	/// <summary>
	/// Summary description for LogUtil
	/// </summary>
	public class LogUtil
	{
		private static ILog log;
		public static ILog Log
		{
			get
			{
				if (log == null)
				{
					log = LogManager.GetLogger("CommonData");
					XmlConfigurator.Configure();
				}

				return log;
			}
		}
	}
}