using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum ResponseState
    {
        /// <summary>
        /// 调用成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 调用失败
        /// </summary>
        Warining = 101,
        /// <summary>
        /// 调用异常
        /// </summary>
        Error = 500
    }
}
