﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CITI.EVO.TwoWayModel.Utils
{
    public static class DateTimeParser
    {
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

        static DateTimeParser()
        {
            var dateFormatsList = (from dateFormat in dateFormats
                                   from timeFormat in timeFormats
                                   let dateTimeFormat = String.Concat(dateFormat, " ", timeFormat)
                                   select dateTimeFormat.Trim()).ToList();

            dateFormatsList.Add("yyyy-MM-ddTHH:mm:ss.fffffffzzz");

            AllowedDateTimeFormats = dateFormatsList.ToArray();
        }

        public static bool TryParse(String value, out DateTime result)
        {
            return TryParse(value, AllowedDateTimeFormats, out result);
        }

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
