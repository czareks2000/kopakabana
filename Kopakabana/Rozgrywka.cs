using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    [Serializable]
    public abstract class Rozgrywka
    {
        protected List<Spotkanie> spotkania;
        protected Dictionary<Druzyna, int> wyniki;
        protected TypGry typGry;

        public Rozgrywka()
        {
            wyniki = new Dictionary<Druzyna, int>();
            spotkania = new List<Spotkanie>();
        }
        /// <summary>
        /// funckja tworzy rozgrywkę, ustwaia typ gry (zależnie od typu gry liczba sędziów jest różna)
        /// tworzy tablicę na punkty, przypisując każdej drużynie 0 punktów
        /// 
        /// </summary>
        public Rozgrywka(List<Druzyna> druzyny, TypGry typ)
        {
            if(druzyny.Count<4)
                throw new ZbytMalaLiczbaDruzynException("Druzyn jest mniej niz 4");

            wyniki = new Dictionary<Druzyna, int>();
            spotkania = new List<Spotkanie>();

            typGry = typ;

            //utworzenie tablicy wyników i przypisanie wartości 0 każdej drużynie
            foreach (var druzyna in druzyny)
            {
                wyniki.Add(druzyna, 0);
            }
        }

        //zwraca posortowaną tablice wyników
        public IOrderedEnumerable<KeyValuePair<Druzyna, int>> TablicaWynikow()
        {
            return from entry in wyniki orderby entry.Value descending select entry;
        }

        public List<Spotkanie> Spotkania()
        {
            return spotkania;
        }

        /// <summary>
        /// funkcja zwracająca kolejne nie rozegrane spotkanie
        /// </summary>
        public Spotkanie KolejneSpotkanie()
        {
            foreach (var spotkanie in spotkania)
            {
                if (!spotkanie.CzyZakonczone)
                    return spotkanie;
            }

            return null;
        }
        /// <summary>
        /// dodaje punkt do tablicy wyników drużynie która wygrała spotkanie
        /// </summary>
        public void DodajPunkt(Druzyna druzyna)
        {
            if (!wyniki.ContainsKey(druzyna))
                throw new NieprawidlowaDruzynaException("Tablica wynikow nie zawiera podanej druzyny");

            wyniki[druzyna] += 1;
        }

        public TypGry GetTyp()
        {
            return typGry;
        }
        /// <summary>
        /// funkcja losuje i zwraca sędziów/sędziego (w zależności od typu gry)
        /// </summary>
        protected List<Osoba> LosujSedziow(List<Osoba> wszyscySedziowie)
        {
            if (wszyscySedziowie.Count == 0)
                throw new PustaListaSedziowException("Lista sedziow jest pusta");

            if (typGry.LiczbaSedziow > wszyscySedziowie.Count)
                throw new ZbytMalaLiczbaSedziowException("Zbyt mala liczba sedziow aby rozegrac spotkanie w grze: ", typGry.Nazwa);

            //tworzymy HashSet losowych indekesów bez powtórzeń
            HashSet<int> wylosowaneIndeksy = new HashSet<int>();
            while(wylosowaneIndeksy.Count < typGry.LiczbaSedziow)
            {
                wylosowaneIndeksy.Add(Util.GetRandom(0, wszyscySedziowie.Count));
            }

            //tworzymi liste sedziow o wylosowanych wczesniej indeksach
            List<Osoba> sedziowie = new List<Osoba>();
            foreach (var index in wylosowaneIndeksy)
            {
                sedziowie.Add(wszyscySedziowie[index]);
            }

            return sedziowie;
        }

    }
}
