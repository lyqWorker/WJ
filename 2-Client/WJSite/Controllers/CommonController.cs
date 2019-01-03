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
using Crypto;
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
        public string[] GetPublicKey()
         {
            RSACrypto crypto = new RSACrypto();
            RSAItem item = new RSAItem();
            string privateKey = "", publicKey = "";
            crypto.CreateKeys(out privateKey,out publicKey);
            item.PrivateKey = privateKey;
            item.PublickKey = publicKey;
            item.Guid = Guid.NewGuid().ToString();
            item.CreateTime = DateTime.Now;
            RedisHelper.Add(item.Guid, item);
            string[] arry = new string[2];
            arry[0] = item.Guid;
            arry[1] = RSAConvert.PemPublicKeyByXml(item.PrivateKey);
            return arry;
        }

        [HttpGet]
        [Route("jiemi")]
        public string GetJieMiData(string guid,string mStr)
        {
            RSACrypto crypto = new RSACrypto();
            var item = RedisHelper.Get<RSAItem>(guid);
            RedisHelper.Remove(guid);
            return crypto.Decrypt(item.PrivateKey, mStr);

        }

    }
}
