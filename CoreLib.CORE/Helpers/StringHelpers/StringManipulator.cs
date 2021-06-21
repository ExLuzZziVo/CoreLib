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
            {":", ": "},
            {";", "; "},
            {"№", "№ "},
            {" %", "%"},
            {"%", "% "},
            {". ,", ".,"},
            {" /", "/"},
            {"/ ", "/"},
            {" \\", "\\"},
            {"\\ ", "\\"},
            {"\"", "\" "},
            {"- ", "-"},
            {" -", "-"},
            {"« ", "«"},
            {" »", "»"},
            {" '", "'"},
            {"' ", "'"},
            {" \"", "\""},
            {"\" ", "\""},
            
            {".", ". "},
            {" .", "."},
            {",", ", "},
            {" ,", ","},
            {"!", "! "},
            {" !", "!"},
            {"?", "? "},
            {" ?", "?"},
        };

        /// <summary>
        /// Formats specified string using <see cref="PunctuationDictionary"/> and <see cref="StringExtensions.TrimWholeString"/>
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>Formatted string</returns>
        public static string FormatText(this string source)
        {
            var result = source.TrimWholeString();

            foreach (var e in PunctuationDictionary)
            {
                result = result.Replace(e.Key, e.Value);
            }

            return result.TrimWholeString();
        }
    }
}