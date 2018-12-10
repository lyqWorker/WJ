using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace TestMethod
{
    class Program
    {
        public static string ImgPath = "C:\\VCodeImg\\";
        static void Main(string[] args)
        {
           string a = GetBitmap();
        }

        public static string GetBitmap()
        {
            Random random = new Random();
            int num = random.Next(0, 60);
            string imgFileName = num.ToString() + ".jpg";
            string imgUrl = Path.Combine(ImgPath, imgFileName);
            var bytes = CommonUtils.GetPictureData(imgUrl);
            string pic = Convert.ToBase64String(bytes).Replace(" ", "");
            Bitmap bitmap = CommonUtils.Base64StringToImg(pic);
            return pic;
        }
    }
}
