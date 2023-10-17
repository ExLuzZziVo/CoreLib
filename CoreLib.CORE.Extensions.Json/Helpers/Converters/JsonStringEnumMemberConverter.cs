#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CoreLib.CORE.Helpers.ObjectHelpers;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    // https://github.com/dotnet/runtime/issues/31081#issuecomment-848697673
    public class JsonStringEnumMemberConverter : JsonConverter<object>
    {
        private readonly Dictionary<Enum, string> _enumToString = new Dictionary<Enum, string>();
        private readonly Dictionary<string, Enum> _stringToEnum = new Dictionary<string, Enum>();

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();

                return;
            }

            writer.WriteStringValue(_enumToString[(Enum)value]);
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

            var str = reader.GetString();

            if (_stringToEnum.TryGetValue(str, out var value))
            {
                return value;
            }

            return default;
        }

        public sealed override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.IsEnum)
            {
                var values = Enum.GetValues(typeToConvert);

                foreach (Enum value in values)
                {
                    var enumMember = typeToConvert.GetMember(value.ToString())[0];

                    var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                        .Cast<EnumMemberAttribute>()
                        .FirstOrDefault();

                    _stringToEnum.Add(value.ToString(), value);

                    if (attr?.Value != null)
                    {
                        _enumToString.Add(value, attr.Value);
                        _stringToEnum.Add(attr.Value, value);
                    }
                    else
                    {
                        _enumToString.Add(value, value.ToString());
                    }
                }
            }

            return typeToConvert.IsEnum;
        }
    }
}