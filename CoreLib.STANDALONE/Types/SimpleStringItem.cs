#region

using System;

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// A simple object that is used for binding to simple objects (e.g. int, enum, string, etc.) that fires property changed event when it gets changed
    /// </summary>
    public class SimpleObservableObject<T> : ViewModelBase where T : IComparable, IConvertible
    {
        public SimpleObservableObject(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => GetValue<T>();
            set => SetValue(value);
        }
    }
}