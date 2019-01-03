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
            string privateKey = "";
            string publicKey = "";
            RSACrypto crypto = new RSACrypto();
            crypto.CreateKeys(out privateKey, out publicKey);
            return crypto.RSAPrivateKeyDotNet2Java(privateKey);
        }

       

    }
}
