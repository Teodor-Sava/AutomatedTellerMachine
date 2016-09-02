using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using static System.Linq.Enumerable;

namespace AutomatedTellerMachine.Helpers
{
    public class AccountGenerator
    {
        public static string AccountNumber()
        {   Random random = new Random();
            int num = random.Next(100000);
            num = num + 000000;
            return num.ToString();
        }
    }
}