using System.Collections.Generic;

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringManipulator
    {
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
