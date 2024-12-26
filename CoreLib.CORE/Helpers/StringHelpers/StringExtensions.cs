#region

using System;
using System.Globalization;
using System.Linq;
using System.Text;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes all leading, trailing and repeating occurrences of \n, \t and space characters from the provided string
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>Returns a new string in which all leading, trailing and repeating occurrences of \n, \t and space characters are removed</returns>
        public static string TrimWholeString(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var src = source.ToCharArray();
            var wasLastCharWhiteSpace = false;
            var wasLastCharNewLine = false;
            var wasLastCharTab = false;
            var resultLength = 0;

            for (var i = 0; i < source.Length; i++)
            {
                var ch = src[i];

                if (ch == '\u0020')
                {
                    if (resultLength > 0)
                    {
                        if (!(wasLastCharWhiteSpace || wasLastCharNewLine || wasLastCharTab))
                        {
                            src[resultLength++] = ch;
                            wasLastCharWhiteSpace = true;
                        }
                    }
                }
                else if (ch == '\n')
                {
                    if (resultLength > 0)
                    {
                        if (wasLastCharWhiteSpace || wasLastCharTab)
                        {
                            src[resultLength - 1] = ch;
                        }
                        else
                        {
                            src[resultLength++] = ch;
                        }

                        wasLastCharWhiteSpace = false;
                        wasLastCharNewLine = true;
                        wasLastCharTab = false;
                    }
                }
                else if (ch == '\t')
                {
                    if (wasLastCharWhiteSpace)
                    {
                        src[resultLength - 1] = ch;
                    }
                    else
                    {
                        src[resultLength++] = ch;
                    }

                    wasLastCharWhiteSpace = false;
                    wasLastCharNewLine = false;
                    wasLastCharTab = true;
                }
                else
                {
                    src[resultLength++] = ch;

                    wasLastCharWhiteSpace = false;
                    wasLastCharNewLine = false;
                    wasLastCharTab = false;
                }
            }

            return new string(src, 0, resultLength).TrimEnd('\u0020', '\n', '\t');
        }

        /// <summary>
        /// Checks if provided string contains specified substring using <see cref="StringComparison"/>
        /// </summary>
        /// <param name="source">Target string</param>
        /// <param name="value">The string to seek</param>
        /// <param name="comp">String comparison type</param>
        /// <returns>True if provided string contains specified substring</returns>
        public static bool Contains(this string source, string value, StringComparison comp)
        {
            return source?.IndexOf(value, comp) >= 0;
        }

        /// <summary>
        /// Checks if provided string is null, empty or white space
        /// </summary>
        /// <param name="source">A string to check</param>
        /// <returns>True if provided string is null, empty or white space</returns>
        public static bool IsNullOrEmptyOrWhiteSpace(this string source)
        {
            return string.IsNullOrEmpty(source) || string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Capitalizes all first characters of words in the specified string
        /// </summary>
        /// <param name="source">Target string</param>
        /// <returns>Returns a new string in which all first characters of words are capitalized</returns>
        public static string ToUpperAllFirstChars(this string source)
        {
            if (source.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            source = source.TrimWholeString();
            var sb = new StringBuilder(source.ToLower());
            var symbols = new[] { ' ', '-' };

            if (sb.Length > 0 && char.IsLetter(sb[0]))
            {
                sb[0] = char.ToUpper(sb[0]);
            }

            for (var i = 1; i < sb.Length; i++)
            {
                var ch = sb[i];

                if (symbols.Contains(sb[i - 1]) && char.IsLetter(ch))
                {
                    sb[i] = char.ToUpper(ch);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Capitalizes first character in the specified string
        /// </summary>
        /// <param name="str">Target string</param>
        /// <returns>Returns a new string in which first character is capitalized</returns>
        public static string ToUpperFirstChar(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            return char.ToUpperInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Makes the first character in the specified string lowercase
        /// </summary>
        /// <param name="str">Target string</param>
        /// <returns>Returns a new string in which the first character is lowercase</returns>
        public static string ToLowerFirstChar(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            return char.ToLowerInvariant(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// Gets a substring of specified length from the beginning of the provided string
        /// </summary>
        /// <param name="str">The string to return substring from</param>
        /// <param name="count">The number of starting characters</param>
        /// <returns>A substring that represents the specified number of characters from the start of the input string</returns>
        public static string TakeSubstring(this string str, int count)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            return new string(str.Take(count).ToArray());
        }

        /// <summary>
        /// Gets a substring that starts at the first occurrence of the specified substring in the provided string
        /// </summary>
        /// <param name="str">The string to return substring from</param>
        /// <param name="startsWith">Substring to seek</param>
        /// <returns>A substring that starts at the first occurrence of the specified substring in the provided string</returns>
        public static string GetSubstringStartsWith(this string str, string startsWith)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var i = str.IndexOf(startsWith, StringComparison.Ordinal);

            return str.Substring(i, str.Length - i);
        }

        /// <summary>
        /// Removes provided characters from string
        /// </summary>
        /// <param name="str">Target string</param>
        /// <param name="chars">List of characters to remove</param>
        /// <returns>Input string with provided characters removed</returns>
        public static string RemoveCharsFromString(this string str, params char[] chars)
        {
            if (str.IsNullOrEmptyOrWhiteSpace() || chars == null || chars.Length == 0)
            {
                return str;
            }

            return string.Join(string.Empty, str.Split(chars));
        }

        /// <summary>
        /// Checks if provided string represents a numeric type
        /// </summary>
        /// <param name="str">String to check</param>
        /// <returns>True if provided string represents a numeric type</returns>
        public static bool IsNumeric(this string str)
        {
            return !str.IsNullOrEmptyOrWhiteSpace() && double.TryParse(str, NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out _);
        }

        /// <summary>
        /// Returns number of uppercase characters in the provided string
        /// </summary>
        /// <param name="str">Target string</param>
        /// <returns>Number of uppercase characters in the provided string</returns>
        public static int UppercaseCharactersCount(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return 0;
            }

            return str.Count(char.IsUpper);
        }

        /// <summary>
        /// Randomizes the case of characters in the specified string
        /// </summary>
        /// <param name="str">Target string</param>
        /// <returns>Target string with random case characters</returns>
        public static string ToRandomCase(this string str)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
            {
                return string.Empty;
            }

            var random = new Random();

            return new string(str.Select(x =>
                random.Next() % 2 == 0
                    ? char.IsUpper(x) ? x.ToString().ToLower()[0] : x.ToString().ToUpper()[0]
                    : x).ToArray());
        }

        /// <summary>
        /// Removes only the first leading and trailing occurrences of a character from the specified string
        /// </summary>
        /// <param name="str">Target string</param>
        /// <param name="ch">A character to remove</param>
        /// <returns>The string that remains after the first instances of the <paramref name="ch"/> character are removed from the beginning and end of the specified string</returns>
        public static string TrimOnce(this string str, char ch)
        {
            var resultStr = str.Substring(str[0] == ch ? 1 : 0);

            if (str[str.Length - 1] == ch)
            {
                resultStr = resultStr.Substring(0, resultStr.Length - 1);
            }

            return resultStr;
        }
    }
}