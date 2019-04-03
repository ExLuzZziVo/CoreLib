namespace UIServiceLib.CORE.CustomObjects
{
    public enum ResultType
    {
        Canceled,
        Completed,
        Exception,
        Error
    }

    public class TaskResult
    {
        public TaskResult(ResultType type, string text)
        {
            Type = type;
            ResultText = text;
        }

        public ResultType Type { get; }
        public string ResultText { get; }
    }
}