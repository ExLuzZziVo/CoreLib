#region

using System.IO;

#endregion

namespace CoreLib.CORE.Helpers.FileHelpers
{
    public static class FileExtensions
    {
        /// <summary>
        /// Checks if provided file is used by another process
        /// </summary>
        /// <param name="file">Target file</param>
        /// <returns>True if provided file is used by another process</returns>
        public static bool IsInUse(this FileInfo file)
        {
            try
            {
                using (file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) { }

                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}