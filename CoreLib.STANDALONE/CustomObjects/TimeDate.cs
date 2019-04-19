#region

using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace CoreLib.STANDALONE.CustomObjects
{
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    [Serializable]
    public class TimeDate : INotifyPropertyChanged
    {

        private DateTime _date;
        private int _hours;
        private int _minutes;

        public TimeDate(DateTime value)
        {
            _hours = value.Hour;
            _minutes = value.Minute;
            _date = value.Date;
            PropertyChanged = null;
        }

        public DateTime GetDateTime => new DateTime(_date.Year, _date.Month, _date.Day, _hours, _minutes, 0);

        [DataMember]
        public int Hours
        {
            get => _hours;
            set
            {
                if (value > 23)
                    value = 23;
                if (value < 0)
                    value = 0;
                _hours = value;
                OnPropertyChanged(nameof(_hours));
            }
        }

        [DataMember]
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value > 59)
                    value = 59;
                if (value < 0)
                    value = 0;
                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        [DataMember]
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string ToLongDateString()
        {
            return GetDateTime.ToLongDateString();
        }

        public string ToShortDateString()
        {
            return GetDateTime.ToShortDateString();
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