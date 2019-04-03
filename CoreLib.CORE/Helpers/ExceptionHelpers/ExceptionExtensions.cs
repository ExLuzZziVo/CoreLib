#region

using System;
using System.Collections.Generic;

#endregion

namespace UIServiceLib.CORE.Helpers.ExceptionHelpers
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<Exception> GetAllInnerExceptions(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex), "Exception must be specified");
            while (ex.InnerException != null)
            {
                yield return ex.InnerException;
                ex = ex.InnerException;
            }
        }

        public static Exception GetBaseOrLastInnerException(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex), "Exception must be specified");

            while (ex.InnerException != null) ex = ex.InnerException;

            return ex;
        }
    }
}