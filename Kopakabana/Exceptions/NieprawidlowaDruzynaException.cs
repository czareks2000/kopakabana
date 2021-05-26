using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class NieprawidlowaDruzynaException: Exception
    {
        public NieprawidlowaDruzynaException() {}
        public NieprawidlowaDruzynaException(string msg) :base(msg) { }
    }
}
