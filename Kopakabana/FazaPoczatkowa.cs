using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class FazaPoczatkowa : Rozgrywka
    {
        public FazaPoczatkowa(List<Druzyna> druzyny, List<Osoba> sedziowie, TypGry typ)
            : base(druzyny, typ)
        {
            if (druzyny.Count == 0)
                throw new BrakDruzynException("Nie dodano druzyn do rozgrywki");

            //utworzenie spotkania każdy z każdym
            for (int i=0; i < druzyny.Count; i++)
            {
                for (int j = i; j < druzyny.Count; j++)
                {
                    if (j != i)
                        spotkania.Add(new Spotkanie(druzyny[i], druzyny[j], LosujSedziow(sedziowie)));
                }
            }

            //zmiana kolejności spotkań na losową
            spotkania = spotkania.OrderBy(i => Guid.NewGuid()).ToList();
        }

        public List<Druzyna> NajlepszeCztery()
        {
            if (new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(4).ToList().Count < 4)
                throw new ZbytMalaLiczbaDruzynException("Druzyn jest mniej niz 4");

            List<Druzyna> najlepszeCztery = new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(4).ToList();

            return najlepszeCztery;
        }
    }
}
