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

            sedziowie.Add(new Osoba("Jan","Kowalski"));
            sedziowie.Add(new Osoba("Joachim","Mazur"));
            //sedziowie.Add(new Osoba("Allan","Wojciechowski"));
            //sedziowie.Add(new Osoba("Kryspin","Szymczak"));
            //sedziowie.Add(new Osoba("Mirosław","Wysocki"));

            druzyny.Add(new Druzyna("Alfa"));
            druzyny.Add(new Druzyna("Beta"));
            druzyny.Add(new Druzyna("Gamma"));
            druzyny.Add(new Druzyna("Delta"));
            druzyny.Add(new Druzyna("Bravo"));

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
            try
            {
                spotkanie.Zakoncz(druzyna);

                if (czyPolfinalRozpoczety)
                {
                    try
                    {
                        fazaFinalowa.DodajPunkt(druzyna);
                    }
                    catch (NieprawidlowaDruzynaException ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                    if (czyFinalRozpoczety)
                    {
                        lbl_zwyciezca.Content = druzyna.Nazwa;
                    }
                    else
                    {
                        if (lbl_FinalD1.Content.ToString() == "")
                            lbl_FinalD1.Content = druzyna.Nazwa;
                        else
                            lbl_FinalD2.Content = druzyna.Nazwa;
                    }

                }
                else
                {
                    try
                    {
                        fazaPoczatkowa.DodajPunkt(druzyna);
                    }
                    catch (NieprawidlowaDruzynaException ex)
                    {
                        MessageBox.Show(ex.Message);
                        throw;
                    }
                }

                listBox_spotkania.Items.Refresh();
                OdswiezTabliceWynikow();
            }
            catch(NieprawidlowaDruzynaException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            catch(ZakonczoneSpotkanieException ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

        }

        private void RozpocznijRozgrywke(TypGry typGry)
        {
            czyRozgrywkaRozpoczeta = true;

            try
            {
                fazaPoczatkowa = new FazaPoczatkowa(druzyny, sedziowie, typGry);
                listBox_spotkania.ItemsSource = fazaPoczatkowa.Spotkania();
                listBox_spotkania.Items.Refresh();

                OdswiezTabliceWynikow();

                PrzelaczInterfaceRozgryki();

            }
            catch (PustaListaSedziowException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ZbytMalaLiczbaSedziowException ex)
            {
                MessageBox.Show(ex.Message + ex.getNazw());
            }
            catch (BrakDruzynException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ZbytMalaLiczbaDruzynException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrzelaczInterfaceRozgryki()
        {
            if (czyFinalRozpoczety)
                InterfaceFinal();
            else if (czyPolfinalRozpoczety)
                InterfacePolFinal();
            else if (czyRozgrywkaRozpoczeta)
                WlaczInterfaceRozgrywka();
            else
                WylaczIngerfaceRozgrywka();
        }

        private void InterfaceFinal()
        {
            btn_WprowadzWynik.Visibility = Visibility.Visible;
            btn_RozpocznijFinal.Visibility = Visibility.Hidden;
        }

        private void InterfacePolFinal()
        {
            btn_WprowadzWynik.Visibility = Visibility.Visible;
            btn_RozpocznijPolfinal.Visibility = Visibility.Hidden;
            border_FazaFinalowa.Visibility = Visibility.Visible;
        }

        private void WlaczInterfaceRozgrywka()
        {
            btn_StartRozgrywka.Visibility = Visibility.Hidden;

            btn_UsunDruzyne.IsEnabled = false;
            btn_UsunSedziego.IsEnabled = false;
            groupBox_druzyny.Visibility = Visibility.Hidden;
            groupBox_sedziowie.Visibility = Visibility.Hidden;

            lbl_NazwaEtapu.Visibility = Visibility.Visible;

            border_Spotkania.Visibility = Visibility.Visible;

            btn_WprowadzWynik.Visibility = Visibility.Visible;
            btn_Podglad.Visibility = Visibility.Visible;

            separatorRozgrywka.Visibility = Visibility.Visible;

            lbl_tablicaWynikow.Visibility = Visibility.Visible;
            border_TablicaWynikow.Visibility = Visibility.Visible;
        }

        private void WylaczIngerfaceRozgrywka()
        {
            btn_StartRozgrywka.Visibility = Visibility.Visible;

            btn_UsunDruzyne.IsEnabled = true;
            btn_UsunSedziego.IsEnabled = true;
            groupBox_druzyny.Visibility = Visibility.Visible;
            groupBox_sedziowie.Visibility = Visibility.Visible;

            btn_RozpocznijPolfinal.Visibility = Visibility.Hidden;
            btn_RozpocznijFinal.Visibility = Visibility.Hidden;
            btn_ZakonczRozgrywke.Visibility = Visibility.Hidden;

            lbl_NazwaEtapu.Visibility = Visibility.Hidden;

            border_Spotkania.Visibility = Visibility.Hidden;

            btn_WprowadzWynik.Visibility = Visibility.Hidden;
            btn_Podglad.Visibility = Visibility.Hidden;

            separatorRozgrywka.Visibility = Visibility.Hidden;

            lbl_tablicaWynikow.Visibility = Visibility.Hidden;
            border_TablicaWynikow.Visibility = Visibility.Hidden;

            border_FazaFinalowa.Visibility = Visibility.Hidden;
        }

        private void OdswiezTabliceWynikow()
        {
            listBox_tablicaWynikow.ItemsSource = fazaPoczatkowa.TablicaWynikow();
            listBox_tablicaWynikow.Items.Refresh();
        }

        private void btn_RozpocznijPolfinal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                czyPolfinalRozpoczety = true;
                lbl_NazwaEtapu.Content = "Połfinał";

                List<Druzyna> najlepszeCztery = fazaPoczatkowa.NajlepszeCztery();

                fazaFinalowa = new FazaFinalowa(najlepszeCztery, sedziowie, fazaPoczatkowa.GetTyp());

                lbl_PolfinalD1.Content = najlepszeCztery[0];
                lbl_PolfinalD2.Content = najlepszeCztery[1];
                lbl_PolfinalD3.Content = najlepszeCztery[2];
                lbl_PolfinalD4.Content = najlepszeCztery[3];

                listBox_spotkania.ItemsSource = fazaFinalowa.Spotkania();
                listBox_spotkania.Items.Refresh();

                PrzelaczInterfaceRozgryki();
            }
            catch (ZbytMalaLiczbaDruzynException ex)
            {
                MessageBox.Show(ex.Message);

                czyRozgrywkaRozpoczeta = false;
                czyPolfinalRozpoczety = false;
                czyFinalRozpoczety = false;

                PrzelaczInterfaceRozgryki();

            }
}

        private void btn_RozpocznijFinal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                czyFinalRozpoczety = true;
                lbl_NazwaEtapu.Content = "Finał";

                Spotkanie spotkanie = fazaFinalowa.RozegrajFinal(sedziowie);

                listBox_spotkania.ItemsSource = null;
                listBox_spotkania.Items.Add(spotkanie);
                listBox_spotkania.Items.Refresh();

                PrzelaczInterfaceRozgryki();
            }
            catch (ZbytMalaLiczbaDruzynException ex)
            {
                MessageBox.Show(ex.Message);

                czyRozgrywkaRozpoczeta = false;
                czyPolfinalRozpoczety = false;
                czyFinalRozpoczety = false;

                PrzelaczInterfaceRozgryki();
            }
        }

        private void btn_ZakonczRozgrywke_Click(object sender, RoutedEventArgs e)
        {
            czyRozgrywkaRozpoczeta = false;
            czyPolfinalRozpoczety = false;
            czyFinalRozpoczety = false;
            listBox_spotkania.Items.Clear();

            PrzelaczInterfaceRozgryki();
        }

        private void tbx_NazwaDruzyna_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbx_NazwaDruzyna.Text == "")
                btn_DodajDruzyne.IsEnabled = false;
            else
                btn_DodajDruzyne.IsEnabled = true;
        }

        private void tbx_ImieSedzia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbx_ImieSedzia.Text == "" || tbx_NazwiskoSedzia.Text == "")
                btn_DodajSedziego.IsEnabled = false;
            else
                btn_DodajSedziego.IsEnabled = true;
        }

        private void tbx_NazwiskoSedzia_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbx_ImieSedzia.Text == "" || tbx_NazwiskoSedzia.Text == "")
                btn_DodajSedziego.IsEnabled = false;
            else
                btn_DodajSedziego.IsEnabled = true;
        }

        private void btn_Podglad_Click(object sender, RoutedEventArgs e)
        {
            Spotkanie spotkanie = (Spotkanie)listBox_spotkania.SelectedItem;

            DlgSpotkanie dlg = new DlgSpotkanie(spotkanie, true);

            dlg.ShowDialog();
        }

        private void listBox_spotkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_Podglad.IsEnabled = true;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            //zapis stanu do pliku
        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            //odczyt stanu z pliku
        }
    }
}
