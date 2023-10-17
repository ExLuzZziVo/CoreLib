#region

using System;
using System.Globalization;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts a boolean value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class BoolToValueConverter<T> : IValueConverter
    {
        /// <summary>
        /// Object value representing a false boolean value
        /// </summary>
        public T FalseValue { get; set; }

        /// <summary>
        /// Object value representing a true boolean value
        /// </summary>
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetValue, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value == null)
            {
                result = false;
            }
            else if (value is bool val)
            {
                result = val;
            }

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var result = false;

            if (value == null)
            {
                result = false;
            }
            else if (value.Equals(TrueValue))
            {
                result = true;
            }

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Converts an object of type <typeparamref name="T"/> to a boolean value
    /// </summary>
    public abstract class ValueToBoolConverter<T> : IValueConverter
    {
        /// <summary>
        /// Object value representing a false boolean value
        /// </summary>
        public T FalseValue { get; set; }

        /// <summary>
        /// Object value representing a true boolean value
        /// </summary>
        public T TrueValue { get; set; }

        public object Convert(object value, Type targetValue, object parameter, CultureInfo culture)
        {
            var result = value.Equals(TrueValue);

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            var result = false;

            if (value == null)
            {
                result = false;
            }
            else if (value.Equals(TrueValue))
            {
                result = true;
            }

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result ? TrueValue : FalseValue;
        }
    }

    /// <summary>
    /// Converter that inverts boolean value
    /// </summary>
    public abstract class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetValue, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    /// <summary>
    /// Converts multiple boolean values to one boolean
    /// </summary>
    public abstract class MultiBooleanToBooleanConverter : IMultiValueConverter
    {
        protected MultiBooleanToBooleanConverter()
        {
            AllFalseValue = false;
            AllTrueValue = true;
            DifferentBoolValues = false;
        }

        /// <summary>
        /// A value indicating that all supplied values are false
        /// </summary>
        public bool AllFalseValue { get; set; }

        /// <summary>
        /// A value indicating that all supplied values are true
        /// </summary>
        public bool AllTrueValue { get; set; }

        /// <summary>
        /// A value indicating that all supplied values are true and false
        /// </summary>
        public bool DifferentBoolValues { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.All(o => (bool)o))
            {
                return AllTrueValue;
            }
            else if (values.All(o => !(bool)o))
            {
                return AllFalseValue;
            }
            else
            {
                return DifferentBoolValues;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}