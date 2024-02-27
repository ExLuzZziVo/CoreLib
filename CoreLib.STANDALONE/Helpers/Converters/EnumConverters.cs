#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using CoreLib.CORE.Helpers.EnumHelpers;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts an enum to its <see cref="DescriptionAttribute"/> value and back
    /// </summary>
    public abstract class EnumToDescriptionConverter : IValueConverter
    {
        private static readonly Dictionary<Enum, string> Cache = new Dictionary<Enum, string>();
        private readonly bool _isCacheEnabled;

        /// <summary>
        /// The constructor is used to enable a cache to store <see cref="Enum"/> values and their descriptions
        /// </summary>
        /// <param name="isCacheEnabled">A flag indicating that <see cref="Enum"/> descriptions caching will be enabled. Is disabled by default</param>
        protected EnumToDescriptionConverter(bool isCacheEnabled = false)
        {
            _isCacheEnabled = isCacheEnabled;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var en = (Enum)value;

            if (_isCacheEnabled)
            {
                if (Cache.TryGetValue(en, out var val))
                {
                    return val;
                }
                else
                {
                    val = en.GetDescription();
                    Cache.Add(en, val);

                    return val;
                }
            }

            return en.GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? null
                : Enum.GetValues(Nullable.GetUnderlyingType(targetType) ?? targetType).Cast<Enum>()
                    .FirstOrDefault(one => value.ToString() == one.GetDescription());
        }
    }

    /// <summary>
    /// Converts an enum to its <see cref="DisplayNameAttribute"/> value and back
    /// </summary>
    public abstract class EnumToDisplayNameConverter : IValueConverter
    {
        private static readonly Dictionary<Enum, string> Cache = new Dictionary<Enum, string>();
        private readonly bool _isCacheEnabled;

        /// <summary>
        /// The constructor is used to enable a cache to store <see cref="Enum"/> values and their display names
        /// </summary>
        /// <param name="isCacheEnabled">A flag indicating that <see cref="Enum"/> display names caching will be enabled. Is disabled by default</param>
        protected EnumToDisplayNameConverter(bool isCacheEnabled = false)
        {
            _isCacheEnabled = isCacheEnabled;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var en = (Enum)value;

            if (_isCacheEnabled)
            {
                if (Cache.TryGetValue(en, out var val))
                {
                    return val;
                }
                else
                {
                    val = en.GetDisplayName(culture);
                    Cache.Add(en, val);

                    return val;
                }
            }

            return en.GetDisplayName(culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? null
                : Enum.GetValues(Nullable.GetUnderlyingType(targetType) ?? targetType).Cast<Enum>()
                    .FirstOrDefault(one => value.ToString() == one.GetDisplayName(culture));
        }
    }

    /// <summary>
    /// Converts an enum to a target type
    /// </summary>
    public abstract class EnumCastConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var valueType = value.GetType();

            if (targetType == typeof(object) && parameter is Type t)
            {
                targetType = t;
            }

            if (!valueType.IsEnum && targetType.IsEnum && Enum.IsDefined(targetType, value))
            {
                return Enum.ToObject(targetType, value);
            }

            if (valueType.IsEnum)
            {
                // I know that it is very bad code, but it works
                return JsonSerializer.Deserialize(JsonSerializer.Serialize(value), targetType);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}