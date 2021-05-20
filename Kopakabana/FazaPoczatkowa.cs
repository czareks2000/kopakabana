using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class FazaPoczatkowa : Rozgrywka
    {
        public FazaPoczatkowa(List<Druzyna> druzyny, List<Osoba> sedziowie, Typ typ)
        {
            //
        }

        public List<Druzyna> NajlepszeCztery()
        {
            List<Druzyna> najlepszeCztery = new List<Druzyna>(wyniki.Keys.Take(4));

            return najlepszeCztery;
        }
    }
}
