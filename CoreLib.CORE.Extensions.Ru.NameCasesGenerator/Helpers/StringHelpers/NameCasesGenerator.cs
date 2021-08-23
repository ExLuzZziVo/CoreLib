#region

using System;
using NameCaseLib.NCL;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class NameCasesGenerator
    {
        /// <summary>
        /// Склоняет ФИО по всем падежам
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="gender">Пол. True - мужской, False - женский, Null - автоматическое определение. По умолчанию: Null</param>
        /// <returns>ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateFullNameCases(string surname,
            string name, string patronymic, bool? gender = null)
        {
            return GenerateFullNameCases(surname, name, patronymic,
                gender == null ? Gender.Null : gender.Value ? Gender.Man : Gender.Woman);
        }

        /// <summary>
        /// Склоняет ФИО по всем падежам
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="gender">Пол</param>
        /// <returns>ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateFullNameCases(string surname,
            string name, string patronymic, Gender gender)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
            {
                throw new NullReferenceException("Surname and Name are required!");
            }

            surname = surname.ToUpperAllFirstChars();
            name = name.ToUpperAllFirstChars();
            patronymic = patronymic.ToUpperAllFirstChars();

            var nameCaseLibInstance = new NameCaseLib.Ru();
            var result = new string[6];

            if (gender == Gender.Null)
            {
                gender = nameCaseLibInstance.DetectGender($"{surname} {name} {patronymic}".TrimWholeString());
            }

            var surnames = nameCaseLibInstance.QSurname(surname, gender);
            var names = nameCaseLibInstance.QName(name, gender);
            var patronymics = nameCaseLibInstance.QFatherName(patronymic, gender);

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = $"{surnames[i]} {names[i]} {patronymics[i]}"
                    .TrimWholeString();
            }

            nameCaseLibInstance.FullReset();

            return result;
        }

        /// <summary>
        /// Склоняет ФИО по всем падежам и сокращает его
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="gender">Пол. True - мужской, False - женский, Null - автоматическое определение. По умолчанию: Null</param>
        /// <returns>Сокращенное ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateShortNameCases(string surname,
            string name, string patronymic, bool? gender = null)
        {
            return GenerateShortNameCases(surname, name, patronymic,
                gender == null ? Gender.Null : gender.Value ? Gender.Man : Gender.Woman);
        }

        /// <summary>
        /// Склоняет ФИО по всем падежам и сокращает его
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="gender">Пол</param>
        /// <returns>Сокращенное ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateShortNameCases(string surname,
            string name, string patronymic, Gender gender)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
            {
                throw new NullReferenceException("Surname and Name are required!");
            }

            surname =
                surname.ToUpperAllFirstChars();

            var nameCaseLibInstance = new NameCaseLib.Ru();
            var result = new string[6];

            var shortNameInitials =
                $"{name.Substring(0, 1).ToUpper()}.{(patronymic.IsNullOrEmptyOrWhiteSpace() ? "" : $" {patronymic.Substring(0, 1).ToUpper()}.")}";

            var surnames = nameCaseLibInstance.QSurname(surname, gender);

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = $"{surnames[i]} {shortNameInitials}"
                    .TrimWholeString();
            }

            nameCaseLibInstance.FullReset();

            return result;
        }
    }
}