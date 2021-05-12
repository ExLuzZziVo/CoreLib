#region

using System;
using System.Drawing;

#endregion

namespace CoreLib.CORE.Helpers.ImageHelpers
{
    public static class ImageManipulator
    {
        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <returns>Width and Height of image</returns>
        public static Tuple<int, int> GetImageWidthAndHeight(string pathToImage)
        {
            return Image.FromFile(pathToImage).GetWidthAndHeight();
        }
    }
}