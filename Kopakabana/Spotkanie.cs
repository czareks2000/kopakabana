using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    public class Spotkanie
    {
        public Druzyna Druzyna1 { get; private set; }
        public Druzyna Druzyna2 { get; private set; }
        private List<Osoba> sedziowie;
        public Druzyna WygranaDruzyna { get; private set; }
        public bool CzyZakonczone { get; private set; }

        public Spotkanie(Druzyna d1, Druzyna d2, List<Osoba> s)
        {
            Druzyna1 = d1;
            Druzyna2 = d2;
            sedziowie = s;
            CzyZakonczone = false;
        }

        public void Zakoncz(Druzyna druzyna)
        {
            if (druzyna != Druzyna1 && druzyna != Druzyna2)
                throw new Exception("Podana druzyna nie nalezy do spotkania");

            CzyZakonczone = true;
            WygranaDruzyna = druzyna;
        }

        public List<Osoba> GetSedziowie()
        {
            return sedziowie;
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
