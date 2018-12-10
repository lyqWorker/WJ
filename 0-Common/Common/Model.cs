using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ResponseMsg
    {
        private string resmsg;
        public ResponseState State { get; set; }
        public string Msg
        {
            get
            {
                if (State == ResponseState.Success)
                {
                    return "调用成功!";
                }
                else if (State == ResponseState.Error)
                {
                    return "服务器内部错误!";
                }
                else
                {
                    return resmsg;
                }
            }
            set
            {
                resmsg = value;
            }
        }
        public string Content { get; set; }
    }

    public class ValidateInfo
    {
        public string point { get; set; }
        public string timespan { get; set; }
        public string datelist { get; set; }
    }
}
