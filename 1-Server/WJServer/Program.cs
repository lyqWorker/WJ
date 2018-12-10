using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace WJServer
{
    class Program
    {
        public static string ImgPath = "C:\\VCodeImg\\";

        private static HttpSelfHostServer Server = null;
        static void Main(string[] args)
        {
            Console.Title = "影像服务";

            RegisterApi();

            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void RegisterApi()
        {
            //服务端需开启8001端口
            string url = "http://localhost:8001";

            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(url);
            //提交json数据返回的支持,默认是返回xml格式数据
            //本例子为了迎合jQuery的ajax 请求，添加datatype参数映射，
            //这样ajax指定datatype:json的时候就自动返回json数据给浏览器
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("datatype", "json", "application/json"));

            //跨域配置
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            //注册路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            //打开服务
            Server = new HttpSelfHostServer(config);
            Server.OpenAsync().Wait();
            Console.WriteLine("服务已启动!");
        }
    }
}
