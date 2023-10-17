#region

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Extensions.Compression.Helpers.FileHelpers
{
    public static class FileManipulator_Compression
    {
        /// <summary>
        /// Packs specified file to zip archive
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <param name="zipPath">Path to new zip archive. If null, it creates zip archive in the directory where the specified file is located</param>
        /// <returns>Path to new zip archive</returns>
        public static string PackFileToZip(string filePath, string zipPath = null)
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
                archive.CreateEntryFromFileSafely(filePath);
            }

            return zipPath;
        }

        /// <summary>
        /// Packs specified files to zip archive
        /// </summary>
        /// <param name="filePaths">Paths to files</param>
        /// <param name="zipPath">Path to new zip archive. If null, it creates zip archive in the directory where the first specified file is located</param>
        /// <returns>Path to new zip archive</returns>
        public static string PackFilesToZip(IEnumerable<string> filePaths, string zipPath)
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
                    archive.CreateEntryFromFileSafely(fPath);
                }
            }

            return zipPath;
        }

        /// <summary>
        /// Packs specified directory to zip archive
        /// </summary>
        /// <param name="directoryPath">Paths to directory</param>
        /// <param name="zipPath">Path to new zip archive. If null, it creates zip archive in the directory where the specified directory is located</param>
        /// <returns>Path to new zip archive</returns>
        public static string PackDirectoryToZip(string directoryPath, string zipPath = null)
        {
            if (directoryPath.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (zipPath.IsNullOrEmptyOrWhiteSpace())
            {
                zipPath = $"{directoryPath.TrimEnd('\\', '/')}.zip";
            }

            using (var archive = ZipFile.Open(zipPath,
                       File.Exists(zipPath) ? ZipArchiveMode.Update : ZipArchiveMode.Create))
            {
                foreach (var fPath in Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories))
                {
                    archive.CreateEntryFromFileSafely(fPath, directoryPath);
                }
            }

            return zipPath;
        }

        /// <summary>
        /// Creates a zip archive entry even if the file is busy
        /// </summary>
        /// <param name="archive">Zip archive</param>
        /// <param name="filePath">Path to the target file</param>
        /// <param name="fileRootDirectoryPath">Path to the root directory of the target file. Is used only to pack a directory with subdirectories</param>
        private static void CreateEntryFromFileSafely(this ZipArchive archive, string filePath,
            string fileRootDirectoryPath = null)
        {
            var file = new FileInfo(filePath);
            var entryFullName = file.Name;

            if (!fileRootDirectoryPath.IsNullOrEmptyOrWhiteSpace())
            {
                entryFullName = filePath.Replace(fileRootDirectoryPath, string.Empty).TrimStart('\\', '/');
            }

            if (archive.Mode == ZipArchiveMode.Update &&
                archive.Entries.Any(e => e.FullName == entryFullName && e.Length == file.Length))
            {
                return;
            }

            var entry = archive.CreateEntry(entryFullName);
            entry.LastWriteTime = file.LastWriteTime;

            using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var es = entry.Open())
                {
                    fs.CopyTo(es);
                }
            }
        }
    }
}