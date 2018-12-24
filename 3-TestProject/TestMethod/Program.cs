using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace TestMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 测试一
            //var list1 = new List<Person>();
            //for (int i = 0; i < 10; i++)
            //{
            //    Person p = new Person()
            //    {
            //        age = 10 + i,
            //        name = "机器人" + i + "号"
            //    };
            //    list1.Add(p);
            //}
            //var list2 = new List<int>();
            //for (int i = 5; i < 15; i++)
            //{
            //    list2.Add(i);
            //}
            //var a= list1.RemoveAll(p => list2.Contains(p.age));
            //var b = list1;
            #endregion

            var list1 = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                Person p = new Person()
                {
                    age = 10 + i,
                    name = "机器人" + i + "号"
                };
                list1.Add(p);
            }
            Test(list1);


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
