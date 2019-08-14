#region

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CoreLib.ASP.Helpers.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper html, string area, string control, string action,
            object routeValues = null)
        {
            var routeArea = (string) html.ViewContext.RouteData.Values["area"];
            return IsActive(html, control, action, routeValues) == null &&
                   !string.Equals(area, routeArea, StringComparison.InvariantCultureIgnoreCase)
                ? null
                : "active";
        }

        public static string IsActive(this IHtmlHelper html, string control, string action, object routeValues = null)
        {
            var routeData = html.ViewContext.RouteData;

            if (routeValues != null)
            {
                var routeValueDictionary = new RouteValueDictionary(routeValues);
                foreach (var rv in routeValueDictionary)
                    if (!html.ViewContext.HttpContext.Request.Query.ContainsKey(rv.Key))
                        return null;
                    else if (html.ViewContext.HttpContext.Request.Query[rv.Key].ToString() != rv.Value.ToString())
                        return null;
            }

            var routeAction = (string) routeData.Values["action"];
            var routeControl = (string) routeData.Values["controller"];

            var returnActive = string.Equals(control, routeControl, StringComparison.InvariantCultureIgnoreCase) &&
                               string.Equals(action, routeAction, StringComparison.InvariantCultureIgnoreCase);

            return returnActive ? "active" : null;
        }
    }
}