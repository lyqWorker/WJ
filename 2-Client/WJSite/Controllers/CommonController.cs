using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Xml;
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

        [HttpGet]
        [Route("pubKey")]
        public string GetPublicKey()
        {
            //string url = Path.Combine(WebApiApplication.WJServer, "api/WJ");
            //string result = HttpUtils.GetData(url);
            return "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC4JrKGRCMB0HBwMxuTb0zMSkIRVkdVhkOHGSmSGCxIH/ZVsC0F6RwNmM6Sl0qxabrqSbOAx/J/iAbmG3MDShxaFCsrdh9KSivwArE2o8mrchOjN+o6AG7O+9jdPCog1CPhiAgyj/jcDN0ctDYd4MC2A09MoLeyYNxQyUH2tRsOJQIDAQAB";


        }

    }
}
