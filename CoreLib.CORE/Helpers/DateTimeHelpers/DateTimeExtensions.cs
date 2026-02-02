#region

using System;
using System.Globalization;

#endregion

namespace CoreLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Checks if <see cref="DateTime"/> belongs to period
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/> to check</param>
        /// <param name="startDate">Period start <see cref="DateTime"/></param>
        /// <param name="endDate">Period end <see cref="DateTime"/></param>
        /// <returns>True if provided <see cref="DateTime"/> belongs to period</returns>
        public static bool IsBelongToPeriod(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return dateTime.IsBelongToPeriod(endDate, startDate);
            }

            return dateTime >= startDate && dateTime <= endDate;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Checks if <see cref="DateOnly"/> belongs to period
        /// </summary>
        /// <param name="date"><see cref="DateOnly"/> to check</param>
        /// <param name="startDate">Period start <see cref="DateOnly"/></param>
        /// <param name="endDate">Period end <see cref="DateOnly"/></param>
        /// <returns>True if provided <see cref="DateOnly"/> belongs to period</returns>
        public static bool IsBelongToPeriod(this DateOnly date, DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                return date.IsBelongToPeriod(endDate, startDate);
            }

            return date >= startDate && date <= endDate;
        }

        /// <summary>
        /// Checks if <see cref="TimeOnly"/> belongs to period
        /// </summary>
        /// <param name="time"><see cref="TimeOnly"/> to check</param>
        /// <param name="startTime">Period start <see cref="TimeOnly"/></param>
        /// <param name="endTime">Period end <see cref="TimeOnly"/></param>
        /// <returns>True if provided <see cref="TimeOnly"/> belongs to period</returns>
        public static bool IsBelongToPeriod(this TimeOnly time, TimeOnly startTime, TimeOnly endTime)
        {
            if (startTime > endTime)
            {
                return time.IsBelongToPeriod(endTime, startTime);
            }

            return time >= startTime && time <= endTime;
        }

#endif

        /// <summary>
        /// Gets a first day of month of the provided <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>First day of month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets a first day of month of the provided <see cref="DateOnly"/>
        /// </summary>
        /// <param name="date">A <see cref="DateOnly"/> to process</param>
        /// <returns>First day of month</returns>
        public static DateOnly GetFirstDayOfMonth(this DateOnly date)
        {
            return new DateOnly(date.Year, date.Month, 1);
        }
#endif

        /// <summary>
        /// Gets a last day of month of the provided <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>Last day of month</returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets a last day of month of the provided <see cref="DateOnly"/>
        /// </summary>
        /// <param name="date">A <see cref="DateOnly"/> to process</param>
        /// <returns>Last day of month</returns>
        public static DateOnly GetLastDayOfMonth(this DateOnly date)
        {
            return date.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }
