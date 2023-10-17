#region

using System;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class CustomDateTimeConverterAttribute : JsonConverterAttribute
    {
        private readonly string _dateTimeFormat;

        public CustomDateTimeConverterAttribute(string dateTimeFormat)
        {
            _dateTimeFormat = dateTimeFormat;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?) ||
                typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?))
            {
                return new CustomDateTimeConverter() { DateTimeFormat = _dateTimeFormat };
            }

            throw new NotSupportedException(
                "This converter only supports DateTime, DateTime?, DateTimeOffset, DateTimeOffset? types.");
        }
    }
}