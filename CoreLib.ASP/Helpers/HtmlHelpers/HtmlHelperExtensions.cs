﻿#region

using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CoreLib.ASP.Helpers.HtmlHelpers
{
    public static partial class HtmlHelperExtensions
    {
        /// <summary>
        /// Helper that checks if the specified controller action is currently displayed. Is used to add the 'active' css class to target html element
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="area">Action area</param>
        /// <param name="controller">Action controller</param>
        /// <param name="action">Action name</param>
        /// <param name="routeValues">Action route values. Default value: null</param>
        /// <returns>Returns 'active' if the specified controller action is currently displayed. Otherwise returns null</returns>
        public static string IsActive(this IHtmlHelper html, string area, string controller, string action,
            object routeValues = null)
        {
            return IsActive(html, controller, action, routeValues) != null &&
                   string.Equals(area, (string)html.ViewContext.RouteData.Values["area"],
                       StringComparison.InvariantCultureIgnoreCase)
                ? "active"
                : null;
        }

        /// <summary>
        /// Helper that checks if the specified controller action is currently displayed. Is used to add the 'active' css class to target html element
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="controller">Action controller</param>
        /// <param name="action">Action name</param>
        /// <param name="routeValues">Action route values. Default value: null</param>
        /// <returns>Returns 'active' if the specified controller action is currently displayed. Otherwise returns null</returns>
        public static string IsActive(this IHtmlHelper html, string controller, string action,
            object routeValues = null)
        {
            var routeData = html.ViewContext.RouteData;

            if (!CheckRouteData(html, routeData, routeValues))
            {
                return null;
            }

            var routeAction = (string)routeData.Values["action"];
            var routeController = (string)routeData.Values["controller"];

            var returnActive =
                string.Equals(controller, routeController, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(action, routeAction, StringComparison.InvariantCultureIgnoreCase);

            return returnActive ? "active" : null;
        }

        /// <summary>
        /// Helper that checks if the specified page is currently displayed. Is used to add the 'active' css class to target html element
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="area">Page area</param>
        /// <param name="page">Page name</param>
        /// <param name="startsWithComparison">Compare only page names, ignoring <paramref name="routeValues"/>. Default value: false</param>
        /// <param name="routeValues">Page route values. Default value: null</param>
        /// <returns>Returns 'active' if the specified page is currently displayed. Otherwise returns null</returns>
        public static string IsPageActive(this IHtmlHelper html, string area, string page,
            bool startsWithComparison = false, object routeValues = null)
        {
            return IsPageActive(html, page, startsWithComparison, routeValues) != null &&
                   string.Equals(area, (string)html.ViewContext.RouteData.Values["area"],
                       StringComparison.InvariantCultureIgnoreCase)
                ? "active"
                : null;
        }

        /// <summary>
        /// Helper that checks if the specified page is currently displayed. Is used to add the 'active' css class to target html element
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="page">Page name</param>
        /// <param name="startsWithComparison">Compare only page names, ignoring <paramref name="routeValues"/>. Default value: false</param>
        /// <param name="routeValues">Page route values. Default value: null</param>
        /// <returns>Returns 'active' if the specified page is currently displayed. Otherwise returns null</returns>
        public static string IsPageActive(this IHtmlHelper html, string page, bool startsWithComparison = false,
            object routeValues = null)
        {
            var routeData = html.ViewContext.RouteData;

            if (!CheckRouteData(html, routeData, routeValues))
            {
                return null;
            }

            var routePage = (string)routeData.Values["page"];

            if (routePage == null)
            {
                return null;
            }

            if (!startsWithComparison)
            {
                return string.Equals(page, routePage,
                    StringComparison.InvariantCultureIgnoreCase)
                    ? "active"
                    : null;
            }

            return routePage.StartsWith(page, StringComparison.InvariantCultureIgnoreCase)
                ? "active"
                : null;
        }

        /// <summary>
        /// Checks if the provided route values and route data values are equal
        /// </summary>
        /// <param name="html">Html helper</param>
        /// <param name="routeData">Route data</param>
        /// <param name="routeValues">Route values</param>
        /// <returns>True if provided <paramref name="routeValues"/> and <see cref="routeData"/> values are equal</returns>
        private static bool CheckRouteData(this IHtmlHelper html, RouteData routeData, object routeValues = null)
        {
            if (routeValues != null)
            {
                var routeValuesDictionary = new RouteValueDictionary(routeValues);

                foreach (var rv in routeValuesDictionary)
                {
                    if (routeData.Values.ContainsKey(rv.Key) &&
                        routeData.Values[rv.Key].ToString() == rv.Value.ToString())
                    {
                        continue;
                    }

                    if (!html.ViewContext.HttpContext.Request.Query.ContainsKey(rv.Key))
                    {
                        return false;
                    }

                    if (html.ViewContext.HttpContext.Request.Query[rv.Key].ToString() != rv.Value.ToString())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
