#region

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreLib.CORE.Helpers.ObjectHelpers;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    internal class CustomDateTimeConverter : JsonConverter<object>
    {
        public DateTimeStyles DateTimeStyles { get; set; } = DateTimeStyles.RoundtripKind;

        public string DateTimeFormat { get; set; }

        public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();

                return;
            }

            string text;

            if (value is DateTime dateTime)
            {
                if ((DateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTime = dateTime.ToUniversalTime();
                }

                text = dateTime.ToString(DateTimeFormat ?? "O", Culture);
            }
            else if (value is DateTimeOffset dateTimeOffset)
            {
                if ((DateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                    || (DateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
                {
                    dateTimeOffset = dateTimeOffset.ToUniversalTime();
                }

                text = dateTimeOffset.ToString(DateTimeFormat ?? "O", Culture);
            }
            else
            {
                throw new JsonException(
                    $"Unexpected value when converting date. Expected DateTime or DateTimeOffset but got {value.GetType()}.");
            }

            writer.WriteStringValue(text);
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

            var type = isNullable ? Nullable.GetUnderlyingType(typeToConvert) : typeToConvert;

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Cannot convert value with token {reader.TokenType} to {typeToConvert}");
            }

            var str = reader.GetString();

            if (isNullable && string.IsNullOrEmpty(str))
            {
                return null;
            }

            if (type == typeof(DateTimeOffset))
            {
                if (!reader.TryGetDateTimeOffset(out var value))
                {
                    value = !string.IsNullOrEmpty(DateTimeFormat)
                        ? DateTimeOffset.ParseExact(str, DateTimeFormat, Culture, DateTimeStyles)
                        : DateTimeOffset.Parse(str, Culture, DateTimeStyles);
                }

                return value;
            }

            {
                if (!reader.TryGetDateTime(out var value))
                {
                    value = !string.IsNullOrEmpty(DateTimeFormat)
                        ? DateTime.ParseExact(str, DateTimeFormat, Culture, DateTimeStyles)
                        : DateTime.Parse(str, Culture, DateTimeStyles);
                }

                return value;
            }
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime) || typeToConvert == typeof(DateTime?) ||
                   typeToConvert == typeof(DateTimeOffset) || typeToConvert == typeof(DateTimeOffset?);
        }
    }
}