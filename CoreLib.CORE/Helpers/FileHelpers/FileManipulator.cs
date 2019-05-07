#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CoreLib.CORE.CustomObjects;
using CoreLib.CORE.Helpers.ExceptionHelpers;

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