#region

using System.Collections.Generic;

#endregion

namespace CoreLib.STANDALONE.Types.ObservableDictionary
{
    /// <summary>
    /// Observable key-value pair implementation for <see cref="ObservableDictionary{TKey,TValue}"/>
    /// </summary>
    public class ObservableKeyValuePair<TKey, TValue> : ViewModelBase
    {
        public ObservableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public ObservableKeyValuePair(KeyValuePair<TKey, TValue> keyValuePair)
        {
            Key = keyValuePair.Key;
            Value = keyValuePair.Value;
        }

        public TKey Key
        {
            get => GetValue<TKey>();
            set => SetValue(value);
        }

        public TValue Value
        {
            get => GetValue<TValue>();
            set => SetValue(value);
        }

        public override string ToString()
        {
            return $"[{Key?.ToString()},{Value?.ToString()}]";
        }
    }
}