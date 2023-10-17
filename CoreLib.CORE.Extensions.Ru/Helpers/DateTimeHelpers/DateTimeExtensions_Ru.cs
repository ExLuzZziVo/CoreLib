#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.DateTimeHelpers
{
    public static class DateTimeExtensions_Ru
    {
        /// <summary>
        /// Конвертирует дату в строку с форматом "dd.MM.yyyy г."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd.MM.yyyy г."</returns>
        public static string ToMiddleDateString(this DateTime dateTime)
        {
            return $"{dateTime.ToString("dd.MM.yyyy")} г.";
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "dd.MM.yyyy г."
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd.MM.yyyy г."</returns>
        public static string ToMiddleDateString(this DateOnly date)
        {
            return $"{date.ToString("dd.MM.yyyy")} г.";
        }
#endif

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM yyyy г."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM yyyy г."</returns>
        public static string ToFullLongDateString(this DateTime dateTime)
        {
            return $"{dateTime.ToString("dd MMMM yyyy")} г.";
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM yyyy г."
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM yyyy г."</returns>
        public static string ToFullLongDateString(this DateOnly date)
        {
            return $"{date.ToString("dd MMMM yyyy")} г.";
        }
#endif

        /// <summary>
        /// Конвертирует временную составляющую даты в строку с форматом "HH час. mm мин."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Временная составляющая даты, представленная в виде строки с форматом "HH час. mm мин."</returns>
        public static string ToFullShortTimeString(this DateTime dateTime)
        {
            return $"{dateTime.Hour.ToString("00")} час. {dateTime.Minute.ToString("00")} мин.";
        }

#if NET6_0_OR_GREATER

        /// <summary>
        /// Конвертирует время в строку с форматом "HH час. mm мин."
        /// </summary>
        /// <param name="time">Конвертируемое время</param>
        /// <returns>Время, представленное в виде строки с форматом "HH час. mm мин."</returns>
        public static string ToFullShortTimeString(this TimeOnly time)
        {
            return $"{time.Hour.ToString("00")} час. {time.Minute.ToString("00")} мин.";
        }

#endif

        /// <summary>
        /// Конвертирует временную составляющую даты в строку с форматом "HH час. mm мин. ss сек."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Временная составляющая даты, представленная в виде строки с форматом "HH час. mm мин. ss сек."</returns>
        public static string ToFullLongTimeString(this DateTime dateTime)
        {
            return
                $"{dateTime.Hour.ToString("00")} час. {dateTime.Minute.ToString("00")} мин. {dateTime.Second.ToString("00")} сек.";
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует время в строку с форматом "HH час. mm мин. ss сек."
        /// </summary>
        /// <param name="time">Конвертируемое время</param>
        /// <returns>Время, представленное в виде строки с форматом "HH час. mm мин. ss сек."</returns>
        public static string ToFullLongTimeString(this TimeOnly time)
        {
            return
                $"{time.Hour.ToString("00")} час. {time.Minute.ToString("00")} мин. {time.Second.ToString("00")} сек.";
        }
#endif

        /// <summary>
        /// Конвертирует дату в строку с форматом "MM.yyyy"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MM.yyyy"</returns>
        public static string ToMonthYearShortString(this DateTime dateTime)
        {
            return dateTime.ToString("MM.yyyy");
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "MM.yyyy"
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MM.yyyy"</returns>
        public static string ToMonthYearShortString(this DateOnly date)
        {
            return date.ToString("MM.yyyy");
        }
#endif

        /// <summary>
        /// Конвертирует дату в строку с форматом "MMMM yyyy г."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MMMM yyyy г."</returns>
        public static string ToMonthYearLongString(this DateTime dateTime)
        {
            return $"{dateTime.ToString("MMMM yyyy")} г.";
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "MMMM yyyy г."
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MMMM yyyy г."</returns>
        public static string ToMonthYearLongString(this DateOnly date)
        {
            return $"{date.ToString("MMMM yyyy")} г.";
        }
#endif

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd.MM"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd.MM"</returns>
        public static string ToDayMonthShortString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM");
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "dd.MM"
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd.MM"</returns>
        public static string ToDayMonthShortString(this DateOnly date)
        {
            return date.ToString("dd.MM");
        }
#endif

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM"</returns>
        public static string ToDayMonthLongString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM");
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM"
        /// </summary>
        /// <param name="date">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM"</returns>
        public static string ToDayMonthLongString(this DateOnly date)
        {
            return date.ToString("dd MMMM");
        }
#endif
    }
}