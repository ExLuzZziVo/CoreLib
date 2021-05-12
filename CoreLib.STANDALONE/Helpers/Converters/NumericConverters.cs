using System;
using System.Globalization;
using CoreLib.CORE.Helpers.ObjectHelpers;

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts an object of type <typeparamref name="T1"/> that is struct and implements <see cref="IComparable"/> interface (e.g. <see cref="int"/>, <see cref="double"/>...) to an object of type <typeparamref name="T2"/>
    /// </summary>
    public abstract class StructToValueConverter<T1, T2> : IValueConverter where T1 : struct, IComparable
    {
        /// <summary>
        /// An object value that is returned if the supplied value is greater than <see cref="GreaterThanValue"/> or less than <see cref="LessThenValue"/>
        /// </summary>
        public T2 TrueValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied value is less than <see cref="GreaterThanValue"/> or greater than <see cref="LessThenValue"/> or these values are null
        /// </summary>
        public T2 FalseValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied value is null
        /// </summary>
        public T2 NullValue { get; set; }

        /// <summary>
        /// A value of the supplied object which must be greater than this value in order to return <see cref="TrueValue"/>
        /// </summary>
        public T1? GreaterThanValue { get; set; } = null;

        /// <summary>
        /// A value of the supplied object which must be less than this value in order to return <see cref="TrueValue"/>
        /// </summary>
        public T1? LessThenValue { get; set; } = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return NullValue;
            }

            T1? val = null;

            if (value is T1 num)
            {
                val = num;
            }
            else if (value.ToString().TryParse<T1>(out var res))
            {
                val = res;
            }

            if (val == null)
            {
                return NullValue;
            }

            var result = false;

            if (GreaterThanValue != null && LessThenValue != null &&
                val.Value.IsGreaterThan(GreaterThanValue.Value) && val.Value.IsLessThan(LessThenValue.Value))
            {
                result = true;
            }
            else if (GreaterThanValue != null && val.Value.IsGreaterThan(GreaterThanValue.Value))
            {
                result = true;
            }
            else if (LessThenValue != null && val.Value.IsLessThan(LessThenValue.Value))
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts an <see cref="int"/> value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class IntToValueConverter<T> : StructToValueConverter<int, T> { }

    /// <summary>
    /// Converts a <see cref="double"/> value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class DoubleToValueConverter<T> : StructToValueConverter<double, T> { }

    /// <summary>
    /// Converts a <see cref="decimal"/> value to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class DecimalToValueConverter<T> : StructToValueConverter<decimal, T> { }

    /// <summary>
    /// Converts an <see cref="int"/> value to a boolean
    /// </summary>
    public abstract class IntToBoolConverter : IntToValueConverter<bool>
    {
        protected IntToBoolConverter()
        {
            TrueValue = true;
            FalseValue = false;
            NullValue = false;
        }
    }

    /// <summary>
    /// Converts a <see cref="double"/> value to a boolean
    /// </summary>
    public abstract class DoubleToBoolConverter : DoubleToValueConverter<bool>
    {
        protected DoubleToBoolConverter()
        {
            TrueValue = true;
            FalseValue = false;
            NullValue = false;
        }
    }

    /// <summary>
    /// Converts a <see cref="decimal"/> value to a boolean
    /// </summary>
    public abstract class DecimalToBoolConverter : DecimalToValueConverter<bool>
    {
        protected DecimalToBoolConverter()
        {
            TrueValue = true;
            FalseValue = false;
            NullValue = false;
        }
    }
}