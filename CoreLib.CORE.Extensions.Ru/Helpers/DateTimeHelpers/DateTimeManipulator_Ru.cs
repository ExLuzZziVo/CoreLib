#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeManipulator_Ru
    {
        /// <summary>
        /// Конвертирует индекс дня недели в сам день недели
        /// </summary>
        /// <param name="index">Индекс дня недели</param>
        /// <returns>День недели</returns>
        public static DayOfWeek GetDayOfWeekByIndex(int index)
        {
            if (index > 6 || index < 0)
            {
                return DayOfWeek.Monday;
            }

            if (index == 6)
            {
                return DayOfWeek.Sunday;
            }

            return (DayOfWeek)index + 1;
        }
    }
}