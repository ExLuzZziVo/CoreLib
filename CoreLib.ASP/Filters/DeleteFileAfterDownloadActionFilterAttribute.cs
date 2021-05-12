#region

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace CoreLib.ASP.Filters
{
    /// <summary>
    /// An action filter attribute that deletes the file after downloading
    /// </summary>
    public class DeleteFileAfterDownloadActionFilterAttribute : ActionFilterAttribute
    {
        public override async void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is PhysicalFileResult filePathResult)
            {
                await context.HttpContext.Response.Body.FlushAsync();
                File.Delete(filePathResult.FileName);
            }
        }
    }
}