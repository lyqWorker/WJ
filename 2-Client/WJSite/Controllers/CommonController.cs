using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Common;
using Utils;

namespace WJSite.Controllers
{
    [RoutePrefix("api/common")]
    public class CommonController : ApiController
    {
        [HttpGet]
        [Route("message")]
        public HttpResponseMessage GetMessage()
        {
            var resp = new HttpResponseMessage();
            resp.Content = new StringContent("OK");
            return resp;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="action"></param>
        /// <param name="spec">图片规格</param>
        /// <returns></returns>
        [HttpGet]
        [Route("verification")]
        public ResponseMsg GetVerificationCode(string action, string spec)
        {
            var res = new ResponseMsg();

            #region 校验参数
            if (CommonUtils.IsNullStr(action))
            {
                res.State = ResponseState.Success;
                res.Msg = "\"action\"不能为空!";
                return res;
            }
            if (!Contract.actionList.Contains(action))
            {
                res.State = ResponseState.Warining;
                res.Msg = "错误的\"action\"值!";
                return res;
            }
            #endregion

            switch (action)
            {

                default:
                    break;
            }


            return res;


        }
    }
}
