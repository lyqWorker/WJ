using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMethod
{
    public class AIHelper
    {
        public void Chat()
        {
            while (true)
            {
                string str = Next();
                str = str.Replace("?", "!");
                str = str.Replace("你", "我");
                str = str.Replace("吗", "");
                str = str.Replace("？", "!");
                Console.WriteLine("AI：" + str);
            }
        }
        public string Next()
        {
            return Console.ReadLine();
        }
    }
}
