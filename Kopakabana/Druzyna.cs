using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    [Serializable]
    public class Druzyna
    {
        public string Nazwa { get; private set; }
        public List<Osoba> Zawodnicy { get; private set; }

        public Druzyna()
        {
            Zawodnicy = new List<Osoba>();
        }

        public Druzyna(string nazwa)
        {
            Nazwa = nazwa;
            Zawodnicy = new List<Osoba>();
        }

        public Druzyna(List<Osoba> zawodnicy)
        {
            Zawodnicy = zawodnicy;
        }

        public void DodajZawodnika(Osoba osoba)
        {
            Zawodnicy.Add(osoba);
        }
    }
}
