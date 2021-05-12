using System;

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

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM yyyy г."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM yyyy г."</returns>
        public static string ToFullLongDateString(this DateTime dateTime)
        {
            return $"{dateTime.ToString("dd MMMM yyyy")} г.";
        }

        /// <summary>
        /// Конвертирует временную составляющую даты в строку с форматом "HH час. mm мин."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Временная составляющая даты, представленная в виде строки с форматом "HH час. mm мин."</returns>
        public static string ToFullShortTimeString(this DateTime dateTime)
        {
            return $"{dateTime.Hour.ToString("00")} час. {dateTime.Minute.ToString("00")} мин.";
        }

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

        /// <summary>
        /// Конвертирует дату в строку с форматом "MM.yyyy"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MM.yyyy"</returns>
        public static string ToMonthYearShortString(this DateTime dateTime)
        {
            return dateTime.ToString("MM.yyyy");
        }

        /// <summary>
        /// Конвертирует дату в строку с форматом "MMMM yyyy г."
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "MMMM yyyy г."</returns>
        public static string ToMonthYearLongString(this DateTime dateTime)
        {
            return $"{dateTime.ToString("MMMM yyyy")} г.";
        }

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd.MM"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd.MM"</returns>
        public static string ToDayMonthShortString(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM");
        }

        /// <summary>
        /// Конвертирует дату в строку с форматом "dd MMMM"
        /// </summary>
        /// <param name="dateTime">Конвертируемая дата</param>
        /// <returns>Дата, представленная в виде строки с форматом "dd MMMM"</returns>
        public static string ToDayMonthLongString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMMM");
        }
    }
}