﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class FazaFinalowa : Rozgrywka
    {
        public FazaFinalowa(List<Druzyna> druzyny, List<Osoba> sedziowie, TypGry typ)
            : base(druzyny, typ)
        {
            //utworzenie dwóch spotkań półfinałowych
            spotkania.Add(new Spotkanie(druzyny[0], druzyny[1], LosujSedziow(sedziowie)));
            spotkania.Add(new Spotkanie(druzyny[2], druzyny[3], LosujSedziow(sedziowie)));
        }

        public Spotkanie RozegrajFinal(List<Osoba> sedziowie)
        {
            //pobranie dwóch wygranych drużyn z tablicy wyników
            List<Druzyna> finaloweDruzyny = new List<Druzyna>(from kvp in TablicaWynikow() select kvp.Key).Take(2).ToList();

            //utworzenie finalowego spotkania
            Spotkanie final = new Spotkanie(finaloweDruzyny[0], finaloweDruzyny[1], LosujSedziow(sedziowie));
            spotkania.Add(final);

            return final;
        }

    }
}
