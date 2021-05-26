using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class ZbytMalaLiczbaSedziowException: Exception
    {
        public ZbytMalaLiczbaSedziowException() { }
        public ZbytMalaLiczbaSedziowException(string msg, string nazwa) { Console.WriteLine(msg + nazwa); }
    }
}
