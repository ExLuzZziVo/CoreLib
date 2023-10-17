#region

using System;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class UnixTimestampConverterAttribute : JsonConverterAttribute
    {
        private readonly bool _useLocalTime;

        public UnixTimestampConverterAttribute(bool useLocalTime = false)
        {
            _useLocalTime = useLocalTime;
        }

        public override JsonConverter CreateConverter(Type typeToConvert)
        {
            if (typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?))
            {
                return new UnixTimestampConverter(_useLocalTime);
            }

            throw new NotSupportedException(
                "This converter only supports DateTime, DateTime? types.");
        }
    }
}