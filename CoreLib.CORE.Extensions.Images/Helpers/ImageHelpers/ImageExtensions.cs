#region

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

#endregion

namespace CoreLib.CORE.Helpers.ImageHelpers
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <returns>Resized image</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="maxWidth"/> or <paramref name="maxHeight"/> is less than 1</exception>
        public static Image Resize(this Image img, int maxWidth, int maxHeight,
            InterpolationMode interpolationMode = InterpolationMode.Default)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth));
            }

            if (maxHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHeight));
            }

            var ratioX = (double) maxWidth / img.Width;
            var ratioY = (double) maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int) (img.Width * ratio);
            var newHeight = (int) (img.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = interpolationMode;
                graphics.DrawImage(img, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="scale">Scale</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <returns>Resized image</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="scale"/> is less or equals 0</exception>
        public static Image Resize(this Image img, double scale,
            InterpolationMode interpolationMode = InterpolationMode.Default)
        {
            if (scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale));
            }

            return img.Resize((int) (img.Width * scale), (int) (img.Height * scale));
        }

        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="img">Image to change quality</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="quality">Quality</param>
        /// <returns>Size of the new image in bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="quality"/> is more than 100</exception>
        public static long ChangeQuality(this Image img, Stream stream, byte quality)
        {
            if (quality > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(quality));
            }

            var encoderInfo = ImageCodecInfo.GetImageEncoders()
                .FirstOrDefault(enc => enc.FormatID == img.RawFormat.Guid);

            var qualityEncoderParameters = new EncoderParameters(1)
            {
                Param = {[0] = new EncoderParameter(Encoder.Quality, (long) quality)}
            };

            var currentPosition = stream.Position;

            img.Save(stream, encoderInfo, qualityEncoderParameters);

            return stream.Position - currentPosition;
        }

        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="img">Image to change quality</param>
        /// <param name="quality">Quality</param>
        /// <returns>Image with changed quality</returns>
        public static Image ChangeQuality(this Image img, byte quality)
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            using (var fs = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                img.ChangeQuality(fs, quality);
            }

            return Image.FromFile(tempFilePath);
        }

        /// <summary>
        /// Crops the image to circle
        /// </summary>
        /// <param name="img">Image to crop</param>
        /// <param name="background">Color of background of the cropped image</param>
        /// <param name="x">X-position of cropping circle center</param>
        /// <param name="y">Y-position of cropping circle center</param>
        /// <param name="radiusX">X-radius of cropping circle</param>
        /// <param name="radiusY">Y-radius of cropping circle</param>
        /// <returns>Cropped image</returns>
        public static Image CropToCircle(this Image img, Color background, float x, float y, float radiusX,
            float radiusY)
        {
            Image newImage = new Bitmap(img.Width, img.Height, img.PixelFormat);

            using (var graphics = Graphics.FromImage(newImage))
            {
                using (Brush br = new SolidBrush(background))
                {
                    graphics.FillRectangle(br, 0, 0, newImage.Width, newImage.Height);
                }

                var path = new GraphicsPath();
                path.AddEllipse(x, y, radiusX, radiusY);
                graphics.SetClip(path);
                graphics.DrawImage(img, 0, 0);
            }

            return newImage;
        }

        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="img">Image to cut</param>
        /// <param name="cropArea">Rectangle area of the image to be cut</param>
        /// <returns>Cut image</returns>
        public static Image Cut(this Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);

            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="img">Image to cut</param>
        /// <param name="rectangleWidth">Width of the rectangle area to cut</param>
        /// <param name="rectangleHeight">Height of the rectangle area to cut</param>
        /// <param name="rectangleXPostion">X-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="rectangleYPosition">Y-position of top-left corner of the rectangle. Default value is 0</param>
        /// <returns>Cut image</returns>
        public static Image Cut(this Image img, int rectangleWidth, int rectangleHeight, int rectangleXPostion = 0,
            int rectangleYPosition = 0)
        {
            if (rectangleWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleWidth));
            }

            if (rectangleHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleHeight));
            }

            return img.Cut(new Rectangle(rectangleXPostion, rectangleYPosition, rectangleWidth, rectangleHeight));
        }

        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="img">Target image</param>
        /// <returns>Width and Height of image</returns>
        public static Tuple<int, int> GetWidthAndHeight(this Image img)
        {
            return new Tuple<int, int>(img.Width, img.Height);
        }
    }
}