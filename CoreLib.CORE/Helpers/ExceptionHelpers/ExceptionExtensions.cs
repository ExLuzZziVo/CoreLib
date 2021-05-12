#region

using System;
using System.Collections.Generic;

#endregion

namespace CoreLib.CORE.Helpers.ExceptionHelpers
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets all inner exceptions from provided <see cref="Exception"/>
        /// </summary>
        /// <param name="ex">Exception to process</param>
        /// <returns>List of inner exceptions</returns>
        public static IEnumerable<Exception> GetAllInnerExceptions(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            while (ex.InnerException != null)
            {
                yield return ex.InnerException;

                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// Gets base or last inner exception from provided <see cref="Exception"/>
        /// </summary>
        /// <param name="ex">Exception to process</param>
        /// <returns>Base or last inner exception</returns>
        public static Exception GetBaseOrLastInnerException(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}