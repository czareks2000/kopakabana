using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kopakabana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Druzyna> druzyny = new List<Druzyna>();
        private List<Osoba> sedziowie = new List<Osoba>();
        private FazaPoczatkowa fazaPoczatkowa;
        private FazaFinalowa fazaFinalowa;
        private bool czyRozgrywkaRozpoczeta = false;
        private bool czyPolfinalRozpoczety = false;
        private bool czyFinalRozpoczety = false;

        public MainWindow()
        {
            InitializeComponent();

            sedziowie.Add(new Osoba("1","1"));
            sedziowie.Add(new Osoba("2","2"));
            sedziowie.Add(new Osoba("3","3"));
            sedziowie.Add(new Osoba("4","4"));
            sedziowie.Add(new Osoba("5","5"));

            druzyny.Add(new Druzyna("1"));
            druzyny.Add(new Druzyna("2"));
            druzyny.Add(new Druzyna("3"));
            druzyny.Add(new Druzyna("4"));
            druzyny.Add(new Druzyna("5"));

            PrzelaczInterfaceRozgryki();

            listBox_druzyny.ItemsSource = druzyny;
            listBox_sedziowie.ItemsSource = sedziowie;
        }

        private void btn_DodajSedziego_Click(object sender, RoutedEventArgs e)
        {
            sedziowie.Add(new Osoba(tbx_ImieSedzia.Text, tbx_NazwiskoSedzia.Text));
            tbx_ImieSedzia.Clear();
            tbx_NazwiskoSedzia.Clear();
            listBox_sedziowie.Items.Refresh();
        }

        private void btn_DodajDruzyne_Click(object sender, RoutedEventArgs e)
        {
            druzyny.Add(new Druzyna(tbx_NazwaDruzyna.Text));
            tbx_NazwaDruzyna.Clear();
            listBox_druzyny.Items.Refresh();
        }

        private void btn_UsunSedziego_Click(object sender, RoutedEventArgs e)
        {
            sedziowie.RemoveAt(listBox_sedziowie.SelectedIndex);
            listBox_sedziowie.Items.Refresh();
        }

        private void btn_UsunDruzyne_Click(object sender, RoutedEventArgs e)
        {
            druzyny.RemoveAt(listBox_druzyny.SelectedIndex);
            listBox_druzyny.Items.Refresh();
        }

        private void btn_StartRozgrywka_Click(object sender, RoutedEventArgs e)
        {
            DlgTypGry dlg = new DlgTypGry();
            if (true == dlg.ShowDialog())
            {
                RozpocznijRozgrywke((TypGry)dlg.cbx_TypGry.SelectedItem);
            }
        }

        private void btn_WprowadzWynik_Click(object sender, RoutedEventArgs e)
        {
            Spotkanie spotkanie;
            if (czyPolfinalRozpoczety)
                spotkanie = fazaFinalowa.KolejneSpotkanie();
            else
                spotkanie = fazaPoczatkowa.KolejneSpotkanie();

            DlgSpotkanie dlg = new DlgSpotkanie(spotkanie);
            
            if (true == dlg.ShowDialog())
            {
                WprowadzWynik(spotkanie, (Druzyna)dlg.cb_wygranaDruzyna.SelectedItem);

                KolejnyEtap();
            }
        }

        private void KolejnyEtap()
        {
            if (czyFinalRozpoczety)
            {
                if (fazaFinalowa.KolejneSpotkanie() == null)
                {
                    btn_WprowadzWynik.Visibility = Visibility.Hidden;
                    btn_ZakonczRozgrywke.Visibility = Visibility.Visible;
                }
                return;
            }

            if (czyPolfinalRozpoczety)
            {
                if (fazaFinalowa.KolejneSpotkanie() == null)
                {
                    btn_WprowadzWynik.Visibility = Visibility.Hidden;
                    btn_RozpocznijFinal.Visibility = Visibility.Visible;
                }
                return;
            }

            if (fazaPoczatkowa.KolejneSpotkanie() == null)
            {
                btn_WprowadzWynik.Visibility = Visibility.Hidden;
                btn_RozpocznijPolfinal.Visibility = Visibility.Visible;
            }
        }

        private void WprowadzWynik(Spotkanie spotkanie, Druzyna druzyna)
        {
            spotkanie.Zakoncz(druzyna);

            fazaPoczatkowa.DodajPunkt(druzyna);
            if(czyPolfinalRozpoczety)
                fazaFinalowa.DodajPunkt(druzyna);

            listBox_spotkania.Items.Refresh();
            OdswiezTabliceWynikow();
        }

        private void RozpocznijRozgrywke(TypGry typGry)
        {
            fazaPoczatkowa = new FazaPoczatkowa(druzyny, sedziowie, typGry);

            czyRozgrywkaRozpoczeta = true;

            PrzelaczInterfaceRozgryki();

            listBox_spotkania.ItemsSource = fazaPoczatkowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            OdswiezTabliceWynikow();
        }

        private void PrzelaczInterfaceRozgryki()
        {
            if (czyFinalRozpoczety)
            {
                btn_WprowadzWynik.Visibility = Visibility.Visible;
                btn_RozpocznijFinal.Visibility = Visibility.Hidden;
            }
            else if (czyPolfinalRozpoczety)
            {
                btn_WprowadzWynik.Visibility = Visibility.Visible;
                btn_RozpocznijPolfinal.Visibility = Visibility.Hidden;
            }
            else if (czyRozgrywkaRozpoczeta)
            {
                btn_StartRozgrywka.Visibility = Visibility.Hidden;

                lbl_NazwaEtapu.Visibility = Visibility.Visible;
                listBox_spotkania.Visibility = Visibility.Visible;
                btn_WprowadzWynik.Visibility = Visibility.Visible;

                separatorRozgrywka.Visibility = Visibility.Visible;

                border_TablicaWynikow.Visibility = Visibility.Visible;
                separator1_TablicaWynikow.Visibility = Visibility.Visible;
                separator2_TablicaWynikow.Visibility = Visibility.Visible;
                lbl_tablicaWynikow.Visibility = Visibility.Visible;
                listBox_tablicaWynikow.Visibility = Visibility.Visible;
            }
            else
            {
                btn_StartRozgrywka.Visibility = Visibility.Visible;

                btn_RozpocznijPolfinal.Visibility = Visibility.Hidden;
                btn_RozpocznijFinal.Visibility = Visibility.Hidden;
                btn_ZakonczRozgrywke.Visibility = Visibility.Hidden;

                lbl_NazwaEtapu.Visibility = Visibility.Hidden;
                listBox_spotkania.Visibility = Visibility.Hidden;
                btn_WprowadzWynik.Visibility = Visibility.Hidden;

                separatorRozgrywka.Visibility = Visibility.Hidden;

                border_TablicaWynikow.Visibility = Visibility.Hidden;
                separator1_TablicaWynikow.Visibility = Visibility.Hidden;
                separator2_TablicaWynikow.Visibility = Visibility.Hidden;
                lbl_tablicaWynikow.Visibility = Visibility.Hidden;
                listBox_tablicaWynikow.Visibility = Visibility.Hidden;
            }
        }

        private void OdswiezTabliceWynikow()
        {
            listBox_tablicaWynikow.ItemsSource = fazaPoczatkowa.TablicaWynikow();
            listBox_tablicaWynikow.Items.Refresh();
        }

        private void btn_RozpocznijPolfinal_Click(object sender, RoutedEventArgs e)
        {
            czyPolfinalRozpoczety = true;

            fazaFinalowa = new FazaFinalowa(fazaPoczatkowa.NajlepszeCztery(), sedziowie, fazaPoczatkowa.GetTyp());

            lbl_NazwaEtapu.Content = "Połfinał";

            listBox_spotkania.ItemsSource = fazaFinalowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            PrzelaczInterfaceRozgryki();
        }

        private void btn_RozpocznijFinal_Click(object sender, RoutedEventArgs e)
        {
            czyFinalRozpoczety = true;

            lbl_NazwaEtapu.Content = "Finał";
            listBox_spotkania.ItemsSource = null;
            listBox_spotkania.Items.Add(fazaFinalowa.RozegrajFinal(sedziowie));

            listBox_spotkania.Items.Refresh();

            PrzelaczInterfaceRozgryki();
        }

        private void btn_ZakonczRozgrywke_Click(object sender, RoutedEventArgs e)
        {
            czyRozgrywkaRozpoczeta = false;
            czyPolfinalRozpoczety = false;
            czyFinalRozpoczety = false;

            PrzelaczInterfaceRozgryki();
        }
    }
}
