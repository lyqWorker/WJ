using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Utils;

namespace WJServer
{
    public class WJController: ApiController
    {
        [HttpGet]
        public string GetPublicKey()
        {
            
            return RSAUtils.GetPublicKey();
        }
    }
}
