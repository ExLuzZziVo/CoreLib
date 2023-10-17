#region

using System;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class ShortNameGenerators
    {
        /// <summary>
        /// Сокращает ФИО
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <returns>Сокращенное ФИО</returns>
        public static string GenerateShortName(string surname, string name, string patronymic)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
            {
                throw new NullReferenceException("Surname and Name are required!");
            }

            return
                $"{surname.ToUpperAllFirstChars()} {name.Substring(0, 1).ToUpper()}.{(patronymic.IsNullOrEmptyOrWhiteSpace() ? "" : $" {patronymic.Substring(0, 1).ToUpper()}.")}"
                    .TrimWholeString();
        }

        /// <summary>
        /// Сокращает ФИО
        /// </summary>
        /// <param name="fullName">ФИО полностью</param>
        /// <returns>Сокращенное ФИО</returns>
        public static string GenerateShortName(string fullName)
        {
            if (fullName.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var strArray = fullName.FormatText().Split(' ');

            if (strArray.Length < 2)
            {
                throw new ArgumentNullException(nameof(fullName), "Surname and Name are required!");
            }

            if (strArray.Length == 2)
            {
                return
                    $"{strArray[0].ToUpperAllFirstChars()} {strArray[1].Substring(0, 1).ToUpper()}.".TrimWholeString();
            }

            return
                $"{strArray[0].ToUpperAllFirstChars()} {strArray[1].Substring(0, 1).ToUpper()}. {strArray[2].Substring(0, 1).ToUpper()}."
                    .TrimWholeString();
        }
    }
}