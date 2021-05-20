using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class Osoba
    {
        private string imie;
        private string nazwisko;

        public void SetNazwa(string imie, string nazwisko)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
        }

        public string GetNazwa()
        {
            return imie + " " + nazwisko;
        }
    }
}
