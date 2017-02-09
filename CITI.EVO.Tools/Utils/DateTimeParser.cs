using System;
using System.Globalization;

namespace CITI.EVO.Tools.Utils
{
	public static class DateTimeParser
	{
		public static bool TryParse(String value, String[] formats, out DateTime result)
		{
			result = default(DateTime);

			foreach (var format in formats)
			{
				if (TryParse(value, format, out result))
					return true;
			}

			return false;
		}

		public static bool TryParse(String value, String format, out DateTime result)
		{
			result = default(DateTime);

			if (String.IsNullOrWhiteSpace(format) || String.IsNullOrWhiteSpace(value))
				return false;

			var s = new[] { '.', '/', '\\', '-', ':', ' ' };

			var formatArr = format.Split(s);
			var valuesArr = value.Split(s);

			if (formatArr.Length != valuesArr.Length)
				return false;

			int year = 1;
			int month = 1;
			int day = 1;

			int hour = 0;
			int minute = 0;
			int second = 0;
			int milisecond = 0;

			var len = formatArr.Length;

			for (int i = 0; i < len; i++)
			{
				var formatItem = formatArr[i];
				var valueItem = valuesArr[i];

				switch (formatItem)
				{
					case "d":
					case "dd":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out day) || day < 1 || day > 31)
								return false;
						}
						break;
					case "M":
					case "MM":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out month) || month < 1 || month > 12)
								return false;
						}
						break;
					case "yy":
						{
							if (valueItem.Length != 2 || !int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out year))
								return false;
						}
						break;
					case "yyyy":
						{
							if (valueItem.Length != 4 || !int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out year))
								return false;
						}
						break;
					case "h":
					case "hh":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out hour) || hour < 0 || hour > 11)
								return false;
						}
						break;
					case "H":
					case "HH":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out hour) || hour < 0 || hour > 23)
								return false;
						}
						break;
					case "m":
					case "mm":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out minute) || minute < 0 || minute > 59)
								return false;
						}
						break;
					case "s":
					case "ss":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out second) || second < 0 || second > 59)
								return false;


						}
						break;
					case "f":
					case "ff":
					case "fff":
						{
							if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out milisecond) || milisecond < 0 || milisecond > 999)
								return false;
						}
						break;
					case "s,f":
					case "s,ff":
					case "s,fff":
					case "ss,f":
					case "ss,ff":
					case "ss,fff":
						{
							double d;
							if (!double.TryParse(valueItem, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d))
								return false;

							var t = TimeSpan.FromSeconds(d);
							second = t.Seconds;
							milisecond = t.Milliseconds;
						}
						break;
					default:
						return false;
				}
			}

			if (DateTime.DaysInMonth(year, month) < day)
				return false;

			result = new DateTime(year, month, day, hour, minute, second, milisecond);

			return true;
		}
	}

}
