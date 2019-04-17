#region

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using CoreLib.CORE.Helpers.FileHelpers.Generators;

#endregion

namespace CoreLib.STANDALONE.Helpers.ExceptionHelpers
{
    public class ExceptionManipulator
    {
        public static void ClearErrorLogs(string applicationFolder)
        {
            var logsDir = Path.Combine(applicationFolder, "Logs");
            if (Directory.Exists(logsDir))
                foreach (var file in new DirectoryInfo(logsDir).GetFiles())
                    file.Delete();
        }

        public static void PackErrorLogs(string applicationFolder, string saveErrorLogsArchiveFilePath)
        {
            var logsDir = Path.Combine(applicationFolder, "Logs");
            if (Directory.Exists(logsDir))
            {
                if (File.Exists(saveErrorLogsArchiveFilePath))
                    File.Delete(saveErrorLogsArchiveFilePath);
                ZipFile.CreateFromDirectory(logsDir, saveErrorLogsArchiveFilePath, CompressionLevel.Optimal, false);
            }
        }

        public static void SaveExceptionToFile(string applicationFolder, Exception ex)
        {
            var rootDir = Path.Combine(applicationFolder, "Logs");
            var path = FileNameGenerator.GenerateNewFileName(rootDir, string.Empty, ".log");
            var nowDate = DateTime.Now;
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write($"DATETIME: {nowDate.ToShortDateString()} {nowDate.ToLongTimeString()}\n\n");
                    sw.Write(ex.ToString());
                }
            }
        }
    }
}