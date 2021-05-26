using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class BrakDruzynException: Exception
    {
        public BrakDruzynException() { }
        public BrakDruzynException(string msg): base(msg) { }
    }
}
