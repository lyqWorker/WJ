using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WJSite.Controllers
{
    [RoutePrefix("api/common")]
    public class CommonController : ApiController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("message")]
        public HttpResponseMessage GetMessage()
        {
            var resp = new HttpResponseMessage();
            resp.Content = new StringContent("OK");
            return resp;
        }
    }
}
