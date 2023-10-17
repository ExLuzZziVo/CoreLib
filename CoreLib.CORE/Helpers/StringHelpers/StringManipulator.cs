#region

using System.Collections.Generic;
using System.Linq;
using CoreLib.CORE.Helpers.IntHelpers;

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
            { ":", ": " },
            { ";", "; " },
            { "№", "№ " },
            { " %", "%" },
            { "%", "% " },
            { " /", "/" },
            { "/ ", "/" },
            { " \\", "\\" },
            { "\\ ", "\\" },
            { "- ", "-" },
            { " -", "-" },
            { "« ", "«" },
            { " »", "»" },
            { "[ ", "[" },
            { " ]", "]" },
            { "( ", "(" },
            { " )", ")" },
            { "{ ", "{" },
            { " }", "}" },
            { "< ", "<" },
            { " >", ">" },

            { ".", ". " },
            { " .", "." },
            { ",", ", " },
            { " ,", "," },
            { "!", "! " },
            { " !", "!" },
            { "?", "? " },
            { " ?", "?" },

            { ". ,", ".," },
            { ",,", "," }
        };

        /// <summary>
        /// Formats specified string using <see cref="PunctuationDictionary"/> and <see cref="StringExtensions.TrimWholeString"/>
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>Formatted string</returns>
        public static string FormatText(this string source)
        {
            var result = source.TrimWholeString();

            // Removes unneeded quotes (", ', `) from source string
            string processQuotes(string str, char quote)
            {
                var strArray = str.Split(quote);

                strArray = strArray.Where((item, index) =>
                    !item.IsNullOrEmptyOrWhiteSpace() || index == 0 || index == strArray.Length - 1).ToArray();

                if (strArray.Length != 2)
                {
                    for (var i = 0; i < strArray.Length; i++)
                    {
                        if (!i.IsEven())
                        {
                            strArray[i] = quote + strArray[i].Trim() + quote;
                        }
                    }
                }

                return string.Join(" ", strArray);
            }

            // Removes special symbols without its pair ( [], (), {}, <>, «» ). Symbol pairs must be processed before the PunctuationDictionary is used to add or remove extra spaces
            string processSymbolPairs(string str, char fCh, char sCh)
            {
                var res = string.Empty;

                var openedPairs = new List<int>(str.Length);

                foreach (var ch in str)
                {
                    if (ch == sCh)
                    {
                        if (openedPairs.Count == 0)
                        {
                            continue;
                        }

                        openedPairs.RemoveAt(openedPairs.Count - 1);
                    }
                    else if (ch == fCh)
                    {
                        openedPairs.Add(res.Length);
                    }

                    res += ch;
                }

                foreach (var p in openedPairs)
                {
                    res = res.Remove(p, 1);
                }

                return res.TrimEnd(fCh);
            }

            if (result.Contains("«") || result.Contains("»"))
            {
                result = processSymbolPairs(result, '«', '»');
            }

            if (result.Contains("<") || result.Contains(">"))
            {
                result = processSymbolPairs(result, '«', '»');
            }

            if (result.Contains("(") || result.Contains(")"))
            {
                result = processSymbolPairs(result, '(', ')');
            }

            if (result.Contains("[") || result.Contains("]"))
            {
                result = processSymbolPairs(result, '[', ']');
            }

            if (result.Contains("{") || result.Contains("}"))
            {
                result = processSymbolPairs(result, '{', '}');
            }

            foreach (var e in PunctuationDictionary)
            {
                result = result.Replace(e.Key, e.Value);
            }

            if (result.Contains("\""))
            {
                result = processQuotes(result, '"');
            }

            if (result.Contains("'"))
            {
                result = processQuotes(result, '\'');
            }

            if (result.Contains("`"))
            {
                result = processQuotes(result, '`');
            }

            return result.TrimWholeString();
        }
    }
}