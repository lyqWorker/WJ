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
    public class ValidatorController : ApiController
    {
        [HttpPost]
        public string AddValidate([FromBody]ValidateCheckInfo vi)
        {
            try
            {
                Program.ValidateDic[vi.Guid] = vi;
                return "OK";
            }
            catch (Exception ex)
            {
                return "FALSE";
            }
        }
        [HttpGet]
        public ValidateCheckInfo GetValidate(string guid)
        {
            try
            {
                return Program.ValidateDic[guid];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpGet]
        public string GetUpdateRes(string guid,int num)
        {
            try
            {
                var item = Program.ValidateDic[guid];
                item.ErrorNum = num;
                return "OK";
            }
            catch (Exception ex)
            {
                return "FALSE";
            }
        }
    }
}
