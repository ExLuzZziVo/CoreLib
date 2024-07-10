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
    public abstract class StringIsNullOrEmptyOrWhiteSpaceToValueConverter<T> : IValueConverter
    {
        /// <summary>
        /// A value that is returned if the supplied string is null or empty or white space
        /// </summary>
        public T EmptyValue { get; set; }

        /// <summary>
        /// A value that is returned if the supplied string is not null or empty or white space
        /// </summary>
        public T NotEmptyValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
    public abstract class StringContainsToValueConverter<T> : IValueConverter
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

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && s.Contains((string)parameter, StringComparison))
            {
                return ContainsValue;
            }

            return NotContainsValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
    public abstract class TextCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && !s.IsNullOrEmptyOrWhiteSpace())
            {
                if (parameter is TextCase stringCase)
                {
                    switch (stringCase)
                    {
                        case TextCase.Upper:
                        {
                            return s.ToUpper();
                        }
                        case TextCase.Lower:
                        {
                            return s.ToLower();
                        }
                        case TextCase.Title:
                        {
                            return culture.TextInfo.ToTitleCase(s);
                        }
                        default:
                        {
                            return value;
                        }
                    }
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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