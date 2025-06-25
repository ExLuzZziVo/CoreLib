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

            foreach (var ch in source)
            {
                sb.Append(TranslitDictionary.ContainsKey(ch.ToString())
                    ? TranslitDictionary[ch.ToString()]
                    : ch.ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Словарь кириллических букв и похожих на них ASCII символов
        /// </summary>
        private static readonly Dictionary<char, char> SimilarLettersDictionary = new Dictionary<char, char>
        {
            { 'а', 'a' },
            { 'е', 'e' },
            { 'ё', 'e' },
            { 'о', 'o' },
            { 'р', 'p' },
            { 'с', 'c' },
            { 'у', 'y' },
            { 'х', 'x' },
            { 'А', 'A' },
            { 'В', 'B' },
            { 'Е', 'E' },
            { 'Ё', 'E' },
            { 'З', '3' },
            { 'К', 'K' },
            { 'М', 'M' },
            { 'Н', 'H' },
            { 'О', 'O' },
            { 'Р', 'P' },
            { 'С', 'C' },
            { 'Т', 'T' },
            { 'Х', 'X' }
        };

        /// <summary>
        /// Заменяет кириллические буквы в строке на похожие на них ASCII символы
        /// </summary>
        /// <param name="source">Строка для замены кириллических букв</param>
        /// <returns>Строка, в которой кириллические буквы заменены на похожие на них ASCII символы</returns>
        public static string ToSimilarString(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            foreach (var ch in source)
            {
                sb.Append(SimilarLettersDictionary.TryGetValue(ch, out var value) ? value : ch);
            }

            return sb.ToString();
        }
    }
}
