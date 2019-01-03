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
