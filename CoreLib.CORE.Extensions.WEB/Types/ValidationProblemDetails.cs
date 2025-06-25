#region

using System.Collections.Generic;

#endregion

namespace CoreLib.CORE.Types
{
    /// <summary>
    /// Validation Problem Details for ASP.NET HTTP APIs
    /// </summary>
    public class ValidationProblemDetails: ProblemDetails
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
