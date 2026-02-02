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
    public static class ImageManipulator
    {
#if SYSTEM_DRAWING
        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <returns>Width and Height of image</returns>
        public static ValueTuple<int, int> GetImageWidthAndHeight(string pathToImage)
        {
            using (var image = Image.FromFile(pathToImage))
            {
                return image.GetWidthAndHeight();
            }
        }
#else
        /// <summary>
        /// Gets width and height of the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <returns>Width and Height of image</returns>
        public static ValueTuple<int, int> GetImageWidthAndHeight(string pathToImage)
        {
            using (var image = SKImage.FromEncodedData(pathToImage))
            {
                return image.GetWidthAndHeight();
            }
        }
#endif

#if SYSTEM_DRAWING
        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ResizeImage(string pathToImage, int maxWidth, int maxHeight,
            InterpolationMode interpolationMode = InterpolationMode.Default, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var ms = new MemoryStream())
            {
                ms.Write(imageData, 0, imageData.Length);
                ms.Position = 0;

                using (var image = Image.FromStream(ms))
                {
                    using (var fs = new FileStream(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                               FileMode.Create))
                    {
                        image.Resize(fs, maxWidth, maxHeight, interpolationMode);
                    }
                }
            }
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="scale">Scale</param>
        /// <param name="interpolationMode">Image interpolation mode</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ResizeImage(string pathToImage, double scale,
            InterpolationMode interpolationMode = InterpolationMode.Default, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var ms = new MemoryStream())
            {
                ms.Write(imageData, 0, imageData.Length);
                ms.Position = 0;

                using (var image = Image.FromStream(ms))
                {
                    using (var fs = new FileStream(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                               FileMode.Create))
                    {
                        image.Resize(fs, scale, interpolationMode);
                    }
                }
            }
        }
#else
        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="maxWidth">Maximum resize width</param>
        /// <param name="maxHeight">Maximum resize height</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ResizeImage(string pathToImage, int maxWidth, int maxHeight,
            SKSamplingOptions sampling = default, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var image = SKImage.FromEncodedData(imageData))
            {
                using (var fs = new FileStream(
                           string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                           FileMode.Create))
                {
                    // Encoding quality?
                    image.Resize(fs, maxWidth, maxHeight, sampling, 60);
                }
            }
        }

        /// <summary>
        /// Resizes the image keeping its initial ratio
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="scale">Scale</param>
        /// <param name="sampling">Resized image sampling options</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ResizeImage(string pathToImage, double scale,
            SKSamplingOptions sampling = default, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var image = SKImage.FromEncodedData(imageData))
            {
                using (var fs = new FileStream(
                           string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                           FileMode.Create))
                {
                    // Encoding quality?
                    image.Resize(fs, scale, sampling, 60);
                }
            }
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="quality">Quality</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ChangeImageQuality(string pathToImage, byte quality, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var ms = new MemoryStream())
            {
                ms.Write(imageData, 0, imageData.Length);
                ms.Position = 0;

                using (var image = Image.FromStream(ms))
                {
                    using (var fs = new FileStream(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                               FileMode.Create))
                    {
                        image.ChangeQuality(fs, quality);
                    }
                }
            }
        }
#else
        /// <summary>
        /// Changes quality of the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="quality">Quality</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void ChangeImageQuality(string pathToImage, byte quality, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var image = SKImage.FromEncodedData(imageData))
            {
                using (var fs = new FileStream(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                           FileMode.Create))
                {
                    image.ChangeQuality(fs, quality);
                }
            }
        }
#endif

#if SYSTEM_DRAWING
        /// <summary>
        /// Crops the image to circle
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="background">Color of background of the cropped image</param>
        /// <param name="x">X-position of cropping circle center</param>
        /// <param name="y">Y-position of cropping circle center</param>
        /// <param name="radius">Radius of cropping circle</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void CropImageToCircle(string pathToImage, Color background, float x, float y, float radius,
            string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var ms = new MemoryStream())
            {
                ms.Write(imageData, 0, imageData.Length);
                ms.Position = 0;

                using (var image = Image.FromStream(ms))
                {
                    using (var croppedImage = image.CropToCircle(background, x, y, radius))
                    {
                        croppedImage.Save(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage);
                    }
                }
            }
        }
#else
        /// <summary>
        /// Crops the image to circle
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="background">Color of background of the cropped image</param>
        /// <param name="x">X-position of cropping circle center</param>
        /// <param name="y">Y-position of cropping circle center</param>
        /// <param name="radius">Radius of cropping circle</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void CropImageToCircle(string pathToImage, SKColor background, float x, float y,
            float radius, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var image = SKImage.FromEncodedData(imageData))
            {
                using (var encoderInfo = SKCodec.Create(image.EncodedData))
                {
                    using (var croppedImage = image.CropToCircle(background, x, y, radius))
                    {
                        // Encoding quality?
                        using (var imageToSaveData = croppedImage.Encode(encoderInfo.EncodedFormat, 60))
                        {
                            using (var fs = new FileStream(
                                       string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                                       FileMode.Create))
                            {
                                imageToSaveData.SaveTo(fs);
                            }
                        }
                    }
                }
            }
        }
