using System;
using System.Diagnostics;

namespace CITI.EVO.Tools.Utils
{
	public static class EventLogUtil
	{
		public static void WriteError(String message)
		{
			var appLog = new EventLog();
			appLog.Source = AppDomain.CurrentDomain.BaseDirectory;
			
			appLog.WriteEntry(message, EventLogEntryType.Error);
		}

		public static void WriteInformation(String message)
		{
			var appLog = new EventLog();
			appLog.Source = AppDomain.CurrentDomain.BaseDirectory;

			appLog.WriteEntry(message, EventLogEntryType.Information);
		}

		public static void WriteWarning(String message)
		{
			var appLog = new EventLog();
			appLog.Source = AppDomain.CurrentDomain.BaseDirectory;

			appLog.WriteEntry(message, EventLogEntryType.Warning);
		}
	}
}
