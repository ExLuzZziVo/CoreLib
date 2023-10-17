#region

using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace CoreLib.CORE.Helpers.Converters
{
    public class JsonLowerCaseStringEnumConverter : JsonStringEnumConverter
    {
        public JsonLowerCaseStringEnumConverter() : this(true) { }
        
        public JsonLowerCaseStringEnumConverter(bool allowIntegerValues = true) : base(LowerCaseNamingPolicy,
            allowIntegerValues) { }

        public static JsonNamingPolicy LowerCaseNamingPolicy { get; } = new LowerCaseNamingPolicy();
    }
}