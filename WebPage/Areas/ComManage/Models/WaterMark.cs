using Common;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WebPage.Areas.ComManage.Controllers
{
    public class WaterMark
    {
        public static void AddImageSignPic(string imgPath, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            if (!File.Exists(Utils.GetMapPath(imgPath)))
            {
                return;
            }
            byte[] buffer = File.ReadAllBytes(Utils.GetMapPath(imgPath));
            Image image = Image.FromStream(new MemoryStream(buffer));
            filename = Utils.GetMapPath(filename);
            if (!watermarkFilename.StartsWith("/"))
            {
                watermarkFilename = "/" + watermarkFilename;
            }
            watermarkFilename = Utils.GetMapPath(watermarkFilename);
            if (!File.Exists(watermarkFilename))
            {
                return;
            }
            Graphics graphics = Graphics.FromImage(image);
            Image image2 = new Bitmap(watermarkFilename);
            if (image2.Height >= image.Height || image2.Width >= image.Width)
            {
                return;
            }
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap[] map = new ColorMap[]
            {
                new ColorMap
                {
                    OldColor = Color.FromArgb(255, 0, 255, 0),
                    NewColor = Color.FromArgb(0, 0, 0, 0)
                }
            };
            imageAttributes.SetRemapTable(map, ColorAdjustType.Bitmap);
            float num = 0.5f;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
            {
                num = (float)watermarkTransparency / 10f;
            }
            float[][] array = new float[5][];
            float[][] arg_111_0 = array;
            int arg_111_1 = 0;
            float[] array2 = new float[5];
            array2[0] = 1f;
            arg_111_0[arg_111_1] = array2;
            float[][] arg_128_0 = array;
            int arg_128_1 = 1;
            float[] array3 = new float[5];
            array3[1] = 1f;
            arg_128_0[arg_128_1] = array3;
            float[][] arg_13F_0 = array;
            int arg_13F_1 = 2;
            float[] array4 = new float[5];
            array4[2] = 1f;
            arg_13F_0[arg_13F_1] = array4;
            float[][] arg_153_0 = array;
            int arg_153_1 = 3;
            float[] array5 = new float[5];
            array5[3] = num;
            arg_153_0[arg_153_1] = array5;
            array[4] = new float[]
            {
                0f,
                0f,
                0f,
                0f,
                1f
            };
            float[][] newColorMatrix = array;
            ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
            imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            int x = 0;
            int y = 0;
            switch (watermarkStatus)
            {
                case 1:
                    x = (int)((float)image.Width * 0.01f);
                    y = (int)((float)image.Height * 0.01f);
                    break;
                case 2:
                    x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
                    y = (int)((float)image.Height * 0.01f);
                    break;
                case 3:
                    x = (int)((float)image.Width * 0.99f - (float)image2.Width);
                    y = (int)((float)image.Height * 0.01f);
                    break;
                case 4:
                    x = (int)((float)image.Width * 0.01f);
                    y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
                    break;
                case 5:
                    x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
                    y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
                    break;
                case 6:
                    x = (int)((float)image.Width * 0.99f - (float)image2.Width);
                    y = (int)((float)image.Height * 0.5f - (float)(image2.Height / 2));
                    break;
                case 7:
                    x = (int)((float)image.Width * 0.01f);
                    y = (int)((float)image.Height * 0.99f - (float)image2.Height);
                    break;
                case 8:
                    x = (int)((float)image.Width * 0.5f - (float)(image2.Width / 2));
                    y = (int)((float)image.Height * 0.99f - (float)image2.Height);
                    break;
                case 9:
                    x = (int)((float)image.Width * 0.99f - (float)image2.Width);
                    y = (int)((float)image.Height * 0.99f - (float)image2.Height);
                    break;
            }
            graphics.DrawImage(image2, new Rectangle(x, y, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel, imageAttributes);
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo imageCodecInfo = null;
            ImageCodecInfo[] array6 = imageEncoders;
            for (int i = 0; i < array6.Length; i++)
            {
                ImageCodecInfo imageCodecInfo2 = array6[i];
                if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
                {
                    imageCodecInfo = imageCodecInfo2;
                }
            }
            EncoderParameters encoderParameters = new EncoderParameters();
            long[] array7 = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            array7[0] = (long)quality;
            EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, array7);
            encoderParameters.Param[0] = encoderParameter;
            if (imageCodecInfo != null)
            {
                image.Save(filename, imageCodecInfo, encoderParameters);
            }
            else
            {
                image.Save(filename);
            }
            graphics.Dispose();
            image.Dispose();
            image2.Dispose();
            imageAttributes.Dispose();
        }

        public static void AddImageSignText(string imgPath, string filename, string watermarkText, int watermarkStatus, int quality, string fontname, int fontsize)
        {
            byte[] buffer = File.ReadAllBytes(Utils.GetMapPath(imgPath));
            Image image = Image.FromStream(new MemoryStream(buffer));
            filename = Utils.GetMapPath(filename);
            Graphics graphics = Graphics.FromImage(image);
            Font font = new Font(fontname, (float)fontsize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF sizeF = graphics.MeasureString(watermarkText, font);
            float num = 0f;
            float num2 = 0f;
            switch (watermarkStatus)
            {
                case 1:
                    num = (float)image.Width * 0.01f;
                    num2 = (float)image.Height * 0.01f;
                    break;
                case 2:
                    num = (float)image.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)image.Height * 0.01f;
                    break;
                case 3:
                    num = (float)image.Width * 0.99f - sizeF.Width;
                    num2 = (float)image.Height * 0.01f;
                    break;
                case 4:
                    num = (float)image.Width * 0.01f;
                    num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
                    break;
                case 5:
                    num = (float)image.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
                    break;
                case 6:
                    num = (float)image.Width * 0.99f - sizeF.Width;
                    num2 = (float)image.Height * 0.5f - sizeF.Height / 2f;
                    break;
                case 7:
                    num = (float)image.Width * 0.01f;
                    num2 = (float)image.Height * 0.99f - sizeF.Height;
                    break;
                case 8:
                    num = (float)image.Width * 0.5f - sizeF.Width / 2f;
                    num2 = (float)image.Height * 0.99f - sizeF.Height;
                    break;
                case 9:
                    num = (float)image.Width * 0.99f - sizeF.Width;
                    num2 = (float)image.Height * 0.99f - sizeF.Height;
                    break;
            }
            graphics.DrawString(watermarkText, font, new SolidBrush(Color.White), num + 1f, num2 + 1f);
            graphics.DrawString(watermarkText, font, new SolidBrush(Color.Yellow), num, num2);
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo imageCodecInfo = null;
            ImageCodecInfo[] array = imageEncoders;
            for (int i = 0; i < array.Length; i++)
            {
                ImageCodecInfo imageCodecInfo2 = array[i];
                if (imageCodecInfo2.MimeType.IndexOf("jpeg") > -1)
                {
                    imageCodecInfo = imageCodecInfo2;
                }
            }
            EncoderParameters encoderParameters = new EncoderParameters();
            long[] array2 = new long[1];
            if (quality < 0 || quality > 100)
            {
                quality = 80;
            }
            array2[0] = (long)quality;
            EncoderParameter encoderParameter = new EncoderParameter(Encoder.Quality, array2);
            encoderParameters.Param[0] = encoderParameter;
            if (imageCodecInfo != null)
            {
                image.Save(filename, imageCodecInfo, encoderParameters);
            }
            else
            {
                image.Save(filename);
            }
            graphics.Dispose();
            image.Dispose();
        }
    }
}
