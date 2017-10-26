using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Simple.ImageResizer;

namespace MagicCube.Common
{
    public class ImageProcessor
    {
        public static BitmapSource Crop(BitmapSource image, Rect croppingRectangle)
        {

            Rect imageRectangle = new Rect(new Size(image.PixelWidth, image.PixelHeight));

            if (croppingRectangle.Width < 1 || croppingRectangle.Height < 1)
            {
                throw new ArgumentOutOfRangeException("croppingRectangle",
                    "CroppingRectangle's dimensions can't be less than 1x1.");
            }
            if (croppingRectangle.X < 0)
                croppingRectangle.X = 0;
            if (croppingRectangle.Y < 0)
                croppingRectangle.Y = 0;
            if (croppingRectangle.X + croppingRectangle.Width > 400)
            {
                croppingRectangle.Width = imageRectangle.Width - croppingRectangle.X;
            }
            if (croppingRectangle.Y + croppingRectangle.Height > 400)
            {
                croppingRectangle.Height = imageRectangle.Height - croppingRectangle.Y;
            }
            croppingRectangle.Location = new Point(Math.Floor(croppingRectangle.X), Math.Floor(croppingRectangle.Y));
            croppingRectangle.Size = new Size(Math.Floor(croppingRectangle.Width), Math.Floor(croppingRectangle.Height));

            if (!imageRectangle.Contains(croppingRectangle))
            {
                throw new ArgumentOutOfRangeException("croppingRectangle",
                    "CroppingRectangle must be within the boundaries of the image.");
            }

            CroppedBitmap result = new CroppedBitmap(image,
                new Int32Rect((int)croppingRectangle.X, (int)croppingRectangle.Y, (int)croppingRectangle.Width,
                    (int)croppingRectangle.Height));

            return result;
        }

        public static BitmapSource Resize(BitmapSource image, int newWidth, int newHeight)
        {
            
           
            double scale = Math.Min((double)newWidth / image.PixelWidth, (double)newHeight / image.PixelHeight);

            TransformedBitmap transformedBitmap = new TransformedBitmap(image,
                new ScaleTransform(scale, scale));

            return transformedBitmap;
        }

        public static BitmapSource Rotate(BitmapSource image, int angle)
        {


            //if (angle < ImageProcessor.MinRotationAngle || angle > ImageProcessor.MaxRotationAngle)
            //{
            //    throw new ArgumentOutOfRangeException("angle",
            //        string.Format("Angle must be between {0} and {1}.", ImageProcessor.MinRotationAngle,
            //            ImageProcessor.MaxRotationAngle));
            //}

            BitmapSource result = null;

            if (angle == -180 || angle == -90 || angle == 0 || angle == 90 || angle == 180)
            {
                // Orthogonal rotation
                result = new TransformedBitmap(image, new RotateTransform(angle));
            }
            //else
            //{
            //    result = ImageProcessor.RotateOnAnyAngle(image, angle, adjustImageSize);
            //}

            return result;
        }

        /// <summary>Gets the bitmap source from the file with specified path.</summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The bitmap source.</returns>
        public static BitmapSource GetBitmapSourceFromFile(string filePath)
        {
            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();

            //bitmapImage.UriSource = new Uri(filePath, UriKind.Absolute);
            Simple.ImageResizer.ImageResizer imageResizer = new Simple.ImageResizer.ImageResizer(filePath);
            byte[] tempByte = imageResizer.Resize(184, 184, true, ImageEncoding.Png);
            Stream stream = new MemoryStream(tempByte);
            bitmapImage.StreamSource = stream;


            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

            bitmapImage.EndInit();

            BitmapSource bs = ImageProcessor.Resize(bitmapImage, 184, 184);

            return bs;
        }

        /// <summary>Saves the specified image to file with specified path.</summary>
        /// <param name="image">The image.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveImageToFile(BitmapSource image, string filePath)
        {


            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(filePath)
                .ToLower())
                {
                    case ".bmp":
                        {
                            encoder = new BmpBitmapEncoder();

                            break;
                        }

                    case ".gif":
                        {
                            encoder = new GifBitmapEncoder();

                            break;
                        }

                    case ".jpg":
                    case ".jpeg":
                        {
                            encoder = new JpegBitmapEncoder();

                            break;
                        }

                    case ".png":
                        {
                            encoder = new PngBitmapEncoder();

                            break;
                        }

                    case ".tif":
                    case ".tiff":
                        {
                            encoder = new TiffBitmapEncoder();

                            break;
                        }

                    default:
                        {
                            encoder = new PngBitmapEncoder();
                            break;
                        }
                }

                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        public static MemoryStream SaveImageToStream(BitmapSource image)
        {
            MemoryStream str = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(str);
            str.Seek(0, SeekOrigin.Begin);
            return str;
        }

           
           
    }
}
