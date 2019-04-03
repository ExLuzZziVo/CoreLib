#region

using System;
using NameCaseLib;
using NameCaseLib.NCL;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers.Generators
{
    public static class NameCasesGenerator
    {
        private static readonly Ru NameCaseLibInstance = new Ru();

        public static string[] GenerateFullNameCases(string surname,
            string name, string patronymic, bool gender)
        {
            return GenerateFullNameCases(surname, name, patronymic, gender ? Gender.Man : Gender.Woman);
        }

        public static string[] GenerateFullNameCases(string surname,
            string name, string patronymic, Gender gender)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
                throw new NullReferenceException("Surname and Name are required!");

            surname = surname.ToUpperAllFirstChars();
            name = name.ToUpperAllFirstChars();
            patronymic = patronymic.ToUpperAllFirstChars();

            var result = new string[6];

            result[0] =
                $"{surname} {name} {patronymic}"
                    .TrimWholeString();
            result[1] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[1]} {NameCaseLibInstance.QFirstName(name, gender)[1]} {GeneratePatronymic(patronymic, 1, gender)}"
                    .TrimWholeString();
            result[2] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[2]} {NameCaseLibInstance.QFirstName(name, gender)[2]} {GeneratePatronymic(patronymic, 2, gender)}"
                    .TrimWholeString();
            result[3] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[3]} {NameCaseLibInstance.QFirstName(name, gender)[3]} {GeneratePatronymic(patronymic, 3, gender)}"
                    .TrimWholeString();
            result[4] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[4]} {NameCaseLibInstance.QFirstName(name, gender)[4]} {GeneratePatronymic(patronymic, 4, gender)}"
                    .TrimWholeString();
            result[5] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[5]} {NameCaseLibInstance.QFirstName(name, gender)[5]} {GeneratePatronymic(patronymic, 5, gender)}"
                    .TrimWholeString();

            return result;
        }

        public static string[] GenerateShortNameCases(string surname,
            string name, string patronymic, bool gender)
        {
            return GenerateShortNameCases(surname, name, patronymic, gender ? Gender.Man : Gender.Woman);
        }

        public static string[] GenerateShortNameCases(string surname,
            string name, string patronymic, Gender gender)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
                throw new NullReferenceException("Surname and Name are required!");

            surname =
                surname.ToUpperAllFirstChars();

            var result = new string[6];

            var shortNameInitials =
                $"{name.Substring(0, 1).ToUpper()}.{(patronymic.IsNullOrEmptyOrWhiteSpace() ? "" : $" {patronymic.Substring(0, 1).ToUpper()}.")}";

            result[0] = $"{surname} {shortNameInitials}".TrimWholeString();
            result[1] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[1]} {shortNameInitials}"
                    .TrimWholeString();
            result[2] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[2]} {shortNameInitials}"
                    .TrimWholeString();
            result[3] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[3]} {shortNameInitials}"
                    .TrimWholeString();
            result[4] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[4]} {shortNameInitials}"
                    .TrimWholeString();
            result[5] =
                $"{NameCaseLibInstance.QSecondName(surname, gender)[5]} {shortNameInitials}"
                    .TrimWholeString();

            return result;
        }

        public static string GenerateShortName(string surname, string name, string patronymic)
        {
            if (surname.IsNullOrEmptyOrWhiteSpace() || name.IsNullOrEmptyOrWhiteSpace())
                throw new NullReferenceException("Surname and Name are required!");
            return
                $"{surname.ToUpperAllFirstChars()} {name.Substring(0, 1).ToUpper()}.{(patronymic.IsNullOrEmptyOrWhiteSpace() ? "" : $" {patronymic.Substring(0, 1).ToUpper()}.")}"
                    .TrimWholeString();
        }

        public static string GenerateShortName(string fullName)
        {
            if (fullName.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            var strArray = fullName.FormatText().Split(' ');
            if (strArray.Length < 2)
                throw new ArgumentNullException(nameof(fullName), "Surname and Name are required!");
            if (strArray.Length == 2)
                return
                    $"{strArray[0].ToUpperAllFirstChars()} {strArray[1].Substring(0, 1).ToUpper()}.".TrimWholeString();
            return
                $"{strArray[0].ToUpperAllFirstChars()} {strArray[1].Substring(0, 1).ToUpper()}. {strArray[2].Substring(0, 1).ToUpper()}."
                    .TrimWholeString();
        }

        private static string GeneratePatronymic(string patronymic, int padej, Gender gender)
        {
            if (patronymic.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            var strArray = patronymic.Split(' ');
            if (strArray.Length == 2)
                switch (strArray[1])
                {
                    case "Оглы":
                    case "Огли":
                    case "Угли":
                    case "Углы":
                        return
                            $"{NameCaseLibInstance.QFirstName(strArray[0], Gender.Man)[padej]} {strArray[1]}";
                    case "Кызы":
                    case "Кизы":
                    case "Кызи":
                    case "Кизи":
                        return
                            $"{NameCaseLibInstance.QFirstName(strArray[0], Gender.Woman)[padej]} {strArray[1]}";
                    default:
                        return patronymic;
                }
            return
                NameCaseLibInstance.QFatherName(strArray[0], gender)[padej];
        }
    }
}