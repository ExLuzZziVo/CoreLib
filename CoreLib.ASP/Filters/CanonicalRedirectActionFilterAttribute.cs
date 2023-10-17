#region

using System;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// An action filter attribute that permanently redirects requested page to its canonical name
    /// </summary>
    public class CanonicalRedirectActionFilterAttribute : ActionFilterAttribute
    {
        private readonly bool _queryStringToLowerCase;

        /// <summary>
        /// An action filter attribute that permanently redirects requested page to its canonical name
        /// </summary>
        /// <param name="queryStringToLowerCase">Always lowercase query strings. Default value: false</param>
        public CanonicalRedirectActionFilterAttribute(bool queryStringToLowerCase = false)
        {
            _queryStringToLowerCase = queryStringToLowerCase;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var urlHelper = context.HttpContext.RequestServices.GetService<IUrlHelperFactory>()?.GetUrlHelper(context);

            if (urlHelper != null)
            {
                var canonicalUrlCheckResult = CheckAndGetCanonicalUrl(urlHelper, _queryStringToLowerCase);

                if (!canonicalUrlCheckResult.Item1)
                {
                    context.Result = new RedirectResult(canonicalUrlCheckResult.Item2, true);
                }
            }
        }

        /// <summary>
        /// Checks if the specified url is canonical. If not, a canonical URL is also generated
        /// </summary>
        /// <param name="urlHelper">Url helper</param>
        /// <param name="queryStringToLowerCase">Lowercase query string. Default value: false</param>
        /// <returns>True if the specified url is canonical. Also always returns canonical url.</returns>
        public static (bool, string) CheckAndGetCanonicalUrl(IUrlHelper urlHelper, bool queryStringToLowerCase = false)
        {
            var context = urlHelper.ActionContext;
            var url = urlHelper.RouteUrl(context.RouteData.Values);
            var requestUrl = context.HttpContext.Request.Path.Value;
            var queryString = context.HttpContext.Request.QueryString.Value;
            var urlHasParams = false;
            var isQueryStringValid = true;

            if (!queryString.IsNullOrEmptyOrWhiteSpace())
            {
                if (context.ActionDescriptor is CompiledPageActionDescriptor cpad)
                {
                    // Get handler name from query string, because RouteData doesn't contain it
                    context.HttpContext.Request.Query.TryGetValue("handler", out var handlerName);

                    urlHasParams = cpad.HandlerMethods.Any(hmd =>
                        string.Equals(hmd.Name, handlerName, StringComparison.OrdinalIgnoreCase) &&
                        string.Equals(hmd.HttpMethod, context.HttpContext.Request.Method,
                            StringComparison.OrdinalIgnoreCase) && hmd.Parameters.Count > 0);
                }
                else
                {
                    urlHasParams = context.ActionDescriptor.Parameters.Count > 0;
                }

                if (!urlHasParams)
                {
                    isQueryStringValid = false;
                }
                else if (queryStringToLowerCase)
                {
                    var tempQueryString = queryString.ToLowerInvariant();

                    if (queryString != tempQueryString)
                    {
                        queryString = tempQueryString;
                        isQueryStringValid = false;
                    }
                }
            }

            var resultUrl = url + (urlHasParams ? queryString : string.Empty);

            if (url != requestUrl || !isQueryStringValid)
            {
                return (false, resultUrl);
            }

            return (true, resultUrl);
        }
    }
}