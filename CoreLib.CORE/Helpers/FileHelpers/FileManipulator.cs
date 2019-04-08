#region

using System.IO;

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
    }
}