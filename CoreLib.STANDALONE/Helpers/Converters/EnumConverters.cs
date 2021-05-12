using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CoreLib.CORE.Helpers.EnumHelpers;
using Newtonsoft.Json;

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// Converts an enum to its <see cref="DescriptionAttribute"/> value and back
    /// </summary>
    public abstract class EnumToDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "" : ((Enum) value).GetDescription();
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "" : ((Enum) value).GetDisplayName(culture);
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
                return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value), targetType);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}