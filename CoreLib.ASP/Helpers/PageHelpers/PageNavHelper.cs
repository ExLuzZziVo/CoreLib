#region

using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

#endregion

namespace CoreLib.ASP.Helpers.PageHelpers
{
    public static class PageNavHelper
    {
        public static string PageNavClass(ViewContext viewContext, string page, string viewDataKey)
        {
            var activePage = viewContext.ViewData[viewDataKey] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}