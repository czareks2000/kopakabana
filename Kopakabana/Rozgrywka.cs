using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    abstract class Rozgrywka
    {
        protected List<Spotkanie> spotkania;
        protected SortedDictionary<Druzyna, int> wyniki;
        protected Typ typGry;

        public SortedDictionary<Druzyna, int> TablicaWynikow()
        {
            return wyniki;
        }

        public List<Spotkanie> ZakonczoneSpotkania()
        {
            List<Spotkanie> zakonczone = new List<Spotkanie>();

            foreach (var spotkanie in spotkania)
            {
                if (spotkanie.CzyZakonczone)
                    zakonczone.Add(spotkanie);
            }

            return zakonczone;
        }

        public List<Spotkanie> Spotkania()
        {
            return spotkania;
        }

        public Spotkanie KolejneSpotkanie()
        {
            foreach (var spotkanie in spotkania)
            {
                if (!spotkanie.CzyZakonczone)
                    return spotkanie;
            }

            return null;
        }

        public void DodajPunkt(Druzyna druzyna)
        {
            if (!wyniki.ContainsKey(druzyna))
                throw new Exception("Tablica wynikow nie zawiera podanej druzyny");

            wyniki[druzyna] += 1;
        }

        public Typ GetTyp()
        {
            return typGry;
        }
    }
}