#endif

        /// <summary>
        /// Checks if <see cref="DateTime"/> is null or equals new <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/> to check</param>
        /// <returns>True if provided <see cref="DateTime"/> is null or equals new <see cref="DateTime"/></returns>
        public static bool IsNullOrNewDateTime(this DateTime? dateTime)
        {
            return dateTime == null || dateTime == new DateTime();
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Checks if <see cref="DateOnly"/> is null or equals new <see cref="DateOnly"/>
        /// </summary>
        /// <param name="date"><see cref="DateOnly"/> to check</param>
        /// <returns>True if provided <see cref="DateOnly"/> is null or equals new <see cref="DateOnly"/></returns>
        public static bool IsNullOrNewDateOnly(this DateOnly? date)
        {
            return date == null || date == new DateOnly();
        }

        /// <summary>
        /// Checks if <see cref="TimeOnly"/> is null or equals new <see cref="TimeOnly"/>
        /// </summary>
        /// <param name="time"><see cref="TimeOnly"/> to check</param>
        /// <returns>True if provided <see cref="TimeOnly"/> is null or equals new <see cref="TimeOnly"/></returns>
        public static bool IsNullOrNewTimeOnly(this TimeOnly? time)
        {
            return time == null || time == new TimeOnly();
        }
#endif

        /// <summary>
        /// Gets a quarter of the provided <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>Quarter</returns>
        public static int GetQuarter(this DateTime dateTime)
        {
            return (dateTime.Month + 2) / 3;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets a quarter of the provided <see cref="DateOnly"/>
        /// </summary>
        /// <param name="date">A <see cref="DateOnly"/> to process</param>
        /// <returns>Quarter</returns>
        public static int GetQuarter(this DateOnly date)
        {
            return (date.Month + 2) / 3;
        }
#endif

        /// <summary>
        /// Gets a financial quarter of the provided <see cref="DateTime"/>
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>Financial quarter</returns>
        public static int GetFinancialQuarter(this DateTime dateTime)
        {
            return (dateTime.AddMonths(3).Month + 2) / 3;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets a financial quarter of the provided <see cref="DateOnly"/>
        /// </summary>
        /// <param name="date">A <see cref="DateOnly"/> to process</param>
        /// <returns>Financial quarter</returns>
        public static int GetFinancialQuarter(this DateOnly date)
        {
            return (date.AddMonths(3).Month + 2) / 3;
        }
#endif

        /// <summary>
        /// Gets the maximum <see cref="DateTime"/> of provided value
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>Maximum <see cref="DateTime"/> of provided value</returns>
        public static DateTime GetMaxDateTimeValueOfDay(this DateTime dateTime)
        {
            return dateTime.Date.Add(DateTime.MaxValue.TimeOfDay);
        }

        /// <summary>
        /// Gets the first day of the week that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <returns>First day of the week that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek)
        {
            return GetFirstDayOfWeek(dayInWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the first day of the week that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>First day of the week that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetFirstDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            var firstDayCultureInWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var firstDayInWeek = dayInWeek.Date;

            while (firstDayInWeek.DayOfWeek != firstDayCultureInWeek)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets the first day of the week that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <returns>First day of the week that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetFirstDayOfWeek(this DateOnly dayInWeek)
        {
            return GetFirstDayOfWeek(dayInWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the first day of the week that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>First day of the week that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetFirstDayOfWeek(this DateOnly dayInWeek, CultureInfo cultureInfo)
        {
            var firstDayCultureInWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            var firstDayInWeek = dayInWeek;

            while (firstDayInWeek.DayOfWeek != firstDayCultureInWeek)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }
#endif

        /// <summary>
        /// Gets the last day of the week that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <returns>Last day of the week that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetLastDayOfWeek(this DateTime dayInWeek)
        {
            return GetLastDayOfWeek(dayInWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the last day of the week that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>Last day of the week that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetLastDayOfWeek(this DateTime dayInWeek, CultureInfo cultureInfo)
        {
            return GetFirstDayOfWeek(dayInWeek, cultureInfo).AddDays(6);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets the last day of the week that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <returns>Last day of the week that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetLastDayOfWeek(this DateOnly dayInWeek)
        {
            return GetLastDayOfWeek(dayInWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the last day of the week that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns>Last day of the week that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetLastDayOfWeek(this DateOnly dayInWeek, CultureInfo cultureInfo)
        {
            return GetFirstDayOfWeek(dayInWeek, cultureInfo).AddDays(6);
        }
#endif

        /// <summary>
        /// Gets the <see cref="DateTime"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <param name="dayOfWeek">Day of the week</param>
        /// <returns><see cref="DateTime"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetDateFromWeekByDayOfWeek(this DateTime dayInWeek, DayOfWeek dayOfWeek)
        {
            return GetDateFromWeekByDayOfWeek(dayInWeek, dayOfWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateTime"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateTime"/> to process</param>
        /// <param name="dayOfWeek">Day of the week</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns><see cref="DateTime"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateTime"/></returns>
        public static DateTime GetDateFromWeekByDayOfWeek(this DateTime dayInWeek, DayOfWeek dayOfWeek,
            CultureInfo cultureInfo)
        {
            var result = dayInWeek.GetFirstDayOfWeek(cultureInfo);

            while (result.DayOfWeek != dayOfWeek)
            {
                result = dayInWeek.AddDays(1);
            }

            return result;
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Gets the <see cref="DateOnly"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <param name="dayOfWeek">Day of the week</param>
        /// <returns><see cref="DateOnly"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetDateFromWeekByDayOfWeek(this DateOnly dayInWeek, DayOfWeek dayOfWeek)
        {
            return GetDateFromWeekByDayOfWeek(dayInWeek, dayOfWeek, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the <see cref="DateOnly"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateOnly"/> using <see cref="CultureInfo"/>
        /// </summary>
        /// <param name="dayInWeek">A <see cref="DateOnly"/> to process</param>
        /// <param name="dayOfWeek">Day of the week</param>
        /// <param name="cultureInfo">Culture info</param>
        /// <returns><see cref="DateOnly"/> that represents provided <see cref="DayOfWeek"/> that belongs to provided <see cref="DateOnly"/></returns>
        public static DateOnly GetDateFromWeekByDayOfWeek(this DateOnly dayInWeek, DayOfWeek dayOfWeek,
            CultureInfo cultureInfo)
        {
            var result = dayInWeek.GetFirstDayOfWeek(cultureInfo);

            while (result.DayOfWeek != dayOfWeek)
            {
                result = dayInWeek.AddDays(1);
            }

            return result;
        }
#endif

        /// <summary>
        /// Truncates provided <see cref="DateTime"/>
        /// </summary>
        /// <param name="date">A <see cref="DateTime"/> to process</param>
        /// <param name="ticks">Number of ticks</param>
        /// <returns>Truncated <see cref="DateTime"/></returns>
        public static DateTime Truncate(this DateTime date, long ticks = TimeSpan.TicksPerSecond)
        {
            return new DateTime(date.Ticks - date.Ticks % ticks, date.Kind);
        }

#if !NETSTANDARD2_0
        /// <summary>
        /// Converts <see cref="DateTime"/> to <see cref="DateOnly"/>
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> to process</param>
        /// <returns>A <see cref="DateOnly"/> instance that is set to the date part of the specified <see cref="DateTime"/></returns>
        public static DateOnly ToDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        /// <summary>
        /// Converts a nullable <see cref="DateTime"/> to the nullable <see cref="DateOnly"/>
        /// </summary>
        /// <param name="dateTime">A nullable <see cref="DateTime"/> to process</param>
        /// <returns>A nullable <see cref="DateOnly"/> instance that is set to the date part of the specified nullable <see cref="DateTime"/></returns>
        public static DateOnly? ToDateOnly(this DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return DateOnly.FromDateTime(dateTime.Value);
        }
#endif
    }
}
