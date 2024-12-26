#region

using System;
using System.IO;
#if SYSTEM_DRAWING
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
#else
using SkiaSharp;
#endif

#endregion

namespace CoreLib.CORE.Helpers.ImageHelpers
{
    public static class ImageExtensions
    {
#if SYSTEM_DRAWING
        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <param name="encoderParameters">Image encoder parameters</param>
        /// <returns>Size of the new image in bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="maxWidth"/> or <paramref name="maxHeight"/> is less than 1</exception>
        public static long Resize(this Image img, Stream stream, int maxWidth, int maxHeight,
            InterpolationMode interpolationMode = InterpolationMode.Default, EncoderParameters encoderParameters = null)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth));
            }

            if (maxHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHeight));
            }

            var ratioX = (double)maxWidth / img.Width;
            var ratioY = (double)maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var currentPosition = stream.Position;

            using (var bitmap = new Bitmap(newWidth, newHeight, img.PixelFormat))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.InterpolationMode = interpolationMode;
                    graphics.DrawImage(img, 0, 0, newWidth, newHeight);

                    // Need to save initial image format
                    var encoderInfo = ImageCodecInfo.GetImageEncoders()
                        .FirstOrDefault(enc => enc.FormatID == img.RawFormat.Guid);

                    bitmap.Save(stream, encoderInfo, encoderParameters);
                }
            }

            return stream.Position - currentPosition;
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <returns>Resized image</returns>
        public static Image Resize(this Image img, int maxWidth, int maxHeight,
            InterpolationMode interpolationMode = InterpolationMode.Default)
        {
            // The MemoryStream MUST NOT be disposed while the associated Image is in use
            var ms = new MemoryStream();
            img.Resize(ms, maxWidth, maxHeight, interpolationMode);
            ms.Position = 0;

            return Image.FromStream(ms);
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="scale">Scale</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <param name="encoderParameters">Image encoder parameters</param>
        /// <returns>Size of the new image in bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="scale"/> is less or equals 0</exception>
        public static long Resize(this Image img, Stream stream, double scale,
            InterpolationMode interpolationMode = InterpolationMode.Default, EncoderParameters encoderParameters = null)
        {
            if (scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale));
            }

            return img.Resize(stream, (int)(img.Width * scale), (int)(img.Height * scale), interpolationMode,
                encoderParameters);
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

            return img.Resize((int)(img.Width * scale), (int)(img.Height * scale), interpolationMode);
        }
#else
        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <param name="quality">Resized image quality</param>
        /// <returns>Size of the new image in bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="maxWidth"/> or <paramref name="maxHeight"/> is less than 1</exception>
        public static long Resize(this SKImage img, Stream stream, int maxWidth, int maxHeight,
            SKSamplingOptions sampling = default, byte quality = 100)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth));
            }

            if (maxHeight < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHeight));
            }

            var ratioX = (double)maxWidth / img.Width;
            var ratioY = (double)maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var imageInfo = img.Info;
            imageInfo.Width = newWidth;
            imageInfo.Height = newHeight;

            var currentPosition = stream.Position;

            using (var newImage = SKImage.Create(imageInfo))
            {
                img.ScalePixels(newImage.PeekPixels(), sampling);

                using (var encoderInfo = SKCodec.Create(img.EncodedData))
                {
                    using (var skData = newImage.Encode(encoderInfo.EncodedFormat, quality))
                    {
                        skData.SaveTo(stream);
                    }
                }
            }

            return stream.Position - currentPosition;
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <returns>Resized image</returns>
        public static SKImage Resize(this SKImage img, int maxWidth, int maxHeight,
            SKSamplingOptions sampling = default)
        {
            using (var ms = new MemoryStream())
            {
                img.Resize(ms, maxWidth, maxHeight, sampling);

                ms.Position = 0;

                return SKImage.FromEncodedData(ms);
            }
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="scale">Scale</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <param name="quality">Resized image quality</param>
        /// <returns>Size of the new image in bytes</returns>-
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="scale"/> is less or equals 0</exception>
        public static long Resize(this SKImage img, Stream stream, double scale,
            SKSamplingOptions sampling = default, byte quality = 100)
        {
            if (scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale));
            }

            return img.Resize(stream, (int)(img.Width * scale), (int)(img.Height * scale), sampling, quality);
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="img">Image to resize</param>
        /// <param name="scale">Scale</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <returns>Resized image</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="scale"/> is less or equals 0</exception>
        public static SKImage Resize(this SKImage img, double scale,
            SKSamplingOptions sampling = default)
        {
            if (scale <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scale));
            }

            return img.Resize((int)(img.Width * scale), (int)(img.Height * scale), sampling);
        }
#endif
#if SYSTEM_DRAWING
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

            using (var qualityEncoderParameters = new EncoderParameters(1)
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, (long)quality) }
            })
            {
                var currentPosition = stream.Position;

                img.Save(stream, encoderInfo, qualityEncoderParameters);

                return stream.Position - currentPosition;
            }
        }
#else
        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="img">Image to change quality</param>
        /// <param name="stream">Stream where the new image will be saved</param>
        /// <param name="quality">Quality</param>
        /// <returns>Size of the new image in bytes</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws if <paramref name="quality"/> is more than 100</exception>
        public static long ChangeQuality(this SKImage img, Stream stream, byte quality)
        {
            if (quality > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(quality));
            }

            var currentPosition = stream.Position;

            using (var encoderInfo = SKCodec.Create(img.EncodedData))
            {
                using (var skData = img.Encode(encoderInfo.EncodedFormat, quality))
                {
                    skData.SaveTo(stream);
                }
            }

            return stream.Position - currentPosition;
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="img">Image to change quality</param>
        /// <param name="quality">Quality</param>
        /// <returns>Image with changed quality</returns>
        public static Image ChangeQuality(this Image img, byte quality)
        {
            // The MemoryStream MUST NOT be disposed while the associated Image is in use
            var ms = new MemoryStream();
            img.ChangeQuality(ms, quality);
            ms.Position = 0;

            return Image.FromStream(ms);
        }
