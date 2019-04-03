#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace UIServiceLib.CORE.Helpers.StringHelpers
{
    public static class StringExtensions
    {
        private static readonly Dictionary<string, string> TranslitDictionary = new Dictionary<string, string>
        {
            {"а", "a"},
            {"б", "b"},
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "e"},
            {"ё", "yo"},
            {"ж", "zh"},
            {"з", "z"},
            {"и", "i"},
            {"й", "i"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "s"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "kh"},
            {"ц", "tc"},
            {"ч", "ch"},
            {"ш", "sh"},
            {"щ", "sch"},
            {"ъ", "ie"},
            {"ы", "i"},
            {"ь", "j"},
            {"э", "e"},
            {"ю", "yu"},
            {"я", "ya"},
            {"А", "A"},
            {"Б", "B"},
            {"В", "V"},
            {"Г", "G"},
            {"Д", "D"},
            {"Е", "E"},
            {"Ё", "Yo"},
            {"Ж", "Zh"},
            {"З", "Z"},
            {"И", "I"},
            {"Й", "I"},
            {"К", "K"},
            {"Л", "L"},
            {"М", "M"},
            {"Н", "N"},
            {"О", "O"},
            {"П", "P"},
            {"Р", "R"},
            {"С", "S"},
            {"Т", "T"},
            {"У", "U"},
            {"Ф", "F"},
            {"Х", "Kh"},
            {"Ц", "Tc"},
            {"Ч", "Ch"},
            {"Ш", "Sh"},
            {"Щ", "Sch"},
            {"Ъ", "Ie"},
            {"Ы", "I"},
            {"Ь", "J"},
            {"Э", "E"},
            {"Ю", "Yu"},
            {"Я", "Ya"}
        };

        public static string TrimWholeString(this string source)
        {
            return source.IsNullOrEmptyOrWhiteSpace() ? string.Empty : Regex.Replace(source, @"\s+", " ").Trim();
        }

        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source?.IndexOf(value, comp) >= 0;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string source)
        {
            return string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source);
        }

        public static string ToTranslit(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            var sb = new StringBuilder();
            foreach (var t in source)
                sb.Append(TranslitDictionary.ContainsKey(t.ToString())
                    ? TranslitDictionary[t.ToString()]
                    : t.ToString());
            return sb.ToString();
        }

        public static string ToUpperAllFirstChars(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            source = source.TrimWholeString();
            var sb = new StringBuilder(source.ToLower());
            var symbols = new[] {' ', '-'};
            if (sb.Length > 0 && char.IsLetter(sb[0]))
                sb[0] = char.ToUpper(sb[0]);
            for (var i = 1; i < sb.Length; i++)
            {
                var ch = sb[i];
                if (symbols.Contains(sb[i - 1]) && char.IsLetter(ch))
                    sb[i] = char.ToUpper(ch);
            }

            return sb.ToString();
        }

        public static string ToUpperFirstChar(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
                return string.Empty;
            return str.First().ToString().ToUpper() + str.Substring(1);
        }
    }
}