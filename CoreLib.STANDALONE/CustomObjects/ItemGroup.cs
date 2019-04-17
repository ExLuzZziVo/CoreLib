#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
    public class ItemGroup<T> : IEnumerable<T>, INotifyPropertyChanged
    {
        private string _groupName;

        private ObservableCollection<T> _items;

        [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
        public string GroupName
        {
            get => _groupName;

            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

        [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
        public ObservableCollection<T> Items
        {
            get => _items;

            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Implement INotifyPropertyChanged

        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}