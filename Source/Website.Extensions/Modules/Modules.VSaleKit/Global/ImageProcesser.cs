using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace Modules.VSaleKit.Global
{
    public class ImageProcesser
    {
        private static MemoryStream DecreaseQualityImage(Stream imgStream, int quality)
        {
            if (quality < 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");
            }
            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            Image img = Image.FromStream(imgStream);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, jpegCodec, encoderParams);
            return ms;
        }

        public static MemoryStream DecreaseImage(Stream imgStream, int capacity)
        {
            int min = 0;
            int max = 100;
            int middle = 50;
            MemoryStream ms = new MemoryStream();
            try
            {
                if ((int)imgStream.Length <= capacity)
                {
                    imgStream.CopyTo(ms);
                    ms.Position = 0;
                    return ms;
                }
                ms = DecreaseQualityImage(imgStream, middle);
                int length = (int)ms.Length;

                if (length > capacity)
                {
                    max = middle;
                }
                else
                {
                    min = middle;
                }
                int count = max - 5;
                while (count >= min)
                {
                    ms = DecreaseQualityImage(imgStream, count);
                    length = (int)ms.Length;
                    if (length <= capacity)
                    {
                        break;
                    }
                    count = count - 5;
                }
                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        public static MemoryStream ResizeImage(Stream imgStream, double scaleFactor)
        {
            Image image = null;
            int newWidth = 0;
            int newHeight = 0;
            try
            {
                image = Image.FromStream(imgStream);
                newWidth = (int)(image.Width * scaleFactor);
                newHeight = (int)(image.Height * scaleFactor);
                Bitmap newImg = new Bitmap(newWidth, newHeight);
                Graphics graphics = Graphics.FromImage(newImg);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                Rectangle rectangle = new Rectangle(0, 0, newWidth, newHeight);
                graphics.DrawImage(image, rectangle);
                MemoryStream ms = new MemoryStream();
                newImg.Save(ms, image.RawFormat);
                ms.Position = 0;
                return ms;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                image?.Dispose();
            }            
        }
        public static MemoryStream ResizeImage(Stream imgStream, int capacity)
        {
            decimal min = 0.0m;
            decimal max = 1.0m;
            decimal middle = 0.5m;
            MemoryStream ms = new MemoryStream();
            try
            {
                if((int)imgStream.Length <= capacity)
                {
                    imgStream.CopyTo(ms);
                    ms.Position = 0;
                    return ms;
                }                
                ms = ResizeImage(imgStream, (double)middle);
                int length = (int)ms.Length;
                
                if(length > capacity)
                {
                    max = middle;
                }
                else
                {
                    min = middle;
                }
                decimal count = max - 0.05m;
                while (count>= min)
                {
                    ms = ResizeImage(imgStream, (double)count);
                    length = (int)ms.Length;
                    if(length<=capacity)
                    {
                        break;
                    }
                    count = count - 0.05m;
                }
                return ms;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                ;          
            }            
        }
    }
}
