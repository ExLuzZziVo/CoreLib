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
        /// <summary>
        /// Physical path to the 'wwwroot' directory
        /// </summary>
        public static readonly string RootDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot";

        private readonly ILogger<UploadHelper> _logger;

        public UploadHelper(ILogger<UploadHelper> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Parses an uploaded text file (e.g. csv file) line by line
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <param name="encoding">Encoding of the uploaded file</param>
        /// <param name="fileSizeLimit">Maximum size of the uploaded file. Default value: 2MB</param>
        /// <returns>A task that represents the asynchronous parsing of the uploaded file line by line. If the operation is successful, it returns a sequence of strings</returns>
        public async Task<IEnumerable<string>> UploadAndParseTextFileByLine(IFormFile file, Encoding encoding,
            long fileSizeLimit = 2048000)
        {
            try
            {
                if (!(file.Length > 0) && file.Length > fileSizeLimit)
                {
                    throw new ValidationException(
                        string.Format(Resources.ValidationStrings.ResourceManager.GetString("UploadFileSizeError"),
                            fileSizeLimit.ToFileSize(IntExtensions.SizeUnits.MB)));
                }

                var result = new List<string>();

                await using (var s = file.OpenReadStream())
                {
                    s.Position = 0;

                    using (var sr = new StreamReader(s, encoding))
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

        /// <summary>
        /// Reads content from uploaded file
        /// </summary>
        /// <param name="file">Uploaded file</param>
        /// <param name="encoding">Encoding of the uploaded file</param>
        /// <param name="fileSizeLimit">Maximum size of the uploaded file. Default value: 2MB</param>
        /// <returns>A task that represents the asynchronous reading the contents of an uploaded file. If the operation is successful, it returns the contents of the uploaded file as a string</returns>
        public async Task<string> UploadTextFile(IFormFile file, Encoding encoding, long fileSizeLimit = 2048000)
        {
            try
            {
                if (!(file.Length > 0) && file.Length > fileSizeLimit)
                {
                    throw new ValidationException(string.Format(
                        Resources.ValidationStrings.ResourceManager.GetString("UploadFileSizeError"),
                        fileSizeLimit.ToFileSize(IntExtensions.SizeUnits.MB)));
                }

                string result;

                await using (var s = file.OpenReadStream())
                {
                    s.Position = 0;

                    using (var sr = new StreamReader(s, encoding))
                    {
                        result = await sr.ReadToEndAsync();
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

        /// <summary>
        /// Checks and saves the uploaded image to specified path
        /// </summary>
        /// <param name="imageFile">Uploaded image</param>
        /// <param name="saveImageFilePath">Full path to save the uploaded image</param>
        /// <param name="imageFileSizeLimit">Maximum size of the uploaded image. Default value: 2MB</param>
        /// <returns>A task that represents the asynchronous checking and saving of the uploaded image. If the operation is successful, it returns <see cref="OkObjectResult"/>, otherwise <see cref="BadRequestObjectResult"/></returns>
        public async Task<IActionResult> UploadImage(IFormFile imageFile, string saveImageFilePath,
            long imageFileSizeLimit = 2048000)
        {
            try
            {
                if (!(imageFile.Length > 0) && imageFile.Length > imageFileSizeLimit)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format(Resources.ValidationStrings.ResourceManager.GetString("UploadFileSizeError"),
                            imageFileSizeLimit.ToFileSize(IntExtensions.SizeUnits.MB)));
                }

                if (!imageFile.IsImage())
                {
                    throw new FormatException(
                        Resources.ValidationStrings.ResourceManager.GetString("UploadFileFormatError"));
                }

                var fullSaveFilePath = RootDirectory + saveImageFilePath;
                var fullSaveDirectoryPath = Path.GetDirectoryName(fullSaveFilePath);

                if (!Directory.Exists(fullSaveDirectoryPath))
                {
                    Directory.CreateDirectory(fullSaveDirectoryPath);
                }

                await using (var stream = new FileStream(fullSaveFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                string message;

                if (ex is ArgumentOutOfRangeException || ex is FormatException)
                {
                    message = ex.Message;
                }
                else
                {
                    message = ValidationStrings.ResourceManager.GetString("SomethingWentWrong");
                }

                _logger.Log(LogLevel.Error, ex, ex.Message);

                return new BadRequestObjectResult(message);
            }

            return new OkObjectResult(new {saveImageFilePath});
        }
    }
}