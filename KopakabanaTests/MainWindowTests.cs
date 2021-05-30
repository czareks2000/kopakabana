using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kopakabana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kopakabana.Tests
{
    [TestClass()]
    public class RozgrywkaTests
    {
        StanProgramu stan;
        Osoba osoba1;
        Druzyna druzyna1;

        [TestInitialize()]
        public void SpotkanieTestsInitialize()
        {
            stan = new StanProgramu();
            osoba1 = new Osoba("Janek", "Franek");
            stan.Sedziowie.Add(new Osoba("Jan", "Kowalski"));
            stan.Sedziowie.Add(new Osoba("Joachim", "Mazur"));
            stan.Sedziowie.Add(new Osoba("Allan", "Wojciechowski"));

            druzyna1 = new Druzyna("druzyna");
            stan.Druzyny.Add(new Druzyna("Alfa"));
            stan.Druzyny.Add(new Druzyna("Beta"));
            stan.Druzyny.Add(new Druzyna("Gamma"));
            stan.Druzyny.Add(new Druzyna("Delta"));
            stan.Druzyny.Add(new Druzyna("Bravo"));
        }

        [TestMethod()]
        public void DodawanieOsob()
        {
            stan.Sedziowie.Add(osoba1);
            Assert.IsTrue(stan.Sedziowie.Contains(osoba1));
        }

        [TestMethod()]
        public void DodawanieDruzyn()
        {
            stan.Druzyny.Add(druzyna1);
            Assert.IsTrue(stan.Druzyny.Contains(druzyna1));
        }

        [TestMethod()]
        public void CzyZakonczonePodstawoweUstawienia()
        {
            Spotkanie spotkanie1 = new Spotkanie(stan.Druzyny[0], stan.Druzyny[1], stan.Sedziowie);
            Assert.IsFalse(spotkanie1.CzyZakonczone);
        }

        [TestMethod()]
        public void CzyZakonczonePoZmianie()
        {
            Spotkanie spotkanie1 = new Spotkanie(stan.Druzyny[0], stan.Druzyny[1], stan.Sedziowie);
            spotkanie1.Zakoncz(stan.Druzyny[0]);
            Assert.IsTrue(spotkanie1.CzyZakonczone);
        }

        [TestMethod()]
        public void ZakonczSpotkanieException()
        {
            stan.Druzyny.Add(new Druzyna("Alfa"));
            stan.Druzyny.Add(new Druzyna("Beta"));
            Spotkanie spotkanie1 = new Spotkanie(stan.Druzyny[0], stan.Druzyny[1], stan.Sedziowie);
            spotkanie1.Zakoncz(stan.Druzyny[0]);
            Assert.ThrowsException<ZakonczoneSpotkanieException>(() => spotkanie1.Zakoncz(stan.Druzyny[0]));
        }

        [TestMethod()]
        public void WygranaDruzyna()
        {
            Spotkanie spotkanie1 = new Spotkanie(stan.Druzyny[0], stan.Druzyny[1], stan.Sedziowie);
            spotkanie1.Zakoncz(stan.Druzyny[0]);
            Assert.IsTrue(spotkanie1.WygranaDruzyna == stan.Druzyny[0]);
        }

        [TestMethod()]
        public void UsuwanieieOsob()
        {
            stan.Sedziowie.Remove(osoba1);
            Assert.IsFalse(stan.Sedziowie.Contains(osoba1));
        }

        [TestMethod()]
        public void UsuwanieDruzyn()
        {
            stan.Druzyny.Remove(druzyna1);
            Assert.IsFalse(stan.Druzyny.Contains(druzyna1));
        }
    }
}