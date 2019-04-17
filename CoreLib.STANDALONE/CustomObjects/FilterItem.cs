#region

using System;
using System.ComponentModel;
using System.Reflection;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
    public class FilterItem<T> : INotifyPropertyChanged
    {
        private bool _enabled;
        private T _item;

        [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }

        [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
        public T Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        #region Implement INotifyPropertyChanged
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}