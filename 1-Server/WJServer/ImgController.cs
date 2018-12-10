using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Utils;

namespace WJServer
{
    public class ImgController : ApiController
    {
        [HttpGet]
        public string GetBitmap()
        {
            Random random = new Random();
            int num = random.Next(0, 60);
            string imgFileName = num.ToString() + ".jpg";
            string imgUrl = Path.Combine(Program.ImgPath, imgFileName);
            var base64Str = CommonUtils.ImgToBase64String(new Bitmap(imgUrl));
            return base64Str;
        }

    }
}
