#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts a number of elements in the sequence to an object of type <typeparamref name="T"/>
    /// </summary>
    public abstract class CollectionCountToValueConverter<T> : ConverterBase
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

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                null => NullValue,

                // For dictionaries
                ICollection collection => GetValue(collection.Count, parameter),
                IEnumerable<object> enumerable => GetValue(enumerable.Count(), parameter),
                _ => throw new NotSupportedException()
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private T GetValue(int count, object parameter)
        {
            var result = GreaterThanValue != null &&
                LessThenValue != null &&
                count > GreaterThanValue &&
                count < LessThenValue ||
                count > GreaterThanValue ||
                count < LessThenValue;

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
    public abstract class CollectionIsEmptyToValueConverter<T> : ConverterBase
    {
        /// <summary>
        /// An object value that is returned if the supplied sequence is empty
        /// </summary>
        public T EmptyValue { get; set; }

        /// <summary>
        /// An object value that is returned if the supplied sequence is not empty
        /// </summary>
        public T NotEmptyValue { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value switch
            {
                null => false,

                // For dictionaries
                ICollection collection => collection.Count > 0,
                IEnumerable<object> enumerable => enumerable.Any(),
                _ => throw new NotSupportedException()
            };

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

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