#endif
#if SYSTEM_DRAWING
        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="rectangleWidth">Width of the rectangle area to cut</param>
        /// <param name="rectangleHeight">Height of the rectangle area to cut</param>
        /// <param name="rectangleXPosition">X-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="rectangleYPosition">Y-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void CutImage(string pathToImage, int rectangleWidth, int rectangleHeight,
            int rectangleXPosition = 0,
            int rectangleYPosition = 0, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var ms = new MemoryStream())
            {
                ms.Write(imageData, 0, imageData.Length);
                ms.Position = 0;

                using (var image = Image.FromStream(ms))
                {
                    using (var cutImage = image.Cut(rectangleWidth, rectangleHeight, rectangleXPosition,
                               rectangleYPosition))
                    {
                        cutImage.Save(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage);
                    }
                }
            }
        }
#else
        /// <summary>
        /// Cuts the image
        /// </summary>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="rectangleWidth">Width of the rectangle area to cut</param>
        /// <param name="rectangleHeight">Height of the rectangle area to cut</param>
        /// <param name="rectangleXPosition">X-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="rectangleYPosition">Y-position of top-left corner of the rectangle. Default value is 0</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        public static void CutImage(string pathToImage, int rectangleWidth, int rectangleHeight,
            int rectangleXPosition = 0,
            int rectangleYPosition = 0, string pathToNewImage = null)
        {
            var imageData = File.ReadAllBytes(pathToImage);

            using (var image = SKImage.FromEncodedData(imageData))
            {
                using (var encoderInfo = SKCodec.Create(image.EncodedData))
                {
                    using (var cutImage = image.Cut(rectangleWidth, rectangleHeight, rectangleXPosition,
                               rectangleYPosition))
                    {
                        // Encoding quality?
                        using (var imageToSaveData = cutImage.Encode(encoderInfo.EncodedFormat, 60))
                        {
                            using (var fs = new FileStream(
                                       string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                                       FileMode.Create))
                            {
                                imageToSaveData.SaveTo(fs);
                            }
                        }
                    }
                }
            }
        }
#endif

#if SYSTEM_DRAWING
        /// <summary>
        /// Changes the quality and size of the target image until it is less than or equal to the specified file size
        /// </summary>
        /// <param name="targetFileSize">Target file size in bytes</param>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        /// <param name="qualityStep">Quality reduction step. Default value: 20</param>
        /// <param name="scaleStep">Scaling down step. Default value: 0.15</param>
        public static void ReduceImageFileSize(long targetFileSize, string pathToImage, string pathToNewImage = null,
            byte qualityStep = 20, double scaleStep = 0.15)
        {
            if (targetFileSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetFileSize));
            }

            if (qualityStep <= 0 || qualityStep >= 100)
            {
                throw new ArgumentOutOfRangeException(nameof(qualityStep));
            }

            if (scaleStep <= 0 || scaleStep >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(scaleStep));
            }

            byte quality = 100;
            var scale = 1 - scaleStep;
            var imageData = File.ReadAllBytes(pathToImage);

            using (var fs = new MemoryStream())
            {
                fs.Write(imageData, 0, imageData.Length);

                var image = Image.FromStream(fs);

                var (imageWidth, imageHeight) = image.GetWidthAndHeight();

                var encoderInfo = ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(enc => enc.FormatID == image.RawFormat.Guid);

                using (var ms = new MemoryStream())
                {
                    ms.Write(imageData, 0, imageData.Length);

                    var qualityEncoderParameters = new EncoderParameters(1)
                    {
                        Param = { [0] = new EncoderParameter(Encoder.Quality, (long)quality) }
                    };

                    while (ms.Length >= targetFileSize)
                    {
                        ms.Position = 0;
                        ms.SetLength(0);
                        ms.Capacity = 0;

                        if (quality <= 60 ||
                            image.RawFormat.Equals(ImageFormat.Png) ||
                            image.RawFormat.Equals(ImageFormat.Bmp) ||
                            image.RawFormat.Equals(ImageFormat.Gif))
                        {
                            imageWidth = Convert.ToInt32(Math.Round(imageWidth * scale, 0, MidpointRounding.AwayFromZero));
                            imageHeight = Convert.ToInt32(Math.Round(imageHeight * scale, 0, MidpointRounding.AwayFromZero));

                            if (imageWidth <= 1 || imageHeight <= 1)
                            {
                                imageWidth = imageWidth < 1 ? 1 : imageWidth;
                                imageHeight = imageHeight < 1 ? 1 : imageHeight;
                            }

                            image.Resize(ms, (int)(imageWidth * scale), (int)(imageHeight * scale),
                                InterpolationMode.Default, qualityEncoderParameters);
                        }
                        else
                        {
                            if (qualityStep >= quality)
                            {
                                quality = 0;
                            }
                            else
                            {
                                quality -= qualityStep;
                            }

                            image.ChangeQuality(ms, quality);

                            qualityEncoderParameters = new EncoderParameters(1)
                            {
                                Param = { [0] = new EncoderParameter(Encoder.Quality, (long)quality) }
                            };
                        }

                        image.Dispose();

                        fs.Position = 0;
                        fs.SetLength(0);
                        fs.Capacity = 0;

                        var msData = ms.ToArray();

                        fs.Write(msData, 0, msData.Length);

                        image = Image.FromStream(fs);

                        if (quality == 0 || imageWidth <= 1 || imageHeight <= 1)
                        {
                            break;
                        }
                    }

                    qualityEncoderParameters.Dispose();

                    File.WriteAllBytes(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                        ms.ToArray());
                }

                image.Dispose();
            }
        }
