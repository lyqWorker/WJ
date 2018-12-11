using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace WJSite
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static string WJServer = "";
        public static VerificationCodeHelper VCH;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            WJServer = ConfigurationManager.AppSettings["WJServer"];

            VCH = new VerificationCodeHelper();
        }
    }
}
