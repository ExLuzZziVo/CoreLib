#region

using System;
using System.Threading.Tasks;
using CoreLib.CORE.CustomObjects;

#endregion

namespace CoreLib.CORE.Helpers.TaskHelpers
{
    public static class TaskManipulator
    {
        public static Task<TaskResult> SimpleActionTask(Action action)
        {
            return new Task<TaskResult>(() =>
            {
                try
                {
                    action();
                    return new TaskResult(ResultType.Completed, string.Empty);
                }
                catch (Exception ex)
                {
                    return new TaskResult(ResultType.Error, ex.Message);
                }
            });
        }
    }
}