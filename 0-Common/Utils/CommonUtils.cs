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
                //ImgToBase64String 转换失败\nException:" + ex.Message);
                return null;
            }
        }
        public static byte[] GetPictureData(string imagepath)
        {
            /**/////根据图片文件的路径使用文件流打开，并保存为byte[] 
            FileStream fs = new FileStream(imagepath, FileMode.Open);//可以是其他重载方法 
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }
        public static Bitmap Base64StringToImg(string Base64Str)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(Base64Str);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                //Base64StringToImage 转换失败\nException：" + ex.Message);
                return null;
            }
        }
    }
}