#else
        /// <summary>
        /// Changes the quality and size of the target image until it is less than or equal to the specified file size
        /// </summary>
        /// <param name="targetFileSize">Target file size in bytes</param>
        /// <param name="pathToImage">Path to the image</param>
        /// <param name="pathToNewImage">The path to save the transformed image. If null, the target image will be replaced. Default value: null</param>
        /// <param name="qualityStep">Quality reduction step. Default value: 20</param>
        /// <param name="scaleStep">Scaling down step. Default value: 0.15</param>
        /// <param name="sampling">Resized image sampling options</param>
        public static void ReduceImageFileSize(long targetFileSize, string pathToImage,
            string pathToNewImage = null,
            byte qualityStep = 20, double scaleStep = 0.15, SKSamplingOptions sampling = default)
        {
            if (targetFileSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetFileSize));
            }

            if (qualityStep <= 0 || qualityStep >= 100)
            {
                throw new ArgumentOutOfRangeException(nameof(qualityStep));
            }

            if (scaleStep <= 0 || scaleStep >= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(scaleStep));
            }

            byte quality = 100;
            var scale = 1 - scaleStep;

            var imageData = File.ReadAllBytes(pathToImage);
            var image = SKImage.FromEncodedData(imageData);
            var imageEncodedData = image.EncodedData;
            var (imageWidth, imageHeight) = image.GetWidthAndHeight();
            var encoderInfo = SKCodec.Create(image.EncodedData);

            while (imageEncodedData.Size >= targetFileSize)
            {
                imageEncodedData.Dispose();

                if (quality <= 60 ||
                    encoderInfo.EncodedFormat == SKEncodedImageFormat.Png ||
                    encoderInfo.EncodedFormat == SKEncodedImageFormat.Bmp ||
                    encoderInfo.EncodedFormat == SKEncodedImageFormat.Gif)
                {
                    imageWidth = Convert.ToInt32(Math.Round(imageWidth * scale, 0, MidpointRounding.AwayFromZero));
                    imageHeight = Convert.ToInt32(Math.Round(imageHeight * scale, 0, MidpointRounding.AwayFromZero));

                    if (imageWidth <= 1 || imageHeight <= 1)
                    {
                        imageWidth = imageWidth < 1 ? 1 : imageWidth;
                        imageHeight = imageHeight < 1 ? 1 : imageHeight;
                    }

                    using (var ms = new MemoryStream())
                    {
                        image.Resize(ms, imageWidth, imageHeight, sampling, quality);

                        ms.Position = 0;

                        imageEncodedData = SKData.Create(ms);
                    }
                }
                else
                {
                    if (qualityStep >= quality)
                    {
                        quality = 0;
                    }
                    else
                    {
                        quality -= qualityStep;
                    }

                    imageEncodedData = image.Encode(encoderInfo.EncodedFormat, quality);
                }

                image.Dispose();

                image = SKImage.FromEncodedData(imageEncodedData);

                imageEncodedData = image.EncodedData;

                if (quality == 0 || imageWidth <= 1 || imageHeight <= 1)
                {
                    break;
                }
            }

            image.Dispose();
            encoderInfo.Dispose();

            File.WriteAllBytes(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                imageEncodedData.ToArray());

            imageEncodedData.Dispose();
        }
#endif

        /// <summary>
        /// Checks if a byte array is an image
        /// </summary>
        /// <param name="data">Data to check</param>
        /// <returns>True if the provided data is an image</returns>
        public static bool IsImage(this byte[] data)
        {
            try
            {
#if SYSTEM_DRAWING
                using (var ms = new MemoryStream(data))
                {
                    using (new Bitmap(ms)) { }
                }
#else
                using (var image = SKImage.FromEncodedData(data))
                {
                    if (image == null)
                    {
                        return false;
                    }
                }
#endif
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
