#region

using System;
using System.Globalization;
using UIServiceLib.CORE.Helpers.IntHelpers;

#endregion

namespace UIServiceLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeExtensions
    {
        public static bool IsBelongToPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return dateTime >= endDate && dateTime <= startDate;
            return dateTime >= startDate && dateTime <= endDate;
        }

        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static bool IsNullOrNewDateTime(this DateTime? dateTime)
        {
            return dateTime == null || dateTime == new DateTime();
        }

        public static string ToMiddleDateString(this DateTime dateTime)
        {
            return $"{dateTime.ToShortDateString()} г.";
        }

        public static string ToFullLongDateString(this DateTime dateTime)
        {
            return $"{dateTime.ToLongDateString().Replace(dateTime.Day.ToString(), dateTime.Day.ToStringWithZero())}";
        }

        public static string ToFullShortTimeString(this DateTime dateTime)
        {
            return $"{dateTime.Hour.ToStringWithZero()} час. {dateTime.Minute.ToStringWithZero()} мин.";
        }

        public static string ToFullLongTimeString(this DateTime dateTime)
        {
            return
                $"{dateTime.Hour.ToStringWithZero()} час. {dateTime.Minute.ToStringWithZero()} мин. {dateTime.Second.ToStringWithZero()} сек.";
        }

        public static string ToMonthYearShortString(this DateTime dateTime)
        {
            return
                $"{dateTime.Month.ToStringWithZero()}.{dateTime.Year}";
        }

        public static string ToMonthYearLongString(this DateTime dateTime)
        {
            return
                $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month)} {dateTime.Year} г.";
        }

        public static string MonthToString(this DateTime dateTime)
        {
            return $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month)}";
        }

        public static int GetQuarter(this DateTime dateTime)
        {
            return (dateTime.Month + 2) / 3;
        }

        public static int GetFinancialQuarter(this DateTime dateTime)
        {
            return (dateTime.AddMonths(3).Month + 2) / 3;
        }
    }
}