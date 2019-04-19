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
        public static void ClearFile(string path)
        {
            using (var fs = File.Open(path, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
            }
        }

        public static bool CheckFileExtension(string fileName, IEnumerable<string> ext)
        {
            return ext.Any(e => fileName.EndsWith(e, StringComparison.OrdinalIgnoreCase));
        }
    }
}