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
        /// <param name="gender">Пол. True - мужской, False - женский</param>
        /// <returns>ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateFullNameCases(string surname,
            string name, string patronymic, bool gender)
        {
            return GenerateFullNameCases(surname, name, patronymic, gender ? Gender.Man : Gender.Woman);
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

            result[0] =
                $"{surname} {name} {patronymic}"
                    .TrimWholeString();

            result[1] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[1]} {nameCaseLibInstance.QFirstName(name, gender)[1]} {nameCaseLibInstance.GeneratePatronymic(patronymic, 1, gender)}"
                    .TrimWholeString();

            result[2] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[2]} {nameCaseLibInstance.QFirstName(name, gender)[2]} {nameCaseLibInstance.GeneratePatronymic(patronymic, 2, gender)}"
                    .TrimWholeString();

            result[3] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[3]} {nameCaseLibInstance.QFirstName(name, gender)[3]} {nameCaseLibInstance.GeneratePatronymic(patronymic, 3, gender)}"
                    .TrimWholeString();

            result[4] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[4]} {nameCaseLibInstance.QFirstName(name, gender)[4]} {nameCaseLibInstance.GeneratePatronymic(patronymic, 4, gender)}"
                    .TrimWholeString();

            result[5] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[5]} {nameCaseLibInstance.QFirstName(name, gender)[5]} {nameCaseLibInstance.GeneratePatronymic(patronymic, 5, gender)}"
                    .TrimWholeString();

            return result;
        }

        /// <summary>
        /// Склоняет ФИО по всем падежам и сокращает его
        /// </summary>
        /// <param name="surname">Фамилия</param>
        /// <param name="name">Имя</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="gender">Пол. True - мужской, False - женский</param>
        /// <returns>Сокращенное ФИО, склоненное по всем падежам в виде массива строк с размерностью 6 (кол-во падежей)</returns>
        public static string[] GenerateShortNameCases(string surname,
            string name, string patronymic, bool gender)
        {
            return GenerateShortNameCases(surname, name, patronymic, gender ? Gender.Man : Gender.Woman);
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

            result[0] = $"{surname} {shortNameInitials}".TrimWholeString();

            result[1] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[1]} {shortNameInitials}"
                    .TrimWholeString();

            result[2] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[2]} {shortNameInitials}"
                    .TrimWholeString();

            result[3] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[3]} {shortNameInitials}"
                    .TrimWholeString();

            result[4] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[4]} {shortNameInitials}"
                    .TrimWholeString();

            result[5] =
                $"{nameCaseLibInstance.QSecondName(surname, gender)[5]} {shortNameInitials}"
                    .TrimWholeString();

            return result;
        }

        /// <summary>
        /// Склоняет отчество
        /// </summary>
        /// <param name="nameCaseLibInstance">Экземпляр класса, склоняющего ФИО</param>
        /// <param name="patronymic">Отчество</param>
        /// <param name="padej">Индекс падежа</param>
        /// <param name="gender">Пол</param>
        /// <returns>Склоненное отчество</returns>
        private static string GeneratePatronymic(this NameCaseLib.Ru nameCaseLibInstance, string patronymic, int padej, Gender gender)
        {
            if (patronymic.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var strArray = patronymic.Split(' ');

            if (strArray.Length == 2)
            {
                switch (strArray[1])
                {
                    case "Оглы":
                    case "Огли":
                    case "Угли":
                    case "Углы":
                        return
                            $"{nameCaseLibInstance.QFirstName(strArray[0], Gender.Man)[padej]} {strArray[1]}";
                    case "Кызы":
                    case "Кизы":
                    case "Кызи":
                    case "Кизи":
                        return
                            $"{nameCaseLibInstance.QFirstName(strArray[0], Gender.Woman)[padej]} {strArray[1]}";
                    default:
                        return patronymic;
                }
            }

            return
                nameCaseLibInstance.QFatherName(strArray[0], gender)[padej];
        }
    }
}