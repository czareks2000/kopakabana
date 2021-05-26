using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class ZbytMalaLiczbaDruzynException: Exception
    {
        public ZbytMalaLiczbaDruzynException() { }
        public ZbytMalaLiczbaDruzynException(string msg): base(msg) { }
    }
}
