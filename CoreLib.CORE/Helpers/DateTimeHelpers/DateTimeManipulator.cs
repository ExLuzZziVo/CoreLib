#region

using System;

#endregion

namespace UIServiceLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeManipulator
    {
        public static int MonthsInPeriodCount(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate > secondDate)
                return MonthsInPeriodCount(secondDate, firstDate);
            var months = Math.Abs(secondDate.Year * 12 + (secondDate.Month - 1) -
                                  (firstDate.Year * 12 + (firstDate.Month - 1)));
            if (firstDate.AddMonths(months) > secondDate || secondDate.Day < firstDate.Day)
                return months - 1;
            return months;
        }

        public static int CalculateAge(DateTime birthDate, DateTime checkDate)
        {
            var age = checkDate.Year -
                      birthDate.Year;
            if (birthDate >
                checkDate.AddYears(-age))
                age--;
            return age;
        }
    }
}