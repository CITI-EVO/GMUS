using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CITI.EVO.Tools.Helpers;

namespace CITI.EVO.Tools.Utils
{
    public static class DateTimeParser
    {
        private static readonly char[] _symbolsArr = { '.', '/', '\\', '-', ':', ' ' };
        private static readonly ISet<char> _symbolsSet = new HashSet<char>(_symbolsArr);

        public static bool TryParse(String value, IEnumerable<String> formats, out DateTime result)
        {
            result = default(DateTime);

            foreach (var format in formats)
            {
                if (value.Length != format.Length)
                    continue;

                if (TryParse(value, format, out result))
                    return true;
            }

            return false;
        }

        public static bool TryParse(String value, String format, out DateTime result)
        {
            var inputTokenizer = new StringTokenizer(value, _symbolsSet);
            var formatTokenizer = new StringTokenizer(format, _symbolsSet);

            return TryParse(inputTokenizer, formatTokenizer, out result);
        }

        private static bool TryParse(StringTokenizerOpt inputTokenizer, StringTokenizerOpt formatTokenizer, out DateTime result)
        {
            result = default(DateTime);

            int year = 1;
            int month = 1;
            int day = 1;

            int hour = 0;
            int minute = 0;
            int second = 0;
            int milisecond = 0;

            while (inputTokenizer.MoveNext() && formatTokenizer.MoveNext())
            {
                var valueItem = inputTokenizer.Current;
                var formatItem = formatTokenizer.Current;

                if (formatItem.Length != valueItem.Length)
                    return false;

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
                            if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out year))
                                return false;
                        }
                        break;
                    case "yyyy":
                        {
                            if (!int.TryParse(valueItem, NumberStyles.None, NumberFormatInfo.InvariantInfo, out year))
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
