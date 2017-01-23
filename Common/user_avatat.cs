using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Common
{
    public class user_avatat
    {
        public MemoryStream Create(string name)
        {
            string[] array = new string[]
            {
                "Microsoft YaHei",
                "Comic Sans MS",
                "Arial",
                "宋体"
            };
            Bitmap bitmap = new Bitmap(80, 80);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.LightGray);
            Font font = new Font(array[0], 35f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.White);
            graphics.DrawString(name, font, brush, new PointF(9f, 6f));
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            graphics.Dispose();
            bitmap.Dispose();
            return memoryStream;
        }
    }
}
