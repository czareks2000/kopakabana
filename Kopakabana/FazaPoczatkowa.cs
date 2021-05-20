﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class FazaPoczatkowa : Rozgrywka
    {
        public FazaPoczatkowa(List<Druzyna> druzyny, List<Osoba> sedziowie, Typ typ)
            : base(druzyny, typ)
        {
            //utworzenie spotkania każdy z każdym
            for (int i=0; i < druzyny.Count; i++)
            {
                for (int j = 0; j < druzyny.Count; j++)
                {
                    if (j != i)
                        spotkania.Add(new Spotkanie(druzyny[i], druzyny[j], LosujSedziow(sedziowie)));
                }
            }
        }

        public List<Druzyna> NajlepszeCztery()
        {
            List<Druzyna> najlepszeCztery = new List<Druzyna>(wyniki.Keys.Take(4));

            return najlepszeCztery;
        }
    }
}