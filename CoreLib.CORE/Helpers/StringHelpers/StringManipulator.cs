using System.Collections.Generic;
using System.Globalization;

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringManipulator
    {
        public static readonly NumberFormatInfo MoneyCultureStringFormatter =
            new NumberFormatInfo { NumberDecimalDigits = 2 };

        private static readonly Dictionary<string, string> PunctuationDictionary = new Dictionary<string, string>
        {
            {".", ". "},
            {",", ", "},
            {":", ": "},
            {";", "; "},
            {"№", "№ "},
            {". ,", ".,"},
            {" /", "/"},
            {"/ ", "/"},
            {" \\", "\\"},
            {"\\ ", "\\"},
            {"\"", "\" "},
            {"- ", "-"},
            {" -", "-"}
        };

        public static string FormatText(this string source)
        {
            foreach (var e in PunctuationDictionary) source = source.Replace(e.Key, e.Value);

            return source.TrimWholeString();
        }
    }
}
