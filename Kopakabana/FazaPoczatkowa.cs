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
            List<Druzyna> najlepszeCztery = new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(4).ToList();

            return najlepszeCztery;
        }
    }
}
