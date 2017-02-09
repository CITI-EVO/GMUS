using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace CITI.EVO.Tools.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime currDate, DateTime startDate, DateTime endDate)
        {
            return (currDate >= startDate && currDate <= endDate);
        }

        public static int DiffYears(this DateTime startDate, DateTime endDate)
        {
            return (int)DateAndTime.DateDiff(DateInterval.Year, startDate, endDate);
        }

        public static int DiffMonths(this DateTime startDate, DateTime endDate)
        {
            return (int)DateAndTime.DateDiff(DateInterval.Month, startDate, endDate);
        }

        public static int DiffDays(this DateTime startDate, DateTime endDate)
        {
            return startDate.DaysLeft(endDate, false, new List<DateTime>());
        }

        public static int DiffHours(this DateTime startDate, DateTime endDate)
        {
            return (int)DateAndTime.DateDiff(DateInterval.Hour, startDate, endDate);
        }

        public static int DiffMinutes(this DateTime startDate, DateTime endDate)
        {
            return (int)DateAndTime.DateDiff(DateInterval.Minute, startDate, endDate);
        }

        public static int DiffSeconds(this DateTime startDate, DateTime endDate)
        {
            return (int)DateAndTime.DateDiff(DateInterval.Second, startDate, endDate);
        }

        public static int DaysLeft(this DateTime startDate, DateTime endDate)
        {
            return startDate.DaysLeft(endDate, false, new List<DateTime>());
        }

        public static int DaysLeft(this DateTime startDate, DateTime endDate, bool excludeWeekends)
        {
            return startDate.DaysLeft(endDate, excludeWeekends, new List<DateTime>());
        }

        public static int DaysLeft(this DateTime startDate, DateTime endDate, bool excludeWeekends, IList<DateTime> excludeDates)
        {
            int count = 0;
            for (DateTime index = startDate; index < endDate; index = index.AddDays(1))
            {
                if (excludeWeekends && index.DayOfWeek != DayOfWeek.Sunday && index.DayOfWeek != DayOfWeek.Saturday)
                {
                    bool excluded = false;
                    for (int i = 0; i < excludeDates.Count; i++)
                    {
                        if (index.Date.CompareTo(excludeDates[i].Date) == 0)
                        {
                            excluded = true;
                            break;
                        }
                    }

                    if (!excluded)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public static DateTime? Max(this DateTime? dateTime, DateTime? otherDate)
        {
            var dateArray = new[] {dateTime, otherDate};
            return dateArray.Max();
        }

        public static DateTime? Min(this DateTime? dateTime, DateTime? otherDate)
        {
            var dateArray = new[] { dateTime, otherDate };
            return dateArray.Min();
        }

        public static DateTime Max(this DateTime dateTime, DateTime otherDate)
        {
            var dateArray = new[] { dateTime, otherDate };
            return dateArray.Max();
        }

        public static DateTime Min(this DateTime dateTime, DateTime otherDate)
        {
            var dateArray = new[] { dateTime, otherDate };
            return dateArray.Min();
        }

        public static int GetQuarter(this DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }
    }
}
