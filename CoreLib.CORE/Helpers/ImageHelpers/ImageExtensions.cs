using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace CoreLib.CORE.Helpers.ImageHelpers
{
    public static class ImageExtensions
    {
        public static Image ResizeImageBySize(this Image img, double maxWidth, double maxHeight)
        {
            var ratioX = maxWidth / img.Width;
            var ratioY = maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(img, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static Image ResizeImageByScale(this Image img, double scale)
        {
            return img.ResizeImageBySize(img.Width * scale, img.Height * scale);
        }

        public static Image CropImageToCircle(this Image srcImage, Color backGround, float x, float y, float radiusX,
            float radiusY)
        {
            Image dstImage = new Bitmap(srcImage.Width, srcImage.Height, srcImage.PixelFormat);
            var g = Graphics.FromImage(dstImage);
            using (Brush br = new SolidBrush(backGround))
            {
                g.FillRectangle(br, 0, 0, dstImage.Width, dstImage.Height);
            }
            var path = new GraphicsPath();
            path.AddEllipse(x, y, radiusX, radiusY);
            g.SetClip(path);
            g.DrawImage(srcImage, 0, 0);
            return dstImage;
        }

        public static Image CutImage(this Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public static Tuple<int, int> GetImageWidthAndHeight(this Image image)
        {
            return new Tuple<int, int>(image.Width, image.Height);
        }
    }
}
