using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class PustaListaSedziowException: Exception
    {
        public PustaListaSedziowException() { }
        public PustaListaSedziowException(string msg): base(msg) { }
    }
}
