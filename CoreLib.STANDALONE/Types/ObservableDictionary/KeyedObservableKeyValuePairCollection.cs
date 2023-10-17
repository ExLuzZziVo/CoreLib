#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

#endregion

namespace CoreLib.STANDALONE.Types.ObservableDictionary
{
    /// <summary>
    /// Implementation of <see cref="KeyedCollection{TKey,TItem}"/> for internal user in <see cref="ObservableDictionary{TKey,TValue}"/>
    /// </summary>
    internal class
        KeyedObservableKeyValuePairCollection<TKey, TValue> : KeyedCollection<TKey,
            ObservableKeyValuePair<TKey, TValue>>
    {
        public KeyedObservableKeyValuePairCollection() : base() { }

        public KeyedObservableKeyValuePairCollection(IEqualityComparer<TKey> comparer) : base(comparer) { }

        protected override void InsertItem(int index, ObservableKeyValuePair<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            item.PropertyChanged += ItemOnPropertyChanged;

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= ItemOnPropertyChanged;

            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            foreach (var i in Items)
            {
                i.PropertyChanged -= ItemOnPropertyChanged;
            }

            base.ClearItems();
        }

        protected override void SetItem(int index, ObservableKeyValuePair<TKey, TValue> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Items[index].PropertyChanged -= ItemOnPropertyChanged;
            item.PropertyChanged += ItemOnPropertyChanged;

            base.SetItem(index, item);
        }

        protected override TKey GetKeyForItem(ObservableKeyValuePair<TKey, TValue> entry)
        {
            return entry.Key;
        }

        private void ItemOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Key")
            {
                var item = (ObservableKeyValuePair<TKey, TValue>)sender;
                var index = IndexOf(item);

                if (Dictionary.ContainsKey(item.Key))
                {
                    throw new ArgumentException("Object with this Key already exists", nameof(item.Key));
                }

                SetItem(index, item);

                foreach (var i in Dictionary)
                {
                    if (!this.Any(p => Equals(p.Key, i.Key)))
                    {
                        Dictionary.Remove(i.Key);

                        break;
                    }
                }
            }
        }
    }
}