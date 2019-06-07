#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CoreLib.ASP.Helpers.CheckHelpers;
using CoreLib.CORE.Helpers.IntHelpers;
using CoreLib.CORE.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#endregion

namespace CoreLib.ASP.Helpers.FileHelpers
{
    public class UploadHelper
    {

        private readonly ILogger<UploadHelper> _logger;

        public UploadHelper(ILogger<UploadHelper> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<string>> UploadAndParseTextFileByLine(IFormFile file, Encoding encoding, long fileSizeLimit = 2048000)
        {
            try
            {
                if (!(file.Length > 0) && file.Length > fileSizeLimit)
                    throw new ValidationException(
                        string.Format(CommonStrings.ResourceManager.GetString("UploadFileSizeError"),
                            fileSizeLimit.ToFileSize(IntExtensions.SizeUnits.MB)));
                var result = new List<string>();
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Position = 0;
                    using (var sr = new StreamReader(ms, encoding))
                    {
                        while (!sr.EndOfStream)
                        {
                            result.Add(await sr.ReadLineAsync());
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, ex.Message);
                throw;
            }
        }

        public async Task<ActionResult> UploadImage(IFormFile imageFile, string saveImageFilePath,
            long imageFileSizeLimit = 2048000)
        {
            try
            {
                if (!(imageFile.Length > 0) && imageFile.Length > imageFileSizeLimit)
                    throw new ArgumentOutOfRangeException(
                        string.Format(CommonStrings.ResourceManager.GetString("UploadFileSizeError"),
                            imageFileSizeLimit.ToFileSize(IntExtensions.SizeUnits.MB)));
                if (!imageFile.IsImage())
                    throw new FormatException(CommonStrings.ResourceManager.GetString("UploadFileFormatError"));
                using (var stream = new FileStream($"{Directory.GetCurrentDirectory()}\\wwwroot{saveImageFilePath}",
                    FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                string message;
                if (ex is ArgumentOutOfRangeException || ex is FormatException)
                    message = ex.Message;
                else
                    message = CommonStrings.ResourceManager.GetString("SomethingWentWrong");
                _logger.Log(LogLevel.Error, ex, ex.Message);
                return new BadRequestObjectResult(message);
            }

            return new OkObjectResult(new {saveImageFilePath});
        }
    }
}