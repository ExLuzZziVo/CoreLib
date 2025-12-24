#region

using System;
using System.Text.RegularExpressions;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Rewrite;

#endregion

namespace CoreLib.ASP.Rules
{
    /// <summary>
    /// Rule that rewrites urls to support SPAs(e.g. Vue using history mode)
    /// </summary>

    // https://stackoverflow.com/a/57850877
    public class SpaHistoryRewriteRule: IRule
    {
        private readonly Regex _ignore;
        private readonly string _rewriteTo;

        /// <summary>
        /// Rule that rewrites urls to support SPAs(e.g. Vue using history mode)
        /// </summary>
        /// <param name="ignore">Regular expression for paths to ignore. Default value: "^/api"</param>
        /// <param name="rewriteTo">Url to rewrite links to. Default value: "/"</param>
        public SpaHistoryRewriteRule(string ignore = "^/api", string rewriteTo = "/")
        {
            if (ignore.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(ignore));
            }

            _ignore = new Regex(ignore);
            _rewriteTo = rewriteTo ?? "/";
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var path = request.Path.Value;

            if (string.Equals(path, _rewriteTo, StringComparison.InvariantCultureIgnoreCase) ||
                path == null ||
                _ignore.IsMatch(path))
            {
                return;
            }

            var fileInfo = context.StaticFileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                request.Path = _rewriteTo;
                context.Result = RuleResult.SkipRemainingRules;
            }
        }
    }
}
