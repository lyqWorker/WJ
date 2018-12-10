using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class HttpUtils
    {
        #region GET

        public static string GetData(string url)
        {
            // 创建一个HTTP请求
            WebRequest req = WebRequest.Create(url);
            req.Method = "GET";
            WebResponse res = req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string resValue = reader.ReadToEnd();
            reader.Close();
            res.Close();
            return resValue;
        }

        public static T GetData<T>(string url)
        {
            // 创建一个HTTP请求
            WebRequest req = WebRequest.Create(url);
            req.Method = "GET";
            WebResponse res = req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string resValue = reader.ReadToEnd();
            reader.Close();
            res.Close();

            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(resValue);
            return (T)serializer.Deserialize(new JsonTextReader(sr), typeof(T));
        }
        public static List<T> GetDataList<T>(string url)
        {
            // 创建一个HTTP请求
            WebRequest req = WebRequest.Create(url);
            req.Method = "GET";
            WebResponse res = req.GetResponse();
            StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            string resValue = reader.ReadToEnd();
            reader.Close();
            res.Close();

            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(resValue);
            return (List<T>)serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
        }
        #endregion

        #region POST
        public static string PostData<T>(string url, T postObj)
        {
            JsonSerializer serializer = new JsonSerializer();
            StringWriter sw = new StringWriter();
            serializer.Serialize(new JsonTextWriter(sw), postObj);
            string postData = sw.GetStringBuilder().ToString();
            string contentType = "application/json";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Method = "POST";
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = sr.ReadToEnd();
            httpWebResponse.Close();
            sr.Close();
            httpWebRequest.Abort();

            return responseContent;
        }
        #endregion
    }
}
