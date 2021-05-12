#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeManipulator
    {
        /// <summary>
        /// Gets number of months in the specified period
        /// </summary>
        /// <param name="firstDate">Period start <see cref="DateTime"/></param>
        /// <param name="secondDate">Period end <see cref="DateTime"/></param>
        /// <returns>Number of months in the specified period</returns>
        public static int MonthsInPeriodCount(DateTime firstDate, DateTime secondDate)
        {
            if (firstDate > secondDate)
            {
                return MonthsInPeriodCount(secondDate, firstDate);
            }

            var months = Math.Abs(secondDate.Year * 12 + (secondDate.Month - 1) -
                                  (firstDate.Year * 12 + (firstDate.Month - 1)));

            if (firstDate.AddMonths(months) > secondDate || secondDate.Day < firstDate.Day)
            {
                return months - 1;
            }

            return months;
        }

        /// <summary>
        /// Calculates the age using provided birth date and <see cref="DateTime"/>
        /// </summary>
        /// <param name="birthDate">Birth date</param>
        /// <param name="checkDate">The date on which the age is calculated</param>
        /// <returns>Calculated age</returns>
        public static int CalculateAge(DateTime birthDate, DateTime checkDate)
        {
            if (checkDate < birthDate)
            {
                throw new ArgumentOutOfRangeException(nameof(checkDate));
            }

            var age = checkDate.Year -
                      birthDate.Year;

            if (birthDate >
                checkDate.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}