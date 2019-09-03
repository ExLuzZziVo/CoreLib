#region

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CoreLib.ASP.Helpers.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static string IsActive(this IHtmlHelper html, string area, string controller, string action,
            object routeValues = null)
        {
            return IsActive(html, controller, action, routeValues) != null &&
                   string.Equals(area, (string) html.ViewContext.RouteData.Values["area"],
                       StringComparison.InvariantCultureIgnoreCase)
                ? "active"
                : null;
        }

        public static string IsActive(this IHtmlHelper html, string controller, string action,
            object routeValues = null)
        {
            var routeData = html.ViewContext.RouteData;

            if (!CheckRouteData(html, routeData, routeValues))
                return null;

            var routeAction = (string) routeData.Values["action"];
            var routeController = (string) routeData.Values["controller"];

            var returnActive =
                string.Equals(controller, routeController, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(action, routeAction, StringComparison.InvariantCultureIgnoreCase);

            return returnActive ? "active" : null;
        }

        public static string IsPageActive(this IHtmlHelper html, string area, string page,
            bool startsWithComparison = false, object routeValues = null)
        {
            return IsPageActive(html, page, startsWithComparison, routeValues) != null &&
                   string.Equals(area, (string) html.ViewContext.RouteData.Values["area"],
                       StringComparison.InvariantCultureIgnoreCase)
                ? "active"
                : null;
        }

        public static string IsPageActive(this IHtmlHelper html, string page, bool startsWithComparison = false,
            object routeValues = null)
        {
            var routeData = html.ViewContext.RouteData;

            if (!CheckRouteData(html, routeData, routeValues))
                return null;
            var routePage = (string) routeData.Values["page"];
            if (routePage == null)
                return null;
            if (!startsWithComparison)
                return string.Equals(page, routePage,
                    StringComparison.InvariantCultureIgnoreCase)
                    ? "active"
                    : null;
            return routePage.ToUpperInvariant().StartsWith(page.ToUpperInvariant())
                ? "active"
                : null;
        }

        private static bool CheckRouteData(this IHtmlHelper html, RouteData routeData, object routeValues = null)
        {
            if (routeValues != null)
            {
                var routeValuesDictionary = new RouteValueDictionary(routeValues);
                foreach (var rv in routeValuesDictionary)
                {
                    if (routeData.Values.ContainsKey(rv.Key) &&
                        routeData.Values[rv.Key].ToString() == rv.Value.ToString())
                        continue;
                    if (!html.ViewContext.HttpContext.Request.Query.ContainsKey(rv.Key))
                        return false;
                    if (html.ViewContext.HttpContext.Request.Query[rv.Key].ToString() != rv.Value.ToString())
                        return false;
                }
            }

            return true;
        }
    }
}