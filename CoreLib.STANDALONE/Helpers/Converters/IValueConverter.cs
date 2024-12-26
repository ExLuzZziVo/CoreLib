#region

using System;
using System.Globalization;

#endregion

namespace CoreLib.STANDALONE.Helpers.Converters
{
    /// <summary>
    /// A helper internal interface for value converters
    /// </summary>
    internal interface IValueConverter
    {
        object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        object Convert(object value, Type targetType, object parameter, string language);
        object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        object ConvertBack(object value, Type targetType, object parameter, string language);
    }

    /// <summary>
    /// A base class for the multiplatform converter support
    /// </summary>
    public abstract class ConverterBase: IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Convert(value, targetType, parameter, new CultureInfo(language));
        }

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return ConvertBack(value, targetType, parameter, new CultureInfo(language));
        }
    }

    /// <summary>
    /// A helper internal interface for multi-value converters
    /// </summary>
    internal interface IMultiValueConverter
    {
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}