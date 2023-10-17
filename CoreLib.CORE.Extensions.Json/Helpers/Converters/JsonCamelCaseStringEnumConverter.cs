#region

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class JsonCamelCaseStringEnumConverter : JsonStringEnumConverter
    {
        public JsonCamelCaseStringEnumConverter() : this(true) { }
        
        public JsonCamelCaseStringEnumConverter(bool allowIntegerValues = true) : base(JsonNamingPolicy.CamelCase,
            allowIntegerValues) { }
    }
}