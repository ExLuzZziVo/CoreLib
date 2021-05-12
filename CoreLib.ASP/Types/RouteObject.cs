#region

using System.Collections.Generic;

#endregion

namespace CoreLib.ASP.Types
{
    /// <summary>
    /// Simple object for storing the route path
    /// </summary>
    public class RouteObject
    {
        public RouteObject(string actionName, string controllerName, Dictionary<string, string> routeValues = null)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues ?? new Dictionary<string, string>();
        }

        public string ControllerName { get; }

        public string ActionName { get; }

        public Dictionary<string, string> RouteValues { get; }
    }
}