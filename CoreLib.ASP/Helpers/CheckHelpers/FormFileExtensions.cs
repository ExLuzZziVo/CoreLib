#region

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    // https://stackoverflow.com/questions/11063900/determine-if-uploaded-file-is-image-any-format-on-mvc/14587821#14587821
    public static class FormFileExtensions
    {
        private const int ImageMinimumBytes = 512;

        /// <summary>
        /// Validates if the uploaded file is an image
        /// </summary>
        /// <param name="postedFile">Uploaded file</param>
        /// <returns>True if uploaded file is an image</returns>
        public static bool IsImage(this IFormFile postedFile)
        {
            if (!postedFile.ContentType.Equals("image/jpg", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/jpeg", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/pjpeg", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/gif", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/x-png", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/png", StringComparison.CurrentCultureIgnoreCase) &&
                !postedFile.ContentType.Equals("image/webp", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            if (!Path.GetExtension(postedFile.FileName).Equals(".jpg"
, StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(postedFile.FileName).Equals(".png"
, StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(postedFile.FileName).Equals(".gif"
, StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(postedFile.FileName).Equals(".jpeg"
, StringComparison.CurrentCultureIgnoreCase)
                && !Path.GetExtension(postedFile.FileName).Equals(".webp", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                var buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                var content = Encoding.UTF8.GetString(buffer);

                if (Regex.IsMatch(content,
                    @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
#if SYSTEM_DRAWING
                using (new System.Drawing.Bitmap(postedFile.OpenReadStream())) { }
#else
                using (var image = SkiaSharp.SKImage.FromEncodedData(postedFile.OpenReadStream()))
                {
                    if(image == null)
                    {
                        return false;
                    }
                }
#endif
            }
            catch
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        }
    }
}
