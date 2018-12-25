using Common;
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
        public string GetValidateImg()
        {
            try
            {
                Random random = new Random();
                string imgFileName = random.Next(0, 60) + ".jpg";
                string url = Path.Combine(Program.ImgFroot, imgFileName);
                byte[] bytes = CommonUtils.GetImageByUrl(url);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return Program.SpareImgBase64Str;
            }
        }
    }
}
