using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    [Serializable]
    public class FazaFinalowa : Rozgrywka
    {
        public Spotkanie Final { get; private set; }

        public FazaFinalowa() { }

        /// <summary>
        /// Konstruktor tworzy dwa spotkania półfinałowe
        /// </summary>
        public FazaFinalowa(List<Druzyna> druzyny, List<Osoba> sedziowie, TypGry typ)
            : base(druzyny, typ)
        {
            //utworzenie dwóch spotkań półfinałowych
            spotkania.Add(new Spotkanie(druzyny[0], druzyny[1], LosujSedziow(sedziowie)));
            spotkania.Add(new Spotkanie(druzyny[2], druzyny[3], LosujSedziow(sedziowie)));
        }
        /// <summary>
        /// Funkcja tworzy spotkanie finałowe z drużyn które wygrały półfinały
        /// </summary>
        public Spotkanie RozegrajFinal(List<Osoba> sedziowie)
        {
            if(new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(2).ToList().Count < 2)
                throw new ZbytMalaLiczbaDruzynException("Druzyn jest mniej niz 2");

            //pobranie dwóch wygranych drużyn z tablicy wyników
            List<Druzyna> finaloweDruzyny = new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(2).ToList();

            //utworzenie finalowego spotkania
            Final = new Spotkanie(finaloweDruzyny[0], finaloweDruzyny[1], LosujSedziow(sedziowie));
            spotkania.Add(Final);

            return Final;
        }

    }
}