#else
        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="img">Image to change quality</param>
        /// <param name="quality">Quality</param>
        /// <returns>Image with changed quality</returns>
        public static SKImage ChangeQuality(this SKImage img, byte quality)
        {
            using (var ms = new MemoryStream())
            {
                img.ChangeQuality(ms, quality);

                ms.Position = 0;

                return SKImage.FromEncodedData(ms);
            }
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Crops the image to circle
        /// </summary>
        /// <param name="img">Image to crop</param>
        /// <param name="background">Color of background of the cropped image</param>
        /// <param name="x">X-position of cropping circle center</param>
        /// <param name="y">Y-position of cropping circle center</param>
        /// <param name="radius">Radius of cropping circle</param>
        /// <returns>Cropped image</returns>
        public static Image CropToCircle(this Image img, Color background, float x, float y, float radius)
        {
            if (x < 0 || x > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if (radius < 1 || radius + x > img.Width || x - radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            if (radius < 1 || radius + y > img.Height || y - radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            var newImageSize = (int)Math.Round(radius) * 2;

            Image newImage = new Bitmap(newImageSize, newImageSize, img.PixelFormat);

            using (var graphics = Graphics.FromImage(newImage))
            {
                using (Brush br = new SolidBrush(background))
                {
                    graphics.FillRectangle(br, 0, 0, newImage.Width, newImage.Height);
                }

                using (var path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, newImageSize, newImageSize);
                    graphics.SetClip(path);

                    graphics.DrawImage(img, Rectangle.FromLTRB(0, 0, newImageSize, newImageSize),
                        new Rectangle((int)(x - radius), (int)(y - radius), newImageSize, newImageSize),
                        GraphicsUnit.Pixel);
                }
            }

            return newImage;
        }
#else
        /// <summary>
        /// Crops the image to circle
        /// </summary>
        /// <param name="img">Image to crop</param>
        /// <param name="background">Color of background of the cropped image</param>
        /// <param name="x">X-position of cropping circle center</param>
        /// <param name="y">Y-position of cropping circle center</param>
        /// <param name="radius">Radius of cropping circle</param>
        /// <returns>Cropped image</returns>
        public static SKImage CropToCircle(this SKImage img, SKColor background, float x, float y, float radius)
        {
            if (x < 0 || x > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (y < 0 || y > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            if (radius < 1 || radius + x > img.Width || x - radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            if (radius < 1 || radius + y > img.Height || y - radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            var newImageSize = (int)Math.Round(radius) * 2;

            using (var newImage = new SKBitmap(newImageSize, newImageSize))
            {
                using (var canvas = new SKCanvas(newImage))
                {
                    canvas.Clear(background);

                    using (var path = new SKPath())
                    {
                        path.AddCircle(radius, radius, radius);
                        canvas.ClipPath(path);

                        canvas.DrawImage(img, SKRect.Create(x - radius, y - radius, newImageSize, newImageSize),
                            SKRect.Create(newImageSize, newImageSize));
                    }
                }

                return SKImage.FromBitmap(newImage);
            }
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="img">Image to cut</param>
        /// <param name="cropArea">Rectangle area of the image to be cut</param>
        /// <returns>Cut image</returns>
        public static Image Cut(this Image img, Rectangle cropArea)
        {
            using (var bmpImage = new Bitmap(img))
            {
                return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            }
        }

        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="img">Image to cut</param>
        /// <param name="rectangleWidth">Width of the rectangle area to cut</param>
        /// <param name="rectangleHeight">Height of the rectangle area to cut</param>
        /// <param name="rectangleXPosition">X-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="rectangleYPosition">Y-position of top-left corner of the rectangle. Default value is 0</param>
        /// <returns>Cut image</returns>
        public static Image Cut(this Image img, int rectangleWidth, int rectangleHeight, int rectangleXPosition = 0,
            int rectangleYPosition = 0)
        {
            if (rectangleXPosition < 0 || rectangleXPosition > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleXPosition));
            }

            if (rectangleYPosition < 0 || rectangleYPosition > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleYPosition));
            }

            if (rectangleWidth < 1 || rectangleWidth + rectangleXPosition > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleWidth));
            }

            if (rectangleHeight < 1 || rectangleHeight + rectangleYPosition > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleHeight));
            }

            return img.Cut(new Rectangle(rectangleXPosition, rectangleYPosition, rectangleWidth, rectangleHeight));
        }
#else
        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="img">Image to cut</param>
        /// <param name="rectangleWidth">Width of the rectangle area to cut</param>
        /// <param name="rectangleHeight">Height of the rectangle area to cut</param>
        /// <param name="rectangleXPosition">X-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="rectangleYPosition">Y-position of top-left corner of the rectangle. Default value is 0</param>
        /// <returns>Cut image</returns>
        public static SKImage Cut(this SKImage img, int rectangleWidth, int rectangleHeight, int rectangleXPosition = 0,
            int rectangleYPosition = 0)
        {
            if (rectangleXPosition < 0 || rectangleXPosition > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleXPosition));
            }

            if (rectangleYPosition < 0 || rectangleYPosition > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleYPosition));
            }

            if (rectangleWidth < 1 || rectangleWidth + rectangleXPosition > img.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleWidth));
            }

            if (rectangleHeight < 1 || rectangleHeight + rectangleYPosition > img.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(rectangleHeight));
            }

            using (var bmpImage = new SKBitmap(rectangleWidth, rectangleHeight))
            {
                using (var canvas = new SKCanvas(bmpImage))
                {
                    canvas.DrawImage(img,
                        SKRect.Create(rectangleXPosition, rectangleYPosition, rectangleWidth, rectangleHeight),
                        SKRect.Create(0, 0, rectangleWidth, rectangleHeight));
                }

                return SKImage.FromBitmap(bmpImage);
            }
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="img">Target image</param>
        /// <returns>Width and Height of image</returns>
        public static ValueTuple<int, int> GetWidthAndHeight(this Image img)
        {
            return new ValueTuple<int, int>(img.Width, img.Height);
        }
#else
        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="img">Target image</param>
        /// <returns>Width and Height of image</returns>
        public static ValueTuple<int, int> GetWidthAndHeight(this SKImage img)
        {
            return new ValueTuple<int, int>(img.Width, img.Height);
        }
#endif
    }
}
