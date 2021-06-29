#region

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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
            var scale = 1d;
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
                        Param = {[0] = new EncoderParameter(Encoder.Quality, (long) quality)}
                    };

                    while (ms.Length >= targetFileSize)
                    {
                        ms.Position = 0;
                        ms.SetLength(0);
                        ms.Capacity = 0;

                        if (quality <= 60 && scale >= 0.1)
                        {
                            if (scaleStep >= scale)
                            {
                                scale = 0.1;
                            }
                            else
                            {
                                scale -= scaleStep;
                            }

                            image.Resize((int) (imageWidth * scale), (int) (imageHeight * scale))
                                .Save(ms, encoderInfo, qualityEncoderParameters);
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
                                Param = {[0] = new EncoderParameter(Encoder.Quality, (long) quality)}
                            };
                        }

                        image.Dispose();

                        fs.Position = 0;
                        fs.SetLength(0);
                        fs.Capacity = 0;

                        var msData = ms.ToArray();

                        fs.Write(msData, 0, msData.Length);

                        image = Image.FromStream(fs);

                        if (quality == 0 && scale <= 0.1)
                        {
                            break;
                        }
                    }

                    File.WriteAllBytes(string.IsNullOrEmpty(pathToNewImage) ? pathToImage : pathToNewImage,
                        ms.ToArray());
                }

                image.Dispose();
            }
        }
    }
}