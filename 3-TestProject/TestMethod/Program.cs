using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace TestMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            /**RSA加密测试,RSA中的密钥对通过SSL工具生成，生成命令如下： 
           * 1 生成RSA私钥： 
           * openssl genrsa -out rsa_private_key.pem 1024 
           *2 生成RSA公钥 
           * openssl rsa -in rsa_private_key.pem -pubout -out rsa_public_key.pem 
           * 
           * 3 将RSA私钥转换成PKCS8格式 
           * openssl pkcs8 -topk8 -inform PEM -in rsa_private_key.pem -outform PEM -nocrypt -out rsa_pub_pk8.pem 
           * 
           * 直接打开rsa_private_key.pem和rsa_pub_pk8.pem文件就可以获取密钥对内容，获取密钥对内容组成字符串时，注意将换行符删除 
           * */
            //rsa_pub_pk8.pem内容
           
            //加密字符串  
            string data = "yangyoushan";
            string publickey = "";
            string privatekey = "";
            using (StreamReader reader = new StreamReader("publicKey.txt"))
            {
                publickey = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader("privateKey.txt"))
            {
                privatekey = reader.ReadToEnd();
            }

            Console.WriteLine("加密前字符串内容：" + data);
            //加密  
            
            string encrypteddata = RSAUtils.EncryptData(data, publickey, "UTF-8");
            Console.WriteLine("加密后的字符串为：" + encrypteddata);
            Console.WriteLine("解密后的字符串内容：" + RSAUtils.DecryptData(encrypteddata, privatekey, "UTF-8"));
           

            Console.ReadKey();


        }
      

        static void Test(List<Person> list1)
        {
            try
            {
                if (list1.Count > 1)
                {
                    var lst = list1.Take(3).ToList();
                    list1.RemoveAll(p => lst.Contains(p));
                    int a = 0;
                    a = int.Parse("");
                    Test(list1);
                }
                else
                {
                    Console.WriteLine("ok");
                }
            }
            catch (Exception ex)
            {
                Test(list1);
                Console.WriteLine(ex.Message);
            }
            
           
        }

    }
    public class Person
    {
        public string name { get; set; }
        public int age { get; set; }
    }
}
