using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Common
{
    public class VerificationCodeHelper
    {
        //public Dictionary<string, ValidateInfo> VerCodeDic = new Dictionary<string, ValidateInfo>();
      
        #region 参数
        //裁剪的小图大小
        private const int shearSize = 40;
        //原始图片数量
        private const int imgCount = 60;
        //原始图片宽,单位:px
        private int imgWidth = 300;
        //原始图片高,单位:px
        private int imgHeight = 200;
        //裁剪位置X轴最小位置
        private int minRangeX = 30;
        //裁剪位置Y轴最小位置
        private int minRangeY = 30;
        //裁剪位置X轴最大位置
        private int maxRangeX = 240;
        //裁剪位置Y轴最大位置
        private int maxRangeY = 150;
        //裁剪X轴大小，裁剪成20张上下10张
        private int cutX = 30;
        //裁剪Y轴大小，裁剪成20张上下10张
        private int cutY = 100;
        //小图相对于原图左上角的x坐标,并保存到session用于校验
        private int positionX;
        //小图相对于原图左上角的y坐标,y坐标返回前端
        private int positionY;
        //允许误差 单位像素
        private const int Deviation = 5;
        //是否跨域访问 在将项目做成第三方使用时可用跨域解决方案 所有的session替换成可共用的变量(Redis)
        private bool IsCallback = false;
        //最大错误次数
        private const int MaxErrorNum = 4;
        #endregion
        
        /// <summary>
        /// 后台校验验证是否被伪造
        /// </summary>
        //public string CheckResult()
        //{
        //    //校验成功 返回正确坐标
        //    if (VerCodeDic["isCheck"] == null)
        //       return "{\"errcode\":-1,\"errmsg\":\"校验未通过，未进行过校验\"}";
        //    else if (VerCodeDic["isCheck"].ToString() != "OK")
        //        return "{\"errcode\":-1,\"errmsg\":\"校验未通过\"}";
        //    else
        //       return "{\"errcode\":0,\"errmsg\":\"校验通过\"}";
        //}
        /// <summary>
        /// 校验验证
        /// </summary>
        /// <param name="pointX"></param>
        /// <param name="datelist"></param>
        /// <param name="timespan"></param>
        /// <returns></returns>

        public string CheckCode(ValidatePost post,string url)
        {
            url = url + "?guid=" + post.guid;
            var validateInfo = HttpUtils.GetData<ValidateCheckInfo>(url);
            if (validateInfo.Code == null|| validateInfo.Code.Length==0)
            {
                return "{\"state\":-1,\"msg\":发生错误}";
            }
            if (string.IsNullOrEmpty(post.point))
            {
                return "{\"state\":-1,\"msg\":未取到坐标值}";
            }
            int oldPoint = 0, nowPoint = 0;
            try
            {
                oldPoint = int.Parse(validateInfo.Code);
            }
            catch
            {
                return "{\"state\":-1,\"msg\":发生错误}";
            }
            try
            {
                nowPoint = int.Parse(post.point);
            }
            catch
            {
                return "{\"state\":-1,\"msg\":获取到坐标值不正确}";
            }
            //错误
            if (Math.Abs(oldPoint - nowPoint) > Deviation)
            {
                int checkCount = 0;
                try
                {
                    checkCount = validateInfo.ErrorNum;
                }
                catch
                {
                    checkCount = 0;
                }
                checkCount++;
                if (checkCount > MaxErrorNum)
                {
                    //超过最大错误次数后不再校验
                    validateInfo.Code = null;
                    return "{\"state\":-1,\"msg\":" + checkCount + "}";
                }
                validateInfo.ErrorNum = checkCount;
                //返回错误次数
                return "{\"state\":-1,\"msg\":" + checkCount + "}";
            }
            if (SlideFeature(post.datelist))
            {
                //机器人??
            }
            //校验成功 返回正确坐标
            //VerCodeDic.Remove(post.guid);
            return "{\"state\":0,\"info\":\"正确\",\"data\":" + oldPoint + "}";

        }
        //返回验证码json
        public string GetVerificationCode(Bitmap bitmap,DateTime reqTime, string url)
        {
            //第一步: 随机生成坐标
            Random random = new Random();
            positionX = random.Next(minRangeX, maxRangeX);
            positionY = random.Next(minRangeY, maxRangeY);
            string guid = Guid.NewGuid().ToString();
            ValidateCheckInfo vi = new ValidateCheckInfo()
            {
                Guid = guid,
                Code = positionX.ToString(),
                ErrorNum = -1,
                IsCheck = false,
                ReqTime = reqTime
            };

            string result = HttpUtils.PostData(url, vi);
            //VerCodeDic[guid] = vi;
            int[] a = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            //打乱a数组的顺序
            int[] array = a.OrderBy(x => Guid.NewGuid()).ToArray();
            //裁剪的小图
            Bitmap cutSmallBitmap = CutImage(bitmap, shearSize, shearSize, positionX, positionY);
            string smallImg = "data:image/jpg;base64," + CommonUtils.ImgToBase64String(cutSmallBitmap);
            //裁剪后的图
            Bitmap cutedBitmap = GetNewBitmap(bitmap, shearSize, shearSize, positionX, positionY);
            //混淆拼接的图
            Bitmap resultBitmap = ConfusionImage(array, cutedBitmap);
            string resultImg = "data:image/jpg;base64," + CommonUtils.ImgToBase64String(resultBitmap);
            JObject jObject = new JObject();
            jObject["errcode"] = 0;
            jObject["y"] = positionY;
            jObject["array"] = string.Join(",", array);
            jObject["imgx"] = imgWidth;
            jObject["imgy"] = imgHeight;
            jObject["small"] = smallImg;
            jObject["normal"] = resultImg;
            jObject["guid"] = guid;
            /* errcode: 状态值 成功为0
             * y:裁剪图片y轴位置
             * small：小图字符串
             * normal：剪切小图后的原图并按无序数组重新排列后的图
             * array：无序数组
             * imgx：原图宽
             * imgy：原图高
             */
            return jObject.ToString();
        }
        /// <summary>
        /// 获取裁剪的小图
        /// </summary>
        /// <param name="bitmap">原图</param>
        /// <param name="cutWidth">剪切宽度</param>
        /// <param name="cutHeight">剪切高度</param>
        /// <param name="x">x轴剪切位置</param>
        /// <param name="y">y轴剪切位置</param>
        /// <returns></returns>
        private Bitmap CutImage(Bitmap bitmap, int cutWidth, int cutHeight, int x, int y)
        {
            //载入底图
            Image image = bitmap;
            //初始化一个位图对象，来存储截取后的图像
            Bitmap bmpDest = new Bitmap(cutWidth, cutHeight, PixelFormat.Format32bppRgb);
            //这个矩形定义了将要在被截取的图像上要截取的图像区域的左顶点位置和大小
            Rectangle rectangle = new Rectangle(x, y, cutWidth, cutHeight);
            //这个矩形定义了，你将要把 截取的图像区域 绘制到初始化的位图的位置和大小
            //我的定义，说明，我将把截取的区域，从位图左顶点开始绘制，绘制截取的区域原来大小
            Rectangle rectDest = new Rectangle(0, 0, cutWidth, cutHeight);
            //第一个参数就是加载你要截取的图像对象，第二个和第三个参数及如上所说定义截取和绘制图像过程中的相关属性，第四个属性定义了属性值所使用的度量单位
            Graphics g = Graphics.FromImage(bmpDest);
            g.DrawImage(image, rectDest, rectangle, GraphicsUnit.Pixel);
            g.Dispose();
            return bmpDest;
        }
        private Bitmap GetNewBitmap(Bitmap bitmap, int cutWidth, int cutHeight, int x, int y)
        {
            // 加载原图片 
            Bitmap oldBmp = bitmap;
            // 绑定画板 
            Graphics grap = Graphics.FromImage(oldBmp);
            // 加载水印图片 
            Bitmap bt = new Bitmap(cutWidth, cutHeight);
            Graphics g1 = Graphics.FromImage(bt);  //创建b1的Graphics
            g1.FillRectangle(Brushes.Black, new Rectangle(0, 0, cutWidth, cutHeight));   //把b1涂成红色
            bt = PTransparentAdjust(bt, 120);
            // 添加水印 
            grap.DrawImage(bt, x, y, cutWidth, cutHeight);
            //grap.Dispose();
            //g1.Dispose();
            return oldBmp;
        }
        /// <summary>
        /// 获取半透明图像
        /// </summary>
        /// <param name="bmp">Bitmap对象</param>
        /// <param name="alpha">alpha分量。有效值为从 0 到 255。</param>
        private Bitmap PTransparentAdjust(Bitmap bmp, int alpha)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color bmpcolor = bmp.GetPixel(i, j);
                    byte A = bmpcolor.A;
                    byte R = bmpcolor.R;
                    byte G = bmpcolor.G;
                    byte B = bmpcolor.B;
                    bmpcolor = Color.FromArgb(alpha, R, G, B);
                    bmp.SetPixel(i, j, bmpcolor);
                }
            }
            return bmp;
        }
        /// <summary>
        /// 获取混淆拼接的图片
        /// </summary>
        /// <param name="a">无序数组</param>
        /// <param name="bmp">剪切小图后的原图</param>
        private Bitmap ConfusionImage(int[] a, Bitmap cutbmp)
        {
            Bitmap[] bmp = new Bitmap[20];
            for (int i = 0; i < 20; i++)
            {
                int x, y;
                x = a[i] > 9 ? (a[i] - 10) * cutX : a[i] * cutX;
                y = a[i] > 9 ? cutY : 0;
                bmp[i] = CutImage(cutbmp, cutX, cutY, x, y);
            }
            Bitmap Img = new Bitmap(imgWidth, imgHeight);      //创建一张空白图片
            Graphics g = Graphics.FromImage(Img);   //从空白图片创建一个Graphics
            for (int i = 0; i < 20; i++)
            {
                //把图片指定坐标位置并画到空白图片上面
                g.DrawImage(bmp[i], new Point(i > 9 ? (i - 10) * cutX : i * cutX, i > 9 ? cutY : 0));
            }
            g.Dispose();
            return Img;
        }
        /// <summary>
        /// 滑动特性
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool SlideFeature(string data)
        {
            if (string.IsNullOrEmpty(data))
                return false;
            string[] _datalist = data.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (_datalist.Length < 10)
                return false;
            //__array二维数组共两列 第一列为x轴坐标值 第二列为时间
            long[,] __array = new long[_datalist.Length, 2];
            #region 获取__array
            int row = 0;
            foreach (string str in _datalist)
            {
                string[] strlist = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (strlist.Length != 2)
                    return false;
                long __coor = 0, __date = 0;
                try { __coor = long.Parse(strlist[0]); __date = long.Parse(strlist[1]); }
                catch { return false; }
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                        __array[row, i] = __coor;
                    if (i == 1)
                        __array[row, i] = __date;
                }
                row++;
            }
            #endregion
            #region 计算速度 加速度 以及他们的标准差
            //速度 像素/每秒
            double[] __v = new double[_datalist.Length - 1];
            //加速度 像素/每2次方秒
            double[] __a = new double[_datalist.Length - 1];
            //总时间
            int __totaldate = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                //时间差
                int __timeSpan = 0;
                if (__array[i + 1, 1] - __array[i, 1] == 0)
                    __timeSpan = 1;
                else
                    __timeSpan = (CommonUtils.ConvertDateTime(__array[i + 1, 1].ToString()) - CommonUtils.ConvertDateTime(__array[i, 1].ToString())).Milliseconds;
                __v[i] = (double)1000 * Math.Abs(__array[i + 1, 0] - __array[i, 0]) / __timeSpan;//有可能移过再一回来 这里只取正值
                __a[i] = (double)1000 * __v[i] / __timeSpan;
                __totaldate += __timeSpan;
            }
            //速度的计算公式：v=S/t
            //加速度计算公式：a=Δv/Δt
            //分析速度与加速度

            //平均速度
            double __mv = 0;
            double __sumv = 0;
            double __s2v = 0;//速度方差
            double __o2v = 0;//速度标准差
            foreach (double a in __v)
            {
                __sumv += a;
            }
            __mv = __sumv / __v.Length;
            __sumv = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                __sumv += Math.Pow(__v[i] - __mv, 2);
            }
            __s2v = __sumv / __v.Length;
            __o2v = Math.Sqrt(__s2v);

            //平均加速度
            double __ma = 0;
            double __suma = 0;
            double __s2a = 0;//加速度方差
            double __o2a = 0;//加速度标准差
            foreach (double a in __a)
            {
                __suma += a;
            }
            __ma = __suma / __a.Length;
            __suma = 0;
            for (int i = 0; i < __a.Length; i++)
            {
                __suma += Math.Pow(__a[i] - __ma, 2);
            }
            __s2a = __suma / __v.Length;
            __o2a = Math.Sqrt(__s2a);

            double threeEqual = __a.Length / 3;
            //将加速度数组分成三等分 求每一份的加速度
            double __ma1 = 0, __ma2 = 0, __ma3 = 0;
            for (int i = 0; i < __a.Length; i++)
            {
                if (i > threeEqual * 2)
                    __ma3 += __a[i];
                else if (i > threeEqual && i < threeEqual * 2)
                    __ma2 += __a[i];
                else
                    __ma1 += __a[i];
            }
            __ma1 = __ma1 / threeEqual;
            __ma2 = __ma2 / threeEqual;
            __ma3 = __ma3 / threeEqual;
            //将速度数组分成三等分 求每一份的速度
            threeEqual = __v.Length / 3;
            double __mv1 = 0, __mv2 = 0, __mv3 = 0;
            for (int i = 0; i < __v.Length; i++)
            {
                if (i > threeEqual * 2)
                    __mv3 += __v[i];
                else if (i > threeEqual && i < threeEqual * 2)
                    __mv2 += __v[i];
                else
                    __mv1 += __v[i];
            }
            __mv1 = __mv1 / threeEqual;
            __mv2 = __mv2 / threeEqual;
            __mv3 = __mv3 / threeEqual;
            #endregion
            //return true;
            //将采样结果收集到数据库
            CreateTable();
            //SQLite_create_sql nsql = new SQLite_create_sql("data");
            //nsql.of_AddCol("sumtime", __totaldate);
            //nsql.of_AddCol("abscissa", HttpContext.Current.Request["point"]);
            //nsql.of_AddCol("total", _datalist.Length);
            //nsql.of_AddCol("meanv", __mv.ToString("0.0000000000"));
            //nsql.of_AddCol("meanv1", __mv1.ToString("0.0000000000"));
            //nsql.of_AddCol("meanv2", __mv2.ToString("0.0000000000"));
            //nsql.of_AddCol("meanv3", __mv3.ToString("0.0000000000"));
            //nsql.of_AddCol("meana", __ma.ToString("0.0000000000"));
            //nsql.of_AddCol("meana1", __ma1.ToString("0.0000000000"));
            //nsql.of_AddCol("meana2", __ma2.ToString("0.0000000000"));
            //nsql.of_AddCol("meana3", __ma3.ToString("0.0000000000"));
            //nsql.of_AddCol("standardv", __o2v.ToString("0.0000000000"));
            //nsql.of_AddCol("standarda", __o2a.ToString("0.0000000000"));
            //nsql.of_execute();
            return true;
        }

        private void CreateTable()
        {
            //if (!SQLiteHelper.of_ExistTable("data"))
            //{
            //    string ls_sql = @"create table data
            //                (
            //                    id integer primary key autoincrement,
            //                    sumtime varchar,
            //                    abscissa varchar,
            //                    total varchar,
            //                    meanv varchar,
            //                    meanv1 varchar,
            //                    meanv2 varchar,
            //                    meanv3 varchar,
            //                    meana varchar,
            //                    meana1 varchar,
            //                    meana2 varchar,
            //                    meana3 varchar,
            //                    standardv varchar,
            //                    standarda varchar
            //                )";
            //    SQLiteHelper.ExecuteNonQuery(ls_sql);
            //}
        }
       
    }
    public enum ActionType
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        GetCode,
        /// <summary>
        /// 校验验证码
        /// </summary>
        Check,
        /// <summary>
        /// 获取校验结果
        /// </summary>
        GetResult
    }
}
