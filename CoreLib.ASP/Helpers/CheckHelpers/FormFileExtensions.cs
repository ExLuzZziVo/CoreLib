#region

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

#endregion

namespace CoreLib.ASP.Helpers.CheckHelpers
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this IFormFile postedFile)
        {
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                postedFile.ContentType.ToLower() != "image/jpeg" &&
                postedFile.ContentType.ToLower() != "image/pjpeg" &&
                postedFile.ContentType.ToLower() != "image/gif" &&
                postedFile.ContentType.ToLower() != "image/x-png" &&
                postedFile.ContentType.ToLower() != "image/png")
                return false;
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
                return false;
            try
            {
                if (!postedFile.OpenReadStream().CanRead) return false;
                if (postedFile.Length < ImageMinimumBytes) return false;
                var buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                var content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content,
                    @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    return false;
                using (new Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
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