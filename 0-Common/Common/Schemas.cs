using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    #region 通用的请求校验的返回类
    /*
     * 1.验证码校验结果
     * 2.登录校验结果
     */
    public class CheckResponse
    {
        public int State { get; set; } = -1;
        public string Msg { get; set; }
        public string ErrorItem { get; set; }
    }
    #endregion

    #region 验证码
    //验证码校验字段(前台提交验证码校验值)
    public class ValidatorCheckPost
    {
        public string Guid { get; set; }
        public int PointX { get; set; }
        public string Timespan { get; set; }
        public string Datelist { get; set; }
    }
    //缓存池记录的验证码信息
    public class ValidatorItem
    {
        //生成的验证码的唯一标识
        public string Guid { get; set; }
        //用来校验的X坐标值
        public int PointX { get; set; }
        //记录校验的错误次数
        public int ErrorNum { get; set; }
        //获取验证码的时间，超时清理
        public DateTime ReqTime { get; set; }
        //标记该验证码是否通过
        public bool IsCorrect { get; set; } = false;
    }
    //验证码信息(前台展示的图片验证码)
    public class ValidatorCode
    {
        //生成的验证码的唯一标识
        public string Guid { get; set; }
        //验证码混淆后的图片
        public string ResultImg { get; set; }
        //裁剪的小图片
        public string SmallImg { get; set; }
        //验证码图片宽度
        public int Width { get; set; }
        //验证码图片高度
        public int Height { get; set; }
        //验证码图片混淆规律
        public string LawArry { get; set; }
        //裁剪的小图的y轴坐标
        public int PointY { get; set; }
        //状态码
        public int State { get; set; }
    }
    #endregion



}
