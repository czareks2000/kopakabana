using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana
{
    [Serializable]
    public class StanProgramu
    {
        public List<Druzyna> Druzyny { get; set; }
        public List<Osoba> Sedziowie { get; set; }
        public FazaPoczatkowa FazaPoczatkowa { get; set; }
        public FazaFinalowa FazaFinalowa { get; set; }
        public bool CzyRozgrywkaRozpoczeta { get; set; }
        public bool CzyPolfinalRozpoczety { get; set; }
        public bool CzyFinalRozpoczety { get; set; }

        public StanProgramu()
        {
            Druzyny = new List<Druzyna>();
            Sedziowie = new List<Osoba>();
            CzyRozgrywkaRozpoczeta = false;
            CzyPolfinalRozpoczety = false;
            CzyFinalRozpoczety = false;
        }

    }
}
