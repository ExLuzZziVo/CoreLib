using System;
using System.Globalization;
using CoreLib.CORE.Helpers.DateTimeHelpers;

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class DateTimeIsNullOrNewToValueConverter<T> : IValueConverter
    {
        /// <summary>
        /// Object value representing a null or new <see cref="DateTime"/> value
        /// </summary>
        public T NullOrNewValue { get; set; }

        /// <summary>
        /// Object value representing not null or new <see cref="DateTime"/> value
        /// </summary>
        public T NotNullOrNewValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime?) value;
            var result = dateTime.IsNullOrNewDateTime();

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result ? NullOrNewValue : NotNullOrNewValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> to a boolean
    /// </summary>
    public abstract class DateTimeIsNullOrNewToBoolConverter : DateTimeIsNullOrNewToValueConverter<bool>
    {
        protected DateTimeIsNullOrNewToBoolConverter()
        {
            NullOrNewValue = true;
            NotNullOrNewValue = false;
        }
    }
}