using System;
using System.Collections.Generic;
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
            VerificationCodeUtils verificationCode = new VerificationCodeUtils();
            verificationCode.GetVerificationCode();
        }
    }
}
