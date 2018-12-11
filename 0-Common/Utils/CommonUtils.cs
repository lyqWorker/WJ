using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class CommonUtils
    {
        /// <summary>
        /// 检查前端发送的字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullStr(string str)
        {
            if (string.IsNullOrEmpty(str) || str == "undefined")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取图片二进制数据
        /// </summary>
        /// <param name="url">图片路径</param>
        /// <returns></returns>
        public static byte[] GetImageByUrl(string url)
        {
            //将图片以文件流的形式进行缓存
            using (FileStream fs = new FileStream(url,FileMode.Open,FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    //将流读入到字节数组中
                    byte[] bytes = br.ReadBytes((int)fs.Length);
                    return bytes;
                }
            }
        }
        /// <summary>
        /// 获取Bitmap的Base64String
        /// </summary>
        /// <param name="bitmap">图像</param>
        /// <returns></returns>
        public static string ImgToBase64String(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    //ms.Close();
                    return Convert.ToBase64String(arr);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Base64String转换为Bitmap
        /// </summary>
        /// <param name="Base64Str">Base64Str</param>
        /// <returns></returns>
        public static Bitmap Base64StringToImg(string base64Str)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(base64Str);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch
            {
                base64Str = base64Str.Trim('"');
                try
                {
                    byte[] arr = Convert.FromBase64String(base64Str);
                    MemoryStream ms = new MemoryStream(arr);
                    Bitmap bmp = new Bitmap(ms);
                    ms.Close();
                    return bmp;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ConvertDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime now = dtStart.Add(toNow);
            return now;
        }
    }
}
