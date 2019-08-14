#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using CoreLib.CORE.CustomObjects;
using CoreLib.CORE.Helpers.ExceptionHelpers;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.FileHelpers
{
    public static class FileManipulator
    {
        public static void ClearFile(string path)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
            }
        }

        public static void PackFileToZip(string filePath, string zipPath = null)
        {
            if (filePath.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(filePath));
            if (zipPath.IsNullOrEmptyOrWhiteSpace())
                zipPath = System.IO.Path.ChangeExtension(filePath, "zip");
            using (var archive = ZipFile.Open(zipPath, File.Exists(zipPath)?ZipArchiveMode.Update:ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }
        }

        public static void PackFilesToZip(IEnumerable<string> filePaths, string zipPath)
        {
            if (!filePaths.Any())
                throw new ArgumentOutOfRangeException(nameof(filePaths));
            if (zipPath.IsNullOrEmptyOrWhiteSpace())
                zipPath = System.IO.Path.ChangeExtension(filePaths.First(),"zip");
            using (var archive = ZipFile.Open(zipPath, File.Exists(zipPath)?ZipArchiveMode.Update:ZipArchiveMode.Create))
            {
                foreach (var fPath in filePaths)
                {
                    archive.CreateEntryFromFile(fPath,Path.GetFileName(fPath));
                }
            }
        }

        public static bool CheckFileExtension(string fileName, IEnumerable<string> ext)
        {
            return ext.Any(e => fileName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
        }

        public static TaskResult SaveStringArrayToFile(IEnumerable<string> list, string fileName, Encoding encoding)
        {
            try
            {
                File.WriteAllLines(fileName, list, encoding);
                return new TaskResult(ResultType.Completed, string.Empty);
            }
            catch (Exception ex)
            {
                return new TaskResult(ResultType.Error, ex.GetBaseOrLastInnerException().Message);
            }
        }
    }
}