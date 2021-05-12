#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        /// <param name="fileName">Filename</param>
        /// <param name="ext">List of file extensions</param>
        /// <returns>True if file has on the specified extensions</returns>
        public static bool CheckFileExtension(string fileName, IEnumerable<string> ext)
        {
            return ext.Any(e => fileName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
        }
    }
}