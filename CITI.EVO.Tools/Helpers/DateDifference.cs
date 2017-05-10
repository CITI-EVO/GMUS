using System;

namespace CITI.EVO.Tools.Helpers
{
    public class DateDifference
    {
        /// <summary>
        /// defining Number of days in month; index 0=> january and 11=> December
        /// february contain either 28 or 29 days, that's why here value is -1
        /// which wil be calculate later.
        /// </summary>
        //private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// contain from date
        /// </summary>
        private DateTime fromDate;

        /// <summary>
        /// contain To Date
        /// </summary>
        private DateTime toDate;

        /// <summary>
        /// this three variable for output representation..
        /// </summary>
        private readonly int year;
        private readonly int month;
        private readonly int day;

        public DateDifference(DateTime xDate, DateTime yDate)
        {
            if (xDate > yDate)
            {
                fromDate = yDate;
                toDate = xDate;
            }
            else
            {
                fromDate = xDate;
                toDate = yDate;
            }

            // Day Calculation
            var increment = 0;

            if (fromDate.Day > toDate.Day)
            {
                increment = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            }

            // if it is february month
            // if it's to day is less then from day
            if (increment == -1)
            {
                increment = DateTime.IsLeapYear(fromDate.Year) ? 29 : 28;
            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }

            //
            //month calculation
            //
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }

            //
            // year calculation
            //
            year = toDate.Year - (fromDate.Year + increment);
        }

        public override string ToString()
        {
            return $"{year} Year(s), {month} Month(s), {day} Day(s)";
        }

        public int Years
        {
            get
            {
                return year;
            }
        }

        public int Months
        {
            get
            {
                return month;
            }
        }

        public int Days
        {
            get
            {
                return day;
            }
        }
    }
}
