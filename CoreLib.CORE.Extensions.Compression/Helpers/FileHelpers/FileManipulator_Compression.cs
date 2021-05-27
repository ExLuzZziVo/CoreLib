using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;

namespace CoreLib.CORE.Extensions.Compression.Helpers.FileHelpers
{
    public static class FileManipulator_Compression
    {
        /// <summary>
        /// Packs specified file to zip archive
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="zipPath">Path to new zip archive. If null, it creates zip archive in the directory where the specified file is located</param>
        public static void PackFileToZip(string filePath, string zipPath = null)
        {
            if (filePath.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (zipPath.IsNullOrEmptyOrWhiteSpace())
            {
                zipPath = Path.ChangeExtension(filePath, "zip");
            }

            using (var archive = ZipFile.Open(zipPath,
                File.Exists(zipPath) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
            }
        }

        /// <summary>
        /// Packs specified files to zip archive
        /// </summary>
        /// <param name="filePaths">Paths to files</param>
        /// <param name="zipPath">Path to new zip archive. If null, it creates zip archive in the directory where the first specified file is located</param>
        public static void PackFilesToZip(IEnumerable<string> filePaths, string zipPath)
        {
            if (!filePaths.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(filePaths));
            }

            if (zipPath.IsNullOrEmptyOrWhiteSpace())
            {
                zipPath = Path.ChangeExtension(filePaths.First(), "zip");
            }

            using (var archive = ZipFile.Open(zipPath,
                File.Exists(zipPath) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
            {
                foreach (var fPath in filePaths)
                {
                    archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                }
            }
        }
    }
}