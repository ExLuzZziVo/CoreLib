#region

using System;
using System.ComponentModel;
using System.Reflection;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
    public class SimpleStringObject : INotifyPropertyChanged
    {
        private string _simpleString;

        public SimpleStringObject(string simpleString)
        {
            SimpleString = simpleString;
        }
        [Obfuscation(Exclude = false, Feature = "merge:Internalize=false")]
        public string SimpleString
        {
            get => _simpleString;
            set
            {
                _simpleString = value;
                OnPropertyChanged(nameof(SimpleString));
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