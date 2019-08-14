using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLib.CORE.Helpers.ImageHelpers
{
    public static class ImageManipulator
    {
        public static Tuple<int, int> GetImageWidthAndHeight(string pathToImage)
        {
            return System.Drawing.Image.FromFile(pathToImage).GetImageWidthAndHeight();
        }
    }
}
