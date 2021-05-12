#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// An object that is used in grouped list of items with group names in the view model
    /// </summary>
    public class ItemGroup<T> : ViewModelBase, IEnumerable<T>
    {
        public string GroupName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ObservableCollection<T> Items
        {
            get => GetValue<ObservableCollection<T>>();
            set => SetValue(value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}