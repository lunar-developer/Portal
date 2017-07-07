using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace Website.Library.Business
{
    public static class ImageBusiness
    {
        public static string GenerateQRCode(
            string data,
            int pixelsPerModule = 1,
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.L)
        {
            byte[] byteImage;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, eccLevel, true, true))
            using (QRCode qrCode = new QRCode(qrCodeData))
            using (Bitmap bitmap = qrCode.GetGraphic(pixelsPerModule, Color.Black, Color.White, null))
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Jpeg);
                byteImage = ms.ToArray();
            }
            return Convert.ToBase64String(byteImage);
        }

        public static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            float ratioHeight = (float) maxHeight / image.Height;
            if (ratioHeight >= 1)
            {
                return image;
            }

            int width = (int) (image.Width * ratioHeight);
            int height = (int) (image.Height * ratioHeight);
            Bitmap newImage = new Bitmap(width, height);
            using (Graphics graphic = Graphics.FromImage(newImage))
            {
                graphic.DrawImage(image, 0, 0, width, height);
            }
            return newImage;
        }

        public static Image ConvertBase64StringToImage(string base64String)
        {
            Image image;
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                memoryStream.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(memoryStream, true);
            }
            return image;
        }

        public static string ConvertImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, format);
                byte[] imageBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static string GetBase64StringFromStream(Stream stream, int maxWidth = 0, int maxHeight = 0)
        {
            using (Bitmap inputImage = new Bitmap(Image.FromStream(stream)))
            using (Image image =
                ScaleImage(inputImage, Math.Max(inputImage.Width, maxWidth), Math.Max(inputImage.Height, maxHeight)))
            {
                return ConvertImageToBase64(image, ImageFormat.Jpeg);
            }
        }

        public static string GetImageFromFile(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                return filePath;
            }

            using (Image image = Image.FromFile(filePath))
            {
                return ConvertImageToBase64(image, image.RawFormat);
            }
        }
    }
}