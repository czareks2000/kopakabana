using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class Typ
    {
        public string Nazwa { get; private set; }
        public int LiczbaSedziow { get; private set; }

        public Typ(string nazwa, int liczba)
        {
            Nazwa = nazwa;
            LiczbaSedziow = liczba;
        }
    }
}
