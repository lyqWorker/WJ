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
        public ValidatorCode GetVerifcationCode()
        {
            string url = Path.Combine(WebApiApplication.WJServer, "api/Img/GetValidateImg");
            string result = HttpUtils.GetData(url).Trim('"');
            Bitmap bitmap = CommonUtils.Base64StringToImg(result);
            return WebApiApplication.VS.GetVerificationCode(bitmap, DateTime.Now); ;
        }
        [HttpPost]
        [Route("checkCode")]
        public ValidatorCheckResult CheckCode([FromBody]ValidatorCheckPost post)
        {
            ValidatorCheckResult result = new ValidatorCheckResult();
            if (post == null)
            {
                result.Msg = "验证失败!";
                return result;
            }
            if (CommonUtils.IsNullStr(post.Guid))
            {
                result.Msg = "验证失败!";
                return result;
            }
            return WebApiApplication.VS.CheckCode(post);
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
