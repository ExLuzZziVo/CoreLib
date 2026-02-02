#region

using System;
using System.Globalization;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts string is null or empty or white space value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class StringIsNullOrEmptyOrWhiteSpaceToValueConverter<T> : ConverterBase
    {
        /// <summary>
        /// A value that is returned if the supplied string is null or empty or white space
        /// </summary>
        public T EmptyValue { get; set; }

        /// <summary>
        /// A value that is returned if the supplied string is not null or empty or white space
        /// </summary>
        public T NotEmptyValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (value as string).IsNullOrEmptyOrWhiteSpace();

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result
                ? EmptyValue
                : NotEmptyValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts string is null or empty or white space value to a boolean
    /// </summary>
    public abstract class
        StringIsNullOrEmptyOrWhiteSpaceToBoolConverter : StringIsNullOrEmptyOrWhiteSpaceToValueConverter<bool>
    {
        protected StringIsNullOrEmptyOrWhiteSpaceToBoolConverter()
        {
            EmptyValue = true;
            NotEmptyValue = false;
        }
    }

    /// <summary>
    /// Converts string containing the provided value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class StringContainsToValueConverter<T> : ConverterBase
    {
        /// <summary>
        /// A value that is returned if the supplied string contains provided value
        /// </summary>
        public T ContainsValue { get; set; }

        /// <summary>
        /// A value that is returned if the supplied string not contains provided value
        /// </summary>
        public T NotContainsValue { get; set; }

        /// <summary>
        /// String comparison type
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="StringComparison.Ordinal"/>
        /// </remarks>
        public StringComparison StringComparison { get; set; } = StringComparison.Ordinal;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s.Contains((string)parameter, StringComparison))
            {
                return ContainsValue;
            }

            return NotContainsValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts string containing the provided value to a boolean
    /// </summary>
    public abstract class StringContainsToBoolConverter : StringContainsToValueConverter<bool>
    {
        protected StringContainsToBoolConverter()
        {
            ContainsValue = true;
            NotContainsValue = false;
        }
    }

    /// <summary>
    /// Converts string to the specified casing
    /// </summary>
    public abstract class TextCaseConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && !s.IsNullOrEmptyOrWhiteSpace())
            {
                if (parameter is TextCase stringCase)
                {
                    return stringCase switch
                    {
                        TextCase.Upper => s.ToUpper(),
                        TextCase.Lower => s.ToLower(),
                        TextCase.Title => culture.TextInfo.ToTitleCase(s),
                        _ => value
                    };
                }
            }

            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Text character casing types. Used for <see cref="TextCaseConverter"/> as a ConverterParameter
    /// </summary>
    public enum TextCase : byte
    {
        /// <summary>
        /// All text characters remain unchanged
        /// </summary>
        None,

        /// <summary>
        /// All text characters become uppercase
        /// </summary>
        Upper,

        /// <summary>
        /// All text characters become lowercase
        /// </summary>
        Lower,

        /// <summary>
        /// All text becomes title case
        /// </summary>
        Title
    }
}
