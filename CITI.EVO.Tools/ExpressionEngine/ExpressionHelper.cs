using System;
using System.Globalization;
using CITI.EVO.Tools.Utils;

namespace CITI.EVO.Tools.ExpressionEngine
{
	internal static class ExpressionHelper
	{
		public static Object GetAny(Object value)
		{
			if (IsNumber(value))
			{
				return GetNumber(value);
			}

			if (IsString(value))
			{
				return GetString(value);
			}

			if (IsDateTime(value))
			{
				return GetDateTime(value);
			}

			return value;
		}

		public static double GetNumber(Object value)
		{
			if (value is double)
			{
				return (double)value;
			}

			return double.Parse(Convert.ToString(value), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}

		public static String GetString(Object value)
		{
			var strValue = Convert.ToString(value);

			if (IsString(value))
			{
				return strValue.Substring(1, strValue.Length - 2);
			}

			return strValue;
		}

		public static String GetQuota(Object value)
		{
			var strValue = Convert.ToString(value);

			if (IsString(value))
			{
				return strValue.Substring(0, 1);
			}

			throw new Exception();
		}

		public static DateTime GetDateTime(Object value)
		{
			if (value is DateTime)
			{
				return (DateTime)value;
			}

			var strValue = Convert.ToString(value);

			if (IsDateTime(value))
			{
				strValue = strValue.Substring(1, strValue.Length - 2);

				var dateTimeValue = DateTime.ParseExact(strValue, new[] { "dd.MM.yyyy", "dd.MM.yyyy HH:mm:ss", "dd.MM.yyyy hh:mm:ss", "dd.MM.yyyy h:mm:ss" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
				return dateTimeValue;
			}

			var result = DataConverter.ToNullableDateTime(strValue);
			if (result.HasValue)
			{
				return result.Value;
			}

			throw new Exception();
		}

		public static bool IsNumber(Object value)
		{
			if (value is double)
			{
				return true;
			}

			double d;
			return double.TryParse(Convert.ToString(value), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out d);
		}

		public static bool IsString(Object value)
		{
			if (IsNumber(value))
			{
				return false;
			}

			var strValue = Convert.ToString(value);
			if (strValue.StartsWith("'"))
			{
				var nextIndex = strValue.IndexOf('\'', 1);
				if (nextIndex == strValue.Length - 1)
				{
					return true;
				}
			}
			else if (strValue.StartsWith("\""))
			{
				var nextIndex = strValue.IndexOf('\"', 1);
				if (nextIndex == strValue.Length - 1)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsDateTime(Object value)
		{

			if (value is DateTime)
			{
				return true;
			}

			if (IsNumber(value))
			{
				return false;
			}

			var strValue = Convert.ToString(value);
			if (strValue.StartsWith("["))
			{
				var nextIndex = strValue.IndexOf(']', 1);
				if (nextIndex == strValue.Length - 1)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsInteger(Object value)
		{
			if (IsNumber(value))
			{
				var number = GetNumber(value);
				return Math.Abs(Math.Truncate(number) - number) < double.Epsilon;
			}

			return false;
		}

		public static bool IsEmptyOrSpace(Object value)
		{
			var strValue = Convert.ToString(value);
			return String.IsNullOrEmpty(strValue) || String.IsNullOrEmpty(strValue.Trim());
		}
	}
}