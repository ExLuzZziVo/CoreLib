#region

using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

#endregion

namespace CoreLib.ASP.Rules
{
    /// <summary>
    /// Rule that rewrites urls to lowercase
    /// </summary>

    //https://stackoverflow.com/questions/48469342/redirect-asp-net-core-2-0-urls-to-lowercase
    public class RedirectToLowerCaseRule : IRule
    {
        private readonly int _statusCode;

        /// <summary>
        /// Rule that rewrites urls to lowercase
        /// </summary>
        /// <param name="statusCode">Redirect status code</param>
        public RedirectToLowerCaseRule(int statusCode)
        {
            _statusCode = statusCode;
        }

        public void ApplyRule(RewriteContext context)
        {
            var c = context.HttpContext.GetRouteData();
            var request = context.HttpContext.Request;

            if (!request.Scheme.Any(char.IsUpper)
                && !request.Host.Value.Any(char.IsUpper)
                && !request.PathBase.Value.Any(char.IsUpper)
                && !request.Path.Value.Any(char.IsUpper))
            {
                context.Result = RuleResult.ContinueRules;

                return;
            }

            var newUrl = UriHelper.BuildAbsolute(request.Scheme.ToLowerInvariant(),
                new HostString(request.Host.Value.ToLowerInvariant()), request.PathBase.Value.ToLowerInvariant(),
                request.Path.Value.ToLowerInvariant(), request.QueryString);

            var response = context.HttpContext.Response;
            response.StatusCode = _statusCode;
            response.Headers[HeaderNames.Location] = newUrl;
            context.Result = RuleResult.EndResponse;
        }
    }
}