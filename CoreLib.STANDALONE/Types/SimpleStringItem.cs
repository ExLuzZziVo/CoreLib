#region

#endregion

namespace CoreLib.STANDALONE.Types
{
    /// <summary>
    /// A simple object that is used for binding to a string that fires property changed event when it gets changed
    /// </summary>
    public class SimpleStringObject : ViewModelBase
    {
        public SimpleStringObject(string simpleString)
        {
            SimpleString = simpleString;
        }

        public string SimpleString
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}