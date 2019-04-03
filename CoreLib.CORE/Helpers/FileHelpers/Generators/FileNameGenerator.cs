#region

using System;
using System.IO;
using CoreLib.CORE.Helpers.StringHelpers;

#endregion

namespace CoreLib.CORE.Helpers.FileHelpers.Generators
{
    public static class FileNameGenerator
    {
        public static string GenerateNewFolderName(string pathToNewFolderRoot, string folderName, bool createFolder)
        {
            if (pathToNewFolderRoot.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(pathToNewFolderRoot), "Root folder must be specified!");
            if (!Directory.Exists(
                $"{pathToNewFolderRoot}"))
                Directory.CreateDirectory(
                    $"{pathToNewFolderRoot}");
            var fullPath = Path.Combine(pathToNewFolderRoot, folderName.IsNullOrEmptyOrWhiteSpace() ? "[0]" : folderName);
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
            if(createFolder)
                Directory.CreateDirectory(fullPath);
            return fullPath;
        }

        public static string GenerateNewFileName(string pathToNewFileRoot, string fileName, string fileExtension)
        {
            if (pathToNewFileRoot.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentNullException(nameof(pathToNewFileRoot), "Root folder must be specified!");
            if (!Directory.Exists(
                $"{pathToNewFileRoot}"))
                Directory.CreateDirectory(
                    $"{pathToNewFileRoot}");
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