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
            return WebApiApplication.VS.GetVerificationCode(bitmap, DateTime.Now);
        }
        [HttpPost]
        [Route("checkCode")]
        public CheckResponse CheckCode([FromBody]ValidatorCheckPost post)
        {
            CheckResponse result = new CheckResponse();
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
            return WebApiApplication.VS.CheckCode(post, result);
        }
        [HttpPost]
        [Route("login")]
        public CheckResponse Login([FromBody]WJUser user)
        {
            var res = new CheckResponse();
            if (user == null || CommonUtils.IsNullStr(user.UNO) || CommonUtils.IsNullStr(user.PW))
            {
                res.Msg = "user schemas is error";
                res.ErrorItem = "user";
                return res;
            }
            using (WJCenterEntities ctx = new WJCenterEntities())
            {
                string uno = user.UNO.Split('|')[0];
                string guid = user.UNO.Split('|')[1];
                var item = ctx.WJUsers.Where(p => p.UNO == uno).FirstOrDefault();
                if (item == null)
                {
                    res.Msg = "该工号不存在";
                    res.ErrorItem = "username";
                    return res;
                }
                if (item.ErrorStatus > 0)
                {
                    res.Msg = "该工号已冻结";
                    res.ErrorItem = "username";
                    return res;
                }
                //后期密码要加密
                if (item.PW != user.PW)
                {
                    res.Msg = "密码错误";
                    res.ErrorItem = "pwd";
                    return res;
                }
                if (!WebApiApplication.VS.VerCodeDic.ContainsKey(guid))
                {
                    res.Msg = "验证码已过期，请重新验证";
                    res.ErrorItem = "validator";
                    return res;
                }
                var vcode = WebApiApplication.VS.VerCodeDic[guid];
                if (!vcode.IsCorrect)
                {
                    res.Msg = "验证码错误";
                    res.ErrorItem = "validator";
                    return res;
                }
                res.State = 1;
                res.Msg = "/Views/main.html";
                return res;
            }
        }
    }
}