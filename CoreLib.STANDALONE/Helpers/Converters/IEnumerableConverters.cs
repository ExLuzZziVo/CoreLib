#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts a number of elements in the sequence to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class CollectionCountToValueConverter<T> : IValueConverter
    {
        /// <summary>
        /// An object value that is returned if the number of elements of the supplied sequence is greater than <see cref="GreaterThanValue"/> or less than <see cref="LessThenValue"/>
        /// </summary>
        public T TrueValue { get; set; }

        /// <summary>
        /// An object value that is returned if the number of elements of the supplied sequence is less than <see cref="GreaterThanValue"/> or greater than <see cref="LessThenValue"/> or these values are null
        /// </summary>
        public T FalseValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied sequence is null
        /// </summary>
        public T NullValue { get; set; }

        /// <summary>
        /// A number of elements of the supplied sequence which must be greater than this value in order to return <see cref="TrueValue"/>
        /// </summary>
        public int? GreaterThanValue { get; set; } = null;

        /// <summary>
        /// A number of elements of the supplied sequence which must be less than this value in order to return <see cref="TrueValue"/>
        /// </summary>
        public int? LessThenValue { get; set; } = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return NullValue;
            }

            if (value is IEnumerable<object> objects)
            {
                var val = objects.Count();

                var result = false;

                if (GreaterThanValue != null && LessThenValue != null &&
                    val > GreaterThanValue && val < LessThenValue)
                {
                    result = true;
                }
                else if (GreaterThanValue != null && val > GreaterThanValue)
                {
                    result = true;
                }
                else if (LessThenValue != null && val < LessThenValue.Value)
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

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// A number of elements in the sequence to a boolean converter
    /// </summary>
    public abstract class CollectionCountToBoolConverter : CollectionCountToValueConverter<bool>
    {
        protected CollectionCountToBoolConverter()
        {
            TrueValue = true;
            FalseValue = false;
            NullValue = false;
        }
    }

    /// <summary>
    /// Converts a sequence emptiness to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class CollectionIsEmptyToValueConverter<T> : IValueConverter
    {
        /// <summary>
        /// An object value that is returned if the supplied sequence is empty
        /// </summary>
        public T EmptyValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied sequence is not empty
        /// </summary>
        public T NotEmptyValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value == null)
            {
                result = false;
            }
            else if (value is IEnumerable<object> objects)
            {
                result = objects.Any();
            }
            else
            {
                throw new NotSupportedException();
            }

            if (bool.TryParse(parameter?.ToString(), out var par))
            {
                if (par)
                {
                    result = !result;
                }
            }

            return result
                ? NotEmptyValue
                : EmptyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// Converts a sequence emptiness to a boolean
    /// </summary>
    public abstract class CollectionIsEmptyToBoolConverter : CollectionIsEmptyToValueConverter<bool>
    {
        protected CollectionIsEmptyToBoolConverter()
        {
            EmptyValue = true;
            NotEmptyValue = false;
        }
    }
}