#region

using System;
using System.IO;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.FileHelpers.Generators
{
    public static class FileNameGenerator
    {
        /// <summary>
        /// Generates new directory with specified name. If directory exists, it adds [i] postfix to its name
        /// </summary>
        /// <param name="pathToNewFolderRoot">Root directory</param>
        /// <param name="folderName">Folder name</param>
        /// <param name="createFolder">Create directory with new name or just return full path of it</param>
        /// <returns>Full path with new directory name</returns>
        public static string GenerateNewFolderName(string pathToNewFolderRoot, string folderName,
            bool createFolder = true)
        {
            if (pathToNewFolderRoot.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(pathToNewFolderRoot));
            }

            if (!Directory.Exists(
                $"{pathToNewFolderRoot}"))
            {
                Directory.CreateDirectory(
                    $"{pathToNewFolderRoot}");
            }

            var fullPath = Path.Combine(pathToNewFolderRoot,
                folderName.IsNullOrEmptyOrWhiteSpace() ? "[0]" : folderName);

            if (Directory.Exists(fullPath))
            {
                var i = 1;
                fullPath = Path.Combine($"{pathToNewFolderRoot}", $"{folderName}[{i}]");

                while (Directory.Exists(fullPath))
                {
                    i++;
                    fullPath = Path.Combine($"{pathToNewFolderRoot}", $"{folderName}[{i}]");
                }
            }

            if (createFolder)
            {
                Directory.CreateDirectory(fullPath);
            }

            return fullPath;
        }

        /// <summary>
        /// Generates new filename. If file with specified name exists, it adds [i] postfix to its name
        /// </summary>
        /// <param name="pathToNewFileRoot">Root directory</param>
        /// <param name="fileName">Filename</param>
        /// <param name="fileExtension">File extension</param>
        /// <returns>Full path with new filename</returns>
        public static string GenerateNewFileName(string pathToNewFileRoot, string fileName, string fileExtension)
        {
            if (pathToNewFileRoot.IsNullOrEmptyOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(pathToNewFileRoot));
            }

            if (!Directory.Exists(
                $"{pathToNewFileRoot}"))
            {
                Directory.CreateDirectory(
                    $"{pathToNewFileRoot}");
            }

            var fullPath = Path.Combine(pathToNewFileRoot,
                $"{(fileName.IsNullOrEmptyOrWhiteSpace() ? "[0]" : fileName)}{(fileExtension.IsNullOrEmptyOrWhiteSpace() ? string.Empty : $".{fileExtension}")}");

            if (File.Exists(fullPath))
            {
                var i = 1;

                fullPath = Path.Combine(pathToNewFileRoot,
                    $"{fileName}[{i}]{(fileExtension.IsNullOrEmptyOrWhiteSpace() ? string.Empty : $".{fileExtension}")}");

                while (File.Exists(fullPath))
                {
                    i++;

                    fullPath = Path.Combine(pathToNewFileRoot,
                        $"{fileName}[{i}]{(fileExtension.IsNullOrEmptyOrWhiteSpace() ? string.Empty : $".{fileExtension}")}");
                }

                return fullPath;
            }

            return fullPath;
        }
    }
}