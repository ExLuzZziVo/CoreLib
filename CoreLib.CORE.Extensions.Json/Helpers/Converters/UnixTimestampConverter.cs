#region

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreLib.CORE.Helpers.ObjectHelpers;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    /// <summary>
    /// Converts unix timestamp to <see cref="System.DateTime"/> and back
    /// </summary>
    internal class UnixTimestampConverter : JsonConverter<object>
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private readonly bool _useLocalTime;

        /// <summary>
        /// Converts unix timestamp to <see cref="System.DateTime"/> and back
        /// </summary>
        internal UnixTimestampConverter() { }

        /// <summary>
        /// Converts unix timestamp to local <see cref="System.DateTime"/> and back if the <paramref name="useLocalTime"/> is set to true
        /// </summary>
        /// <param name="useLocalTime">If true, the provided unix timestamp will be converted to local time and back</param>
        internal UnixTimestampConverter(bool useLocalTime)
        {
            _useLocalTime = useLocalTime;
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();

                return;
            }

            if (!(value is DateTime dateTime))
            {
                throw new JsonException(
                    $"Unexpected value when converting date. Expected DateTime but got {value.GetType()}.");
            }

            writer.WriteStringValue(((_useLocalTime ? dateTime.ToUniversalTime() : dateTime) - UnixEpoch)
                .TotalMilliseconds + "000");
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var isNullable = typeToConvert.IsNullable();

            if (reader.TokenType == JsonTokenType.Null)
            {
                if (!isNullable)
                {
                    throw new JsonException($"Cannot convert null value to {typeToConvert}");
                }

                return null;
            }

            if (!(reader.TokenType == JsonTokenType.String || reader.TokenType == JsonTokenType.Number))
            {
                throw new JsonException($"Cannot convert value with token {reader.TokenType} to {typeToConvert}");
            }

            if (!reader.TryGetInt64(out var value))
            {
                var str = reader.GetString();

                if (isNullable && string.IsNullOrEmpty(str))
                {
                    return null;
                }

                value = long.Parse(str);
            }

            var result = UnixEpoch.AddMilliseconds(value);

            return _useLocalTime ? result.ToLocalTime() : result;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?);
        }
    }
}