using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class ZakonczoneSpotkanieException: Exception
    {
        public ZakonczoneSpotkanieException() { }
        public ZakonczoneSpotkanieException(string msg): base(msg) { }
    }
}
