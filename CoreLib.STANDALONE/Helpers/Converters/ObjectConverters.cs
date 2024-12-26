#region

using System;
using System.Globalization;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts multiple objects to a new array
    /// </summary>
    public abstract class MultiBindingToObjectArrayConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Converter returns reference to object, so we need to create new array
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts an object equality to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class ObjectEqualsToValueConverter<T> : ConverterBase
    {
        /// <summary>
        /// An object value that is returned if the supplied objects are equal
        /// </summary>
        public T EqualsValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied objects are not equal
        /// </summary>
        public T NotEqualsValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return parameter == null ? EqualsValue : NotEqualsValue;
            }

            return value.Equals(parameter) ? EqualsValue : NotEqualsValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts an object equality to a boolean
    /// </summary>
    public abstract class ObjectEqualsToBoolConverter : ObjectEqualsToValueConverter<bool>
    {
        protected ObjectEqualsToBoolConverter()
        {
            EqualsValue = true;
            NotEqualsValue = false;
        }
    }

    /// <summary>
    /// Converts an object is null to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class ObjectIsNullToValueConverter<T> : ConverterBase
    {
        /// <summary>
        /// A value that is returned if the supplied object is null
        /// </summary>
        public T NullValue { get; set; }

        /// <summary>
        /// A value that is returned if the supplied object is not null
        /// </summary>
        public T NotNullValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value == null;

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result ? NullValue : NotNullValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts an object is null to a string
    /// </summary>
    public abstract class ObjectIsNullToStringConverter : ObjectIsNullToValueConverter<string> { }

    /// <summary>
    /// Converts an object is null to a boolean
    /// </summary>
    public abstract class ObjectIsNullToBoolConverter : ObjectIsNullToValueConverter<bool>
    {
        protected ObjectIsNullToBoolConverter()
        {
            NullValue = true;
            NotNullValue = false;
        }
    }
}