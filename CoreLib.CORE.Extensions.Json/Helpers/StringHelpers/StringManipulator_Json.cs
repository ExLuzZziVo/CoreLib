#region

using System;
using System.Text.Encodings.Web;
using System.Text.Json;

#endregion

namespace CoreLib.CORE.Helpers.StringHelpers
{
    public static class StringManipulator_Json
    {
        private static readonly JsonSerializerOptions IndentedJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// Beautifies the provided Json string
        /// </summary>
        /// <param name="jsonString">Target Json string</param>
        /// <returns>Returns a new beautified Json string</returns>
        public static string BeautifyJson(string jsonString)
        {
            if (jsonString.IsNullOrEmptyOrWhiteSpace())
            {
                return jsonString;
            }

            try
            {
                using (var jsonDocument = JsonDocument.Parse(jsonString))
                {
                    var result = JsonSerializer.Serialize(jsonDocument.RootElement, IndentedJsonSerializerOptions);

                    return result;
                }
            }
            catch
            {
                throw new ArgumentException("The passed string cannot be parsed as Json.", nameof(jsonString));
            }
        }
    }
}