using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class Druzyna
    {
        public string Nazwa { get; private set; }
        private List<Osoba> zawodnicy;

        public Druzyna()
        {
            zawodnicy = new List<Osoba>();
        }

        public Druzyna(List<Osoba> zawodnicy)
        {
            this.zawodnicy = zawodnicy;
        }

        public void DodajZawodnika(Osoba osoba)
        {
            zawodnicy.Add(osoba);
        }
    }
}
