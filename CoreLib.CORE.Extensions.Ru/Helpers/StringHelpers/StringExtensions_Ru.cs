#region

using System.Collections.Generic;
using System.Text;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringExtensions_Ru
    {
        /// <summary>
        /// Словарь транслитерации
        /// </summary>
        private static readonly Dictionary<string, string> TranslitDictionary = new Dictionary<string, string>
        {
            { "а", "a" },
            { "б", "b" },
            { "в", "v" },
            { "г", "g" },
            { "д", "d" },
            { "е", "e" },
            { "ё", "yo" },
            { "ж", "zh" },
            { "з", "z" },
            { "и", "i" },
            { "й", "i" },
            { "к", "k" },
            { "л", "l" },
            { "м", "m" },
            { "н", "n" },
            { "о", "o" },
            { "п", "p" },
            { "р", "r" },
            { "с", "s" },
            { "т", "t" },
            { "у", "u" },
            { "ф", "f" },
            { "х", "kh" },
            { "ц", "tc" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "shch" },
            { "ъ", "ie" },
            { "ы", "y" },
            { "ь", "′" },
            { "э", "e" },
            { "ю", "yu" },
            { "я", "ya" },
            { "А", "A" },
            { "Б", "B" },
            { "В", "V" },
            { "Г", "G" },
            { "Д", "D" },
            { "Е", "E" },
            { "Ё", "Yo" },
            { "Ж", "Zh" },
            { "З", "Z" },
            { "И", "I" },
            { "Й", "I" },
            { "К", "K" },
            { "Л", "L" },
            { "М", "M" },
            { "Н", "N" },
            { "О", "O" },
            { "П", "P" },
            { "Р", "R" },
            { "С", "S" },
            { "Т", "T" },
            { "У", "U" },
            { "Ф", "F" },
            { "Х", "Kh" },
            { "Ц", "Tc" },
            { "Ч", "Ch" },
            { "Ш", "Sh" },
            { "Щ", "Shch" },
            { "Ъ", "Ie" },
            { "Ы", "Y" },
            { "Ь", "′" },
            { "Э", "E" },
            { "Ю", "Yu" },
            { "Я", "Ya" }
        };

        /// <summary>
        /// Транслитерирует строку
        /// </summary>
        /// <param name="source">Строка для транслитерации</param>
        /// <returns>Строка в транслитерации</returns>
        public static string ToTranslit(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var t in source)
            {
                sb.Append(TranslitDictionary.ContainsKey(t.ToString())
                    ? TranslitDictionary[t.ToString()]
                    : t.ToString());
            }

            return sb.ToString();
        }
    }
}