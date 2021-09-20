#region

using System.Collections.Generic;

#endregion

namespace CoreLib.CORE.Helpers.DictionaryHelpers
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Updates value in dictionary if key exists. If not, adds provided key-value pair
        /// </summary>
        /// <param name="dictionary">Target dictionary</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void AddOrUpdate<T, T1>(this IDictionary<T, T1> dictionary, T key, T1 value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}