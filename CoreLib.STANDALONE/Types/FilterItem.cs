#region

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// An object that is used as a filter item in the view model
    /// </summary>
    public class FilterItem<T> : ViewModelBase
    {
        public bool IsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public T Item
        {
            get => GetValue<T>();
            set => SetValue(value);
        }
    }
}