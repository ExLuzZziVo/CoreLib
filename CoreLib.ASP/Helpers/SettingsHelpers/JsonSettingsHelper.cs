#region

using System.IO;
using System.Text.Json;
using CoreLib.CORE.Helpers.ObjectHelpers;

#endregion

namespace CoreLib.ASP.Helpers.SettingsHelpers
{
    public static class JsonSettingsHelper
    {
        private static readonly JsonSerializerOptions IndentedJsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// Adds or updates a value in the specified json settings file
        /// </summary>
        /// <param name="settingsFilePath">Full path to the json settings file</param>
        /// <param name="key">Json key</param>
        /// <param name="value">New value</param>
        public static void AddOrUpdateSettingsFile<T>(string settingsFilePath, string key, T value)
        {
            var input = File.ReadAllText(settingsFilePath);
            var jsonObj = JsonSerializer.Deserialize<object>(input);

            jsonObj.SetPropertyValueByName(key.Replace(':', '.'), value);

            var output = JsonSerializer.Serialize(jsonObj, IndentedJsonSerializerOptions);
            File.WriteAllText(settingsFilePath, output);
        }
    }
}