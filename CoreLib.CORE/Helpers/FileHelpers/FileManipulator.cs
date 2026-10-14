#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.FileHelpers
{
    public static class FileManipulator
    {
        /// <summary>
        /// Clears all content of specified file
        /// </summary>
        /// <param name="path">Path to the file</param>
        public static void ClearFile(string path)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
            }
        }

        /// <summary>
        /// Checks if file has one of the specified extensions
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="ext">List of file extensions</param>
        /// <returns>True if file has on the specified extensions</returns>
        public static bool CheckFileExtension(string fileName, IEnumerable<string> ext)
        {
            return ext.Any(e => fileName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Checks if file is used by another process
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>True if file is used by another process</returns>
        public static bool IsInUse(string fileName)
        {
            return new FileInfo(fileName).IsInUse();
        }

        /// <summary>
        /// Replaces characters in a <see cref="fileName"/> that are not valid on the current system with the character provided by <see cref="replaceWith"/>
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="replaceWith">The string to replace invalid characters with</param>
        /// <returns>File name with replaced characters</returns>
        /// <exception cref="ArgumentNullException">Throws if the <see cref="replaceWith"/> is null</exception>
        /// <exception cref="ArgumentException">Throws if the <see cref="replaceWith"/> contains characters that are not valid on the current system</exception>
        public static string ReplaceInvalidCharsInFileName(string fileName, string replaceWith = "")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            if (replaceWith == null)
            {
                throw new ArgumentNullException(nameof(replaceWith));
            }

            var invalidChars = Path.GetInvalidFileNameChars();

            if (replaceWith.IndexOfAny(invalidChars) > -1)
            {
                throw new ArgumentException(
                    "The replacement string contains characters that are not valid on the current system.",
                    nameof(replaceWith));
            }

            return string.Join(replaceWith, fileName.Split(invalidChars));
        }
    }
}
