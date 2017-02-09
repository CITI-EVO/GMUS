using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace CITI.EVO.Tools.Utils
{
	public static class DataConverter
	{
		private static readonly Regex doubleRegex = new Regex(@"(?<mantis>^(-\d*)|^(\d*))[,\.](?<exponent>\d+$)|^(?<mantis>\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static readonly NumberFormatInfo invariantNumberFormat = NumberFormatInfo.InvariantInfo;
		private static readonly DateTimeFormatInfo invariantDateFormat = DateTimeFormatInfo.InvariantInfo;

		private static readonly String numberDecimalSeparator = invariantNumberFormat.NumberDecimalSeparator;

		private static readonly StringComparer ingnoreCaseComparer = StringComparer.OrdinalIgnoreCase;

		private static readonly String[] timeFormats =
		{
			"",

			"H:mm", "HH:mm", "H:m", "HH:m",
			"H:mm:ss", "HH:mm:ss", "H:m:ss", "HH:m:ss",
			"H:mm:ss.fff", "HH:mm:ss.fff", "H:m:ss.fff", "HH:m:ss.fff",
		};

		private static readonly String[] dateFormats =
		{
			"d.M.yyyy", "d-M-yyyy", "d/M/yyyy",
			"d.MM.yyyy", "d-MM-yyyy", "d/MM/yyyy",
			"dd.M.yyyy", "dd-M-yyyy", "dd/M/yyyy",
			"dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy",

			"yyyy.M.d", "yyyy-M-d", "yyyy/M/d",
			"yyyy.MM.d", "yyyy-MM-d", "yyyy/MM/d",
			"yyyy.M.dd", "yyyy-M-dd", "yyyy/M/dd",
			"yyyy.MM.dd", "yyyy-MM-dd", "yyyy/MM/dd",

			"d.M.yy", "d-M-yy", "d/M/yy",
			"d.MM.yy", "d-MM-yy", "d/MM/yy",
			"dd.M.yy", "dd-M-yy", "dd/M/yy",
			"dd.MM.yy", "dd-MM-yy", "dd/MM/yy",

			"yy.M.d", "yy-M-d", "yy/M/d",
			"yy.MM.d", "yy-MM-d", "yy/MM/d",
			"yy.M.dd", "yy-M-dd", "yy/M/dd",
			"yy.MM.dd", "yy-MM-dd", "yy/MM/dd",
		};

		public static readonly String[] AllowedDateTimeFormats;

		static DataConverter()
		{
			var dateFormatsList = (from dateFormat in dateFormats
								   from timeFormat in timeFormats
								   let dateTimeFormat = String.Concat(dateFormat, " ", timeFormat)
								   select dateTimeFormat.Trim()).ToList();

			dateFormatsList.Add("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

			AllowedDateTimeFormats = dateFormatsList.ToArray();
		}

		public static bool IsConvertable(Object value, Type type)
		{
			return IsConvertable(value, type.FullName);
		}
		public static bool IsConvertable(Object value, String typeName)
		{
			return TryChangeType(value, typeName, out value);
		}

		public static Object ChangeType(Object value, String typeName)
		{
			if (String.IsNullOrWhiteSpace(typeName))
			{
				return value;
			}

			var strValue = Convert.ToString(value);
			if (String.IsNullOrWhiteSpace(strValue))
			{
				return null;
			}

			typeName = typeName.ToLower();

			switch (typeName)
			{
				case "bool":
				case "boolean":
				case "system.bool":
				case "system.boolean":
					{
						if (ingnoreCaseComparer.Equals(strValue, "on"))
						{
							strValue = "true";
						}
						else if (ingnoreCaseComparer.Equals(strValue, "off"))
						{
							strValue = "false";
						}

						return Convert.ToBoolean(strValue, invariantNumberFormat);
					}
				case "byte":
				case "system.byte":
					{
						return Convert.ToByte(value, invariantNumberFormat);
					}
				case "sbyte":
				case "system.sbyte":
					{
						return Convert.ToSByte(value, invariantNumberFormat);
					}
				case "short":
				case "int16":
				case "system.short":
				case "system.int16":
					{
						return Convert.ToInt16(value, invariantNumberFormat);
					}
				case "ushort":
				case "uint16":
				case "system.ushort":
				case "system.uint16":
					{
						return Convert.ToUInt16(value, invariantNumberFormat);
					}
				case "int":
				case "int32":
				case "system.int":
				case "system.int32":
					{
						return Convert.ToInt32(value, invariantNumberFormat);
					}
				case "uint":
				case "uint32":
				case "system.uint":
				case "system.uint32":
					{
						return Convert.ToUInt32(value, invariantNumberFormat);
					}
				case "long":
				case "int64":
				case "system.long":
				case "system.int64":
					{
						return Convert.ToInt64(value, invariantNumberFormat);
					}
				case "ulong":
				case "uint64":
				case "system.ulong":
				case "system.uint64":
					{
						return Convert.ToUInt64(value, invariantNumberFormat);
					}
				case "float":
				case "single":
				case "system.float":
				case "system.single":
					{
						return Convert.ToSingle(value, invariantNumberFormat);
					}
				case "double":
				case "system.double":
					{
						return Convert.ToDouble(value, invariantNumberFormat);
					}
				case "decimal":
				case "system.decimal":
					{
						return Convert.ToDecimal(value, invariantNumberFormat);
					}
				case "datetime":
				case "system.datetime":
					{
						var dateTime = ToNullableDateTime(value);
						if (dateTime != null)
						{
							return dateTime.Value;
						}

						return null;
					}
				case "timespan":
				case "system.timespan":
					{
						var timeSpan = ToNullableTimeSpan(value);
						if (timeSpan != null)
						{
							return timeSpan.Value;
						}

						return null;
					}
				case "guid":
				case "system.guid":
					{
						Guid convertedValue;
						if (Guid.TryParse(strValue, out convertedValue))
						{
							return convertedValue;
						}
						return null;
					}
				case "string":
				case "system.string":
					{
						return strValue;
					}
			}

			return value;
		}
		public static Object ChangeType(Object value, Type type)
		{
			return Convert.ChangeType(value, type);
		}

		public static bool TryChangeType(Object value, Type type, out Object result)
		{
			return TryChangeType(value, type.FullName, out result);
		}
		public static bool TryChangeType(Object value, String typeName, out Object result)
		{
			result = value;

			if (String.IsNullOrWhiteSpace(typeName))
			{
				return true;
			}

			var strValue = Convert.ToString(value);
			if (String.IsNullOrWhiteSpace(strValue))
			{
				result = null;
				return true;
			}

			typeName = typeName.ToLower();

			bool converted = false;

			switch (typeName)
			{
				case "bool":
				case "boolean":
				case "system.bool":
				case "system.boolean":
					{
						if (ingnoreCaseComparer.Equals(strValue, "on"))
						{
							strValue = "true";
						}
						else if (ingnoreCaseComparer.Equals(strValue, "off"))
						{
							strValue = "false";
						}

						bool convertedValue;
						converted = bool.TryParse(strValue, out convertedValue);
						result = convertedValue;
					}
					break;
				case "byte":
				case "system.byte":
					{
						byte convertedValue;
						converted = byte.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "sbyte":
				case "system.sbyte":
					{
						sbyte convertedValue;
						converted = sbyte.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "short":
				case "int16":
				case "system.short":
				case "system.int16":
					{
						Int16 convertedValue;
						converted = Int16.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "ushort":
				case "uint16":
				case "system.ushort":
				case "system.uint16":
					{
						UInt16 convertedValue;
						converted = UInt16.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "int":
				case "int32":
				case "system.int":
				case "system.int32":
					{
						Int32 convertedValue;
						converted = Int32.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "uint":
				case "uint32":
				case "system.uint":
				case "system.uint32":
					{
						UInt32 convertedValue;
						converted = UInt32.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "long":
				case "int64":
				case "system.long":
				case "system.int64":
					{
						Int64 convertedValue;
						converted = Int64.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "ulong":
				case "uint64":
				case "system.ulong":
				case "system.uint64":
					{
						UInt64 convertedValue;
						converted = UInt64.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out convertedValue);
						result = convertedValue;
					}
					break;
				case "float":
				case "single":
				case "system.float":
				case "system.single":
					{
						String correctDouble;
						if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
						{
							float convertedValue;
							converted = float.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out convertedValue);
							result = convertedValue;
						}
					}
					break;
				case "double":
				case "system.double":
					{
						String correctDouble;
						if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
						{
							double convertedValue;
							converted = double.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out convertedValue);
							result = convertedValue;
						}
					}
					break;
				case "decimal":
				case "system.decimal":
					{
						String correctDouble;
						if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
						{
							decimal convertedValue;
							converted = decimal.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out convertedValue);
							result = convertedValue;
						}
					}
					break;
				case "datetime":
				case "system.datetime":
					{
						var dateTime = ToNullableDateTime(value);
						converted = (dateTime != null);
						result = (converted ? (Object)dateTime.Value : null);
					}
					break;
				case "timespan":
				case "system.timespan":
					{
						TimeSpan convertedValue;
						converted = TimeSpan.TryParseExact(strValue, "G", CultureInfo.InvariantCulture, out convertedValue);
						result = convertedValue;
					}
					break;
				case "guid":
				case "system.guid":
					{
						Guid convertedValue;
						converted = Guid.TryParse(strValue, out convertedValue);
						result = convertedValue;
					}
					break;
				case "string":
				case "system.string":
					{
						result = Convert.ToString(value);
						converted = true;
					}
					break;
			}

			return converted;
		}

		public static Object ConvertToAny(Object value)
		{
			var strValue = Convert.ToString(value);
			if (String.IsNullOrWhiteSpace(strValue))
			{
				return strValue;
			}

			Guid guid;
			if (Guid.TryParse(strValue, out guid))
			{
				return guid;
			}

			String correctDouble;
			if (TryGetCorrectDouble(strValue, out correctDouble))
			{
				decimal dec;
				if (decimal.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out dec))
				{
					return dec;
				}
			}
			else
			{
				long lng;
				if (long.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out lng))
				{
					return lng;
				}

				ulong ulng;
				if (ulong.TryParse(strValue, NumberStyles.Any, invariantNumberFormat, out ulng))
				{
					return ulng;
				}
			}

			DateTime dateTime;
			if (TryGetDateTime(strValue, out dateTime))
			{
				return dateTime;
			}

			return strValue;
		}

		public new static bool Equals(Object value1, Object value2)
		{
			return Object.Equals(value1, value2);
		}

		public static byte[] FromBase64String(String value)
		{
			return Convert.FromBase64String(value);
		}

		public static String ToBase64String(byte[] value)
		{
			return Convert.ToBase64String(value);
		}

		public static bool ToBool(Object value)
		{
			return ToBoolean(value);
		}
		public static bool? ToNullableBool(Object value)
		{
			return ToNullableBoolean(value);
		}

		public static Boolean ToBoolean(Object value)
		{
			return Convert.ToBoolean(value, invariantNumberFormat);
		}
		public static Boolean? ToNullableBoolean(Object value)
		{
			if (value is Boolean)
			{
				return (Boolean)value;
			}

			Boolean result;
			if (!Boolean.TryParse(Convert.ToString(value), out result))
			{
				return null;
			}

			return result;
		}

		public static Byte? ToByte(Object value)
		{
			return Convert.ToByte(value, invariantNumberFormat);
		}
		public static Byte? ToNullableByte(Object value)
		{
			if (value is Byte)
			{
				return (Byte)value;
			}

			Byte result;
			if (!Byte.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static Char ToChar(Object value)
		{
			return Convert.ToChar(value, invariantNumberFormat);
		}

		public static DateTime ToDateTime(Object value)
		{
			var strValue = Convert.ToString(value);
			if (String.IsNullOrWhiteSpace(strValue))
			{
				throw new InvalidDataException();
			}

			DateTime dateTimeValue;
			if (TryGetDateTime(strValue, out dateTimeValue))
			{
				return dateTimeValue;
			}

			throw new InvalidDataException();
		}
		public static DateTime? ToNullableDateTime(Object value)
		{
			var strValue = Convert.ToString(value);
			if (String.IsNullOrWhiteSpace(strValue))
			{
				return null;
			}

			DateTime dateTimeValue;
			if (TryGetDateTime(strValue, out dateTimeValue))
			{
				return dateTimeValue;
			}

			return null;
		}

		public static Decimal ToDecimal(Object value)
		{
			return Convert.ToDecimal(value, invariantNumberFormat);
		}
		public static Decimal? ToNullableDecimal(Object value)
		{
			if (value is Decimal)
			{
				return (Decimal)value;
			}

			String correctDouble;
			if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
			{
				decimal dec;
				if (decimal.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out dec))
				{
					return dec;
				}
			}

			return null;
		}

		public static Double ToDouble(Object value)
		{
			return Convert.ToDouble(value, invariantNumberFormat);
		}
		public static Double? ToNullableDouble(Object value)
		{
			if (value is Double)
			{
				return (Double)value;
			}

			String correctDouble;
			if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
			{
				Double dec;
				if (Double.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out dec))
				{
					return dec;
				}
			}

			return null;
		}

		public static short ToShort(Object value)
		{
			return ToInt16(value);
		}
		public static short? ToNullableShort(Object value)
		{
			return ToNullableInt16(value);
		}

		public static Int16 ToInt16(Object value)
		{
			return Convert.ToInt16(value, invariantNumberFormat);
		}
		public static Int16? ToNullableInt16(Object value)
		{
			if (value is Int16)
			{
				return (Int16)value;
			}

			Int16 result;
			if (!Int16.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static int ToInt(Object value)
		{
			return ToInt32(value);
		}
		public static int? ToNullableInt(Object value)
		{
			return ToNullableInt32(value);
		}

		public static Int32 ToInt32(Object value)
		{
			return Convert.ToInt32(value, invariantNumberFormat);
		}
		public static Int32? ToNullableInt32(Object value)
		{
			if (value is Int32)
			{
				return (Int32)value;
			}

			Int32 result;
			if (!Int32.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static long ToLong(Object value)
		{
			return ToInt64(value);
		}
		public static long? ToNullableLong(Object value)
		{
			return ToNullableInt64(value);
		}

		public static Int64 ToInt64(Object value)
		{
			return Convert.ToInt64(value, invariantNumberFormat);
		}
		public static Int64? ToNullableInt64(Object value)
		{
			if (value is Int64)
			{
				return (Int64)value;
			}

			Int64 result;
			if (!Int64.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static SByte ToSByte(Object value)
		{
			return Convert.ToSByte(value, invariantNumberFormat);
		}
		public static SByte? ToNullableSByte(Object value)
		{
			if (value is SByte)
			{
				return (SByte)value;
			}

			SByte result;
			if (!SByte.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static float ToFloat(Object value)
		{
			return ToSingle(value);
		}
		public static float? ToNullableFloat(Object value)
		{
			return ToNullableSingle(value);
		}

		public static Single ToSingle(Object value)
		{
			return Convert.ToSingle(value, invariantNumberFormat);
		}
		public static Single? ToNullableSingle(Object value)
		{
			if (value is Single)
			{
				return (Single)value;
			}

			String correctDouble;
			if (TryGetCorrectDouble(Convert.ToString(value), out correctDouble))
			{
				Single dec;
				if (Single.TryParse(correctDouble, NumberStyles.Any, invariantNumberFormat, out dec))
				{
					return dec;
				}
			}

			return null;
		}

		public static String ToString(Object value)
		{
			return Convert.ToString(value);
		}

		public static ushort ToUShort(Object value)
		{
			return ToUInt16(value);
		}
		public static ushort? ToNullableUShort(Object value)
		{
			return ToNullableUInt16(value);
		}

		public static UInt16 ToUInt16(Object value)
		{
			return Convert.ToUInt16(value, invariantNumberFormat);
		}
		public static UInt16? ToNullableUInt16(Object value)
		{
			if (value is UInt16)
			{
				return (UInt16)value;
			}

			UInt16 result;
			if (!UInt16.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static uint ToUInt(Object value)
		{
			return ToUInt32(value);
		}
		public static uint? ToNullableUInt(Object value)
		{
			return ToNullableUInt32(value);
		}

		public static UInt32 ToUInt32(Object value)
		{
			return Convert.ToUInt32(value, invariantNumberFormat);
		}
		public static UInt32? ToNullableUInt32(Object value)
		{
			if (value is UInt32)
			{
				return (UInt32)value;
			}

			UInt32 result;
			if (!UInt32.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static ulong ToULong(Object value)
		{
			return ToUInt64(value);
		}
		public static ulong? ToNullableULong(Object value)
		{
			return ToNullableUInt64(value);
		}

		public static UInt64 ToUInt64(Object value)
		{
			return Convert.ToUInt64(value, invariantNumberFormat);
		}
		public static UInt64? ToNullableUInt64(Object value)
		{
			UInt64 result;
			if (!UInt64.TryParse(Convert.ToString(value), NumberStyles.Any, invariantNumberFormat, out result))
			{
				return null;
			}

			return result;
		}

		public static Guid ToGuid(Object value)
		{
			var result = ToNullableGuid(value);
			return result.GetValueOrDefault();
		}
		public static Guid? ToNullableGuid(Object value)
		{
			if (value is Guid)
			{
				return (Guid)value;
			}

			var strValue = Convert.ToString(value);

			Guid guid;
			if (Guid.TryParse(strValue, out guid))
			{
				return guid;
			}

			return null;
		}

		public static BigInteger ToBigInteger(Object value)
		{
			if (value is BigInteger)
			{
				return (BigInteger)value;
			}

			var strValue = Convert.ToString(value);
			return BigInteger.Parse(strValue);
		}
		public static BigInteger? ToNullableBigInteger(Object value)
		{
			if (value is BigInteger)
			{
				return (BigInteger)value;
			}

			var strValue = Convert.ToString(value);

			BigInteger bigInteger;
			if (BigInteger.TryParse(strValue, out bigInteger))
			{
				return bigInteger;
			}

			return null;
		}

		public static TimeSpan ToTimeSpan(Object value)
		{
			if (value is TimeSpan)
			{
				return (TimeSpan)value;
			}

			var strValue = Convert.ToString(value);
			return TimeSpan.Parse(strValue);
		}
		public static TimeSpan? ToNullableTimeSpan(Object value)
		{
			if (value is TimeSpan)
			{
				return (TimeSpan)value;
			}

			var strValue = Convert.ToString(value);

			TimeSpan timeSpan;
			if (TimeSpan.TryParse(strValue, out timeSpan))
			{
				return timeSpan;
			}

			return null;
		}

		public static TEnum ToEnum<TEnum>(Object value)
		{
			return (TEnum)Enum.Parse(typeof(TEnum), Convert.ToString(value));
		}
		public static TEnum? ToNullableEnum<TEnum>(Object value) where TEnum : struct, IConvertible
		{
			TEnum result;
			if (Enum.TryParse(Convert.ToString(value), out result))
			{
				return result;
			}

			return null;
		}

		public static int GetDaysCount(DateTime startDate, DateTime endDate, params DayOfWeek[] holidays)
		{
			var daysCount = 0;
			for (var date = startDate; date < endDate; date = date.AddDays(1))
			{
				if (holidays.Contains(date.DayOfWeek))
				{
					continue;
				}

				daysCount++;
			}

			return daysCount;
		}

		public static int GetBusinessDays(DateTime startDate, DateTime endDate, bool excludeWeekends, List<DateTime> excludeDates)
		{
			var count = 0;
			for (var index = startDate; index <= endDate; index = index.AddDays(1))
			{
				if (excludeWeekends && (index.DayOfWeek == DayOfWeek.Sunday || index.DayOfWeek == DayOfWeek.Saturday))
					continue;
				var excluded = excludeDates.Any(t => index.Date.CompareTo(t.Date) == 0);

				if (!excluded)
				{
					count++;
				}
			}

			return count;
		}

		public static DateTime? GetDateByVacationDayCount(DateTime? startDate, DateTime? endDate, bool excludeWeekends, List<DateTime> excludeDates, int vacationDaysCount)
		{
			var count = 0;
			var index = startDate != null ? startDate.GetValueOrDefault() : endDate.GetValueOrDefault();
			var increment = startDate == null ? -1 : 1;

			while (count < vacationDaysCount - 1)
			{
				if (excludeWeekends && (index.DayOfWeek == DayOfWeek.Sunday || index.DayOfWeek == DayOfWeek.Saturday))
					continue;

				var excluded = excludeDates.Any(t => index.Date.CompareTo(t.Date) == 0);

				if (!excluded)
				{
					count++;
				}

				index = index.AddDays(increment);
			}

			return index;
		}

		public static bool IsUnderEnum(Type type, int value)
		{
			var valueNames = Convert.ToString(Enum.ToObject(type, value));

			var valueNamesArr = Regex.Split(valueNames, @"\W", RegexOptions.IgnoreCase);

			var valueNamesSet = new HashSet<String>(valueNamesArr, StringComparer.OrdinalIgnoreCase);
			valueNamesSet.RemoveWhere(String.IsNullOrWhiteSpace);

			var enumNamesSet = new HashSet<String>(Enum.GetNames(type), StringComparer.OrdinalIgnoreCase);

			valueNamesSet.ExceptWith(enumNamesSet);

			return (valueNamesSet.Count == 0);
		}

		private static bool TryGetCorrectDouble(String text, out String result)
		{
			result = null;

			if (!doubleRegex.IsMatch(text))
			{
				return false;
			}

			var match = doubleRegex.Match(text);

			var mantis = match.Groups["mantis"].Value;
			var exponent = match.Groups["exponent"].Value;

			result = String.Concat(mantis, numberDecimalSeparator, exponent);
			return true;
		}

		private static bool TryGetDateTime(String text, out DateTime dateTime)
		{
			if (!DateTimeParser.TryParse(text, AllowedDateTimeFormats, out dateTime))
			{
				if (!DateTime.TryParseExact(text, AllowedDateTimeFormats, invariantDateFormat, DateTimeStyles.None, out dateTime))
				{
					return false;
				}
			}

			return true;
		}

		public static String MoneyToString(decimal? amount)
		{
			if (amount == null)
			{
				return String.Empty;
			}
			return ((decimal)amount).ToString("F2", CultureInfo.InvariantCulture);
		}
	}

}
