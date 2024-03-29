﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    [Serializable]
    public class Spotkanie
    {
        public Druzyna Druzyna1 { get; private set; }
        public Druzyna Druzyna2 { get; private set; }
        public List<Osoba> Sedziowie { get; private set; }
        public Druzyna WygranaDruzyna { get; private set; }
        public bool CzyZakonczone { get; private set; }

        public Spotkanie()
        {
            CzyZakonczone = false;
        }

        public Spotkanie(Druzyna d1, Druzyna d2, List<Osoba> s)
        {
            Druzyna1 = d1;
            Druzyna2 = d2;
            Sedziowie = s;
            CzyZakonczone = false;
        }

        /// <summary>
        ///Funkcja konczy spotkanie i ustawia wygraną drużyne
        /// </summary>
        public void Zakoncz(Druzyna druzyna)
        {
            if (druzyna != Druzyna1 && druzyna != Druzyna2)
                throw new NieprawidlowaDruzynaException("Podana druzyna nie nalezy do spotkania");

            if (CzyZakonczone == true)
                throw new ZakonczoneSpotkanieException("Podane spotkanie jest zakonczone");

            CzyZakonczone = true;
            WygranaDruzyna = druzyna;
        }

        public List<Druzyna> GetDruzyny()
        {
            List<Druzyna> druzyny = new List<Druzyna>
            {
                Druzyna1,
                Druzyna2
            };

            return druzyny;
        }
    }
}
