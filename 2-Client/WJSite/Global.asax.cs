using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WJSite
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static string WJServer = "";
        public static ValidatorSet VS;
        //验证码失效时间，单位秒
        private static readonly int LoseSecond = 30;
        //清理线程循环时间，单位秒
        private static readonly int SleepTime = 10;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            WJServer = ConfigurationManager.AppSettings["WJServer"];

            VS = new ValidatorSet();

            Thread clearThread = new Thread(ClearValidate);
            clearThread.Start();
        }

        private static void ClearValidate()
        {
            while (true)
            {
                foreach (var key in VS.VerCodeDic.Keys)
                {
                    var timeSpan = DateTime.Now - VS.VerCodeDic[key].ReqTime;
                    if (timeSpan.Seconds > LoseSecond)
                    {
                        var info = VS.VerCodeDic[key];
                        VS.VerCodeDic.TryRemove(key, out info);
                    }
                }
                Thread.Sleep(SleepTime * 1000);
            }
        }
    }
}
