using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreLib.STANDALONE.Types.ObservableDictionary
{
    /// <summary>
    /// <see cref="IEnumerator"/> implementation for <see cref="ObservableDictionary{TKey,TValue}"/>
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ObservableDictionaryEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>>,
        IDictionaryEnumerator
    {
        private ObservableDictionary<TKey, TValue> _observableDictionary;
        private bool _isDictionaryEntryEnumerator;
        private int _index;
        private KeyValuePair<TKey, TValue> _current;

        internal ObservableDictionaryEnumerator(ObservableDictionary<TKey, TValue> observableDictionary,
            bool isDictionaryEntryEnumerator)
        {
            _observableDictionary = observableDictionary;
            _isDictionaryEntryEnumerator = isDictionaryEntryEnumerator;
            _index = -1;
            _current = new KeyValuePair<TKey, TValue>();
        }

        private void ValidateCurrent()
        {
            if (_index == -1 || _index >= _observableDictionary.Count)
            {
                throw new InvalidOperationException();
            }
        }

        #region IEnumerator<KeyValuePair<TKey, TValue>>

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                ValidateCurrent();

                return _current;
            }
        }

        public bool MoveNext()
        {
            if (_index < _observableDictionary.Count - 1)
            {
                _index++;
                var observableKeyValuePair = _observableDictionary[_index];
                _current = new KeyValuePair<TKey, TValue>(observableKeyValuePair.Key, observableKeyValuePair.Value);

                return true;
            }

            _current = new KeyValuePair<TKey, TValue>();

            return false;
        }

        object IEnumerator.Current
        {
            get
            {
                ValidateCurrent();

                if (_isDictionaryEntryEnumerator)
                {
                    return new DictionaryEntry(_current.Key, _current.Value);
                }

                return new KeyValuePair<TKey, TValue>(_current.Key, _current.Value);
            }
        }

        void IEnumerator.Reset()
        {
            _index = -1;
            _current = new KeyValuePair<TKey, TValue>();
        }

        public void Dispose() { }

        #endregion

        #region IDictionaryEnumerator

        DictionaryEntry IDictionaryEnumerator.Entry
        {
            get
            {
                ValidateCurrent();

                return new DictionaryEntry(_current.Key, _current.Value);
            }
        }

        object IDictionaryEnumerator.Key
        {
            get
            {
                ValidateCurrent();

                return _current.Key;
            }
        }

        object IDictionaryEnumerator.Value
        {
            get
            {
                ValidateCurrent();

                return _current.Value;
            }
        }

        #endregion
    }
}