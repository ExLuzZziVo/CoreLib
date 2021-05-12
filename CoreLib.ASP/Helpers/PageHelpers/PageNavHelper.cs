#region

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace CoreLib.ASP.Helpers.PageHelpers
{
    public static class PageNavHelper
    {
        /// <summary>
        /// Helper that checks if the specified page is currently displayed using <see cref="ViewContext"/>. Is used to add the 'active' css class to target html element
        /// </summary>
        /// <param name="viewContext">Current view context</param>
        /// <param name="page">Page name</param>
        /// <param name="viewDataKey">ViewData key where the name of the current page is stored</param>
        /// <returns>Returns 'active' if the specified page is currently displayed. Otherwise returns null</returns>
        public static string PageNavClass(ViewContext viewContext, string page, string viewDataKey)
        {
            var activePage = viewContext.ViewData[viewDataKey] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}