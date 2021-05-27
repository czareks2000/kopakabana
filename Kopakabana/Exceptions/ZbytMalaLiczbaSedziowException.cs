using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class ZbytMalaLiczbaSedziowException: Exception
    {
        private string nazw;
        public ZbytMalaLiczbaSedziowException(string nazwa) { nazw = nazwa; }
        public ZbytMalaLiczbaSedziowException( string msg, string nazwa): base(msg) { nazw = nazwa;  }

        public string getNazw() { return nazw; }
    }
}
