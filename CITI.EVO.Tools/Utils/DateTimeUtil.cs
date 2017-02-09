using System;

namespace CITI.EVO.Tools.Utils
{
	public static class DateTimeUtil
	{
		public static void Split(DateTime dateTime, out DateTime date, out DateTime time)
		{
			var dateOnly = dateTime.Date;
			var timeOnly = new DateTime(dateTime.TimeOfDay.Ticks);

			date = dateOnly;
			time = timeOnly;
		}

		public static DateTime Merge(DateTime date, DateTime time)
		{
			var dateSpan = TimeSpan.FromTicks(time.Date.Ticks);
			var timeOnly = time.Subtract(dateSpan);
			var timeSpan = TimeSpan.FromTicks(timeOnly.Ticks);

			var dateOnly = date.Date;
			var mergedDate = dateOnly.Add(timeSpan);

			return mergedDate;
		}
	}
}
