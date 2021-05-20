using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    class Spotkanie
    {
        private Druzyna druzyna1;
        private Druzyna druzyna2;
        private List<Osoba> sedziowie;
        private Druzyna wygranaDruzyna;
        public bool CzyZakonczone { get; private set; }

        public Spotkanie(Druzyna d1, Druzyna d2, List<Osoba> s)
        {
            druzyna1 = d1;
            druzyna2 = d2;
            sedziowie = s;
            CzyZakonczone = false;
        }

        public void Zakoncz(Druzyna druzyna)
        {
            if (druzyna != druzyna1 && druzyna != druzyna2)
                throw new Exception("Podana druzyna nie nalezy do spotkania");

            CzyZakonczone = true;
            wygranaDruzyna = druzyna;
        }

        public Druzyna GetDruzyna1()
        {
            return druzyna1;
        }

        public Druzyna GetDruzyna2()
        {
            return druzyna2;
        }

        public List<Osoba> GetSedziowie()
        {
            return sedziowie;
        }

        public Druzyna GetWygranaDruzyna()
        {
            return wygranaDruzyna;
        }
    }
}
