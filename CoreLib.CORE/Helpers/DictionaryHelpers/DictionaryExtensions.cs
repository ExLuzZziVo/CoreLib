using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.CORE.Helpers.DictionaryHelpers
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<T,T1>(this Dictionary<T,T1> dictionary, T key, T1 value)
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
