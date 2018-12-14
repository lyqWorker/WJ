using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common;
using Utils;
using WJEntities;

namespace WJSite.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
       
        [HttpGet]
        [Route("validateCode")]
        public string GetVerifcationCode()
        {
            string url = Path.Combine(WebApiApplication.WJServer, "api/Img/GetValidateImg");

            string result = HttpUtils.GetData(url).Trim('"');
            Bitmap bitmap = CommonUtils.Base64StringToImg(result);
            //url = Path.Combine(WebApiApplication.WJServer, "api/Validator/AddValidate");
            return WebApiApplication.VCH.GetVerificationCode(bitmap, DateTime.Now); ;
        }
        [HttpPost]
        [Route("checkCode")]
        public string CheckCode([FromBody]ValidatePost post)
        {
            //string url = Path.Combine(WebApiApplication.WJServer, "api/Validator/GetValidate");
            return WebApiApplication.VCH.CheckCode(post);
        }
        [HttpPost]
        [Route("login")]
        public ResponseMsg Login([FromBody]UserLogin user)
        {
            var res = new ResponseMsg();
            if (user == null)
            {
                res.State = ResponseState.Warining;
                res.Msg = "param(user) is null";
                return res;
            }
            using (WJCenterEntities ctx = new WJCenterEntities())
            {
                var item = ctx.UserLogins.Where(p => p.UNO == user.UNO).FirstOrDefault();
                if (item == null)
                {
                    res.State = ResponseState.Warining;
                    res.Msg = "该工号不存在";
                    return res;
                }
                if (item.Status > 0)
                {
                    res.State = ResponseState.Warining;
                    res.Msg = "该工号已冻结";
                    return res;
                }
                if (item.PW != user.PW)
                {
                    res.State = ResponseState.Warining;
                    res.Msg = "密码错误";
                    return res;
                }

            }
            return res;
        } 
    }
}
