#region

using System.Collections.Generic;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringManipulator
    {
        /// <summary>
        /// Punctuation dictionary for text formatting
        /// </summary>
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

        /// <summary>
        /// Formats specified string using <see cref="PunctuationDictionary"/> and <see cref="StringExtensions.TrimWholeString"/>
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>Formatted string</returns>
        public static string FormatText(this string source)
        {
            foreach (var e in PunctuationDictionary)
            {
                source = source.Replace(e.Key, e.Value);
            }

            return source.TrimWholeString();
        }
    }
}