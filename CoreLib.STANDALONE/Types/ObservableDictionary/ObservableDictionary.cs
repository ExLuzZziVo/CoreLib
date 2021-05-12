#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Types.ObservableDictionary
{
    /// <summary>
    /// The implementation of observable dictionary
    /// </summary>
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<ObservableKeyValuePair<TKey, TValue>>,
        IDictionary<TKey, TValue>, IDictionary
    {
        private readonly KeyedObservableKeyValuePairCollection<TKey, TValue> _internalDictionary =
            new KeyedObservableKeyValuePairCollection<TKey, TValue>();

        public ObservableDictionary() : base() { }

        public ObservableDictionary(List<ObservableKeyValuePair<TKey, TValue>> list) : base(list)
        {
            foreach (var pair in list)
            {
                _internalDictionary.Add(pair);
            }
        }

        public ObservableDictionary(IEnumerable<ObservableKeyValuePair<TKey, TValue>> enumerable) : base(enumerable)
        {
            foreach (var pair in enumerable)
            {
                _internalDictionary.Add(pair);
            }
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            foreach (var pair in dictionary)
            {
                var observableKeyValuePair = new ObservableKeyValuePair<TKey, TValue>(pair.Key, pair.Value);
                Items.Add(observableKeyValuePair);
                _internalDictionary.Add(observableKeyValuePair);
            }
        }

        protected override void ClearItems()
        {
            _internalDictionary.Clear();
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            _internalDictionary.RemoveAt(index);
            base.RemoveItem(index);
        }

        protected override void InsertItem(int index, ObservableKeyValuePair<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _internalDictionary.Insert(index, item);
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, ObservableKeyValuePair<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _internalDictionary[index] = item;

            base.SetItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            var observableKeyValuePair = this[oldIndex];
            _internalDictionary.RemoveAt(oldIndex);
            _internalDictionary.Insert(newIndex, observableKeyValuePair);

            base.MoveItem(oldIndex, newIndex);
        }

        #region IDictionary<TKey, TValue>

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _internalDictionary.Select(p => p.Key).ToList();

        public ICollection<TValue> Values => _internalDictionary.Select(p => p.Value).ToList();

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new ObservableDictionaryEnumerator<TKey, TValue>(this, false);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(new ObservableKeyValuePair<TKey, TValue>(item));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Contains(new ObservableKeyValuePair<TKey, TValue>(item));
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > array.Length || array.Length - arrayIndex < _internalDictionary.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            foreach (var entry in _internalDictionary)
            {
                array[arrayIndex++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return base.Remove(new ObservableKeyValuePair<TKey, TValue>(item));
        }

        public bool ContainsKey(TKey key)
        {
            return _internalDictionary.Contains(key);
        }

        public void Add(TKey key, TValue value)
        {
            Add(new ObservableKeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            return base.Remove(_internalDictionary[key]);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var result = _internalDictionary.Contains(key);
            value = result ? _internalDictionary[key].Value : default;

            return result;
        }

        public TValue this[TKey key]
        {
            get => _internalDictionary[key].Value;
            set
            {
                if (!ContainsKey(key))
                {
                    Add(key, value);
                }
                else
                {
                    _internalDictionary[key].Value = value;
                }
            }
        }

        #endregion

        #region IDictionary

        public bool IsFixedSize => false;

        ICollection IDictionary.Keys => _internalDictionary.Select(p => p.Key).ToList();

        ICollection IDictionary.Values => _internalDictionary.Select(p => p.Value).ToList();

        bool IDictionary.Contains(object key)
        {
            return ContainsKey((TKey) key);
        }

        void IDictionary.Add(object key, object value)
        {
            Add((TKey) key, (TValue) value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new ObservableDictionaryEnumerator<TKey, TValue>(this, true);
        }

        void IDictionary.Remove(object key)
        {
            Remove((TKey) key);
        }

        object IDictionary.this[object key]
        {
            get => this[(TKey) key];
            set => this[(TKey) key] = (TValue) value;
        }

        #endregion
    }
}