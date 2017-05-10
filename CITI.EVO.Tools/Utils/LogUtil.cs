using System;
using System.Web;
using log4net;
using log4net.Config;

namespace CITI.EVO.Tools.Utils
{
	public static class LogUtil
	{
		private static readonly Lazy<ILog> _baseLoggerLazy;

		static LogUtil()
		{
			_baseLoggerLazy = new Lazy<ILog>(() => GetLogger("BaseLogger"));
		}

		public static ILog Logger
		{
			get { return _baseLoggerLazy.Value; }
		}

		public static void LogInfo(String message)
		{
			var logger = Logger;
			if (logger != null)
				logger.Info(message);
		}

		public static ILog GetLogger(String name)
		{
			ILog log = null;
			if (HttpContext.Current != null)
			{
				var cache = HttpContext.Current.Cache;
				var key = $"$[LogUtil_{name}]";

				log = cache[key] as ILog;

				if (log == null)
				{
					GlobalContext.Properties["LogName"] = name;

					log = LogManager.GetLogger(name);
					XmlConfigurator.Configure();

					cache.Insert(key, log, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration);
				}
			}

			if (log == null)
			{
				GlobalContext.Properties["LogName"] = name;

				log = LogManager.GetLogger(name);
				XmlConfigurator.Configure();
			}

			return log;
		}
	}
}
