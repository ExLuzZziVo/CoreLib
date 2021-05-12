#region

using System.IO;
using CoreLib.CORE.Helpers.StringHelpers;
using Newtonsoft.Json;

#endregion

namespace CoreLib.ASP.Extensions.Json.Helpers.SettingsHelpers
{
    public static class JsonSettingsHelper
    {
        /// <summary>
        /// Adds or updates a value in the specified json settings file
        /// </summary>
        /// <param name="settingsFilePath">Full path to the json settings file</param>
        /// <param name="key">Json key</param>
        /// <param name="value">New value</param>
        public static void AddOrUpdateSettingsFile<T>(string settingsFilePath, string key, T value)
        {
            var input = File.ReadAllText(settingsFilePath);
            dynamic jsonObj = JsonConvert.DeserializeObject(input);
            var sectionPath = key.Split(':')[0];

            if (!sectionPath.IsNullOrEmptyOrWhiteSpace())
            {
                var keyPath = key.Split(':')[1];
                jsonObj[sectionPath][keyPath] = value;
            }
            else
            {
                jsonObj[sectionPath] = value;
            }

            var output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(settingsFilePath, output);
        }
    }
}