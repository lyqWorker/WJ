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
        VerificationCodeHelper vch = new VerificationCodeHelper();
        [HttpGet]
        [Route("validateCode")]
        public string GetVerifcationCode()
        {
            string url = "http://localhost:8001/api/Img/GetBitmap";
            string base64 = HttpUtils.GetData(url);
            Bitmap bitmap = CommonUtils.Base64StringToImg(base64);
            string res = vch.GetVerificationCode(bitmap);
            return res;
        }
        [HttpPost]
        [Route("checkCode")]
        public string CheckCode([FromBody]ValidateInfo validate)
        {
            return vch.CheckCode(validate.point, validate.datelist, validate.timespan);
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
