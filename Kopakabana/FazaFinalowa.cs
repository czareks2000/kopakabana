using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class FazaFinalowa : Rozgrywka
    {
        public FazaFinalowa(List<Druzyna> druzyny, List<Osoba> sedziowie, Typ typ)
            : base(druzyny, typ)
        {
            //utworzenie dwóch spotkań półfinałowych
            spotkania.Add(new Spotkanie(druzyny[0], druzyny[1], LosujSedziow(sedziowie)));
            spotkania.Add(new Spotkanie(druzyny[2], druzyny[3], LosujSedziow(sedziowie)));
        }

        public Spotkanie RozegrajFinal(List<Osoba> sedziowie)
        {
            List<Druzyna> finaloweDruzyny = new List<Druzyna>(wyniki.Keys.Take(2));

            Spotkanie final = new Spotkanie(finaloweDruzyny[0], finaloweDruzyny[1], LosujSedziow(sedziowie));
            spotkania.Add(final);

            return final;
        }

    }
}
