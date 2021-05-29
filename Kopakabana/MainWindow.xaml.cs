using System;
using System.Collections.Generic;
using System.IO;
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
        private StanProgramu stan;

        public MainWindow()
        {
            InitializeComponent();

            stan = new StanProgramu();

            //stan.Sedziowie.Add(new Osoba("Jan","Kowalski"));
            //stan.Sedziowie.Add(new Osoba("Joachim","Mazur"));
            //stan.Sedziowie.Add(new Osoba("Allan","Wojciechowski"));
            //stan.Sedziowie.Add(new Osoba("Kryspin","Szymczak"));
            //stan.Sedziowie.Add(new Osoba("Mirosław","Wysocki"));

            //stan.Druzyny.Add(new Druzyna("Alfa"));
            //stan.Druzyny.Add(new Druzyna("Beta"));
            //stan.Druzyny.Add(new Druzyna("Gamma"));
            //stan.Druzyny.Add(new Druzyna("Delta"));
            //stan.Druzyny.Add(new Druzyna("Bravo"));

            PrzelaczInterfaceRozgryki();

            listBox_druzyny.ItemsSource = stan.Druzyny;
            listBox_sedziowie.ItemsSource = stan.Sedziowie;
        }

        #region Zarządzanie Sędziami

        private void btn_DodajSedziego_Click(object sender, RoutedEventArgs e)
        {
            stan.Sedziowie.Add(new Osoba(tbx_ImieSedzia.Text, tbx_NazwiskoSedzia.Text));
            tbx_ImieSedzia.Clear();
            tbx_NazwiskoSedzia.Clear();
            listBox_sedziowie.Items.Refresh();
        }

        private void btn_UsunSedziego_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_sedziowie.SelectedIndex != -1)
            {
                stan.Sedziowie.RemoveAt(listBox_sedziowie.SelectedIndex);
                listBox_sedziowie.Items.Refresh();
            }
        }

        #endregion

        #region Zarządzanie Drużynami

        private void btn_DodajDruzyne_Click(object sender, RoutedEventArgs e)
        {
            stan.Druzyny.Add(new Druzyna(tbx_NazwaDruzyna.Text));
            tbx_NazwaDruzyna.Clear();
            listBox_druzyny.Items.Refresh();
        }

        private void btn_UsunDruzyne_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_druzyny.SelectedIndex != -1)
            {
                stan.Druzyny.RemoveAt(listBox_druzyny.SelectedIndex);
                listBox_druzyny.Items.Refresh();
            }
        }

        #endregion

        #region Zarządzanie Rozgrywką

        private void btn_StartRozgrywka_Click(object sender, RoutedEventArgs e)
        {
            DlgTypGry dlg = new DlgTypGry();
            if (true == dlg.ShowDialog())
            {
                RozpocznijRozgrywke((TypGry)dlg.cbx_TypGry.SelectedItem);
            }
        }

        private void RozpocznijRozgrywke(TypGry typGry)
        {
            try
            {
                stan.FazaPoczatkowa = new FazaPoczatkowa(stan.Druzyny, stan.Sedziowie, typGry);
                listBox_spotkania.ItemsSource = stan.FazaPoczatkowa.Spotkania();
                listBox_spotkania.Items.Refresh();

                stan.CzyRozgrywkaRozpoczeta = true;

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

        private void btn_RozpocznijPolfinal_Click(object sender, RoutedEventArgs e)
        {
            lbl_NazwaEtapu.Content = "Połfinał";

            List<Druzyna> najlepszeCztery = stan.FazaPoczatkowa.NajlepszeCztery();

            stan.FazaFinalowa = new FazaFinalowa(najlepszeCztery, stan.Sedziowie, stan.FazaPoczatkowa.GetTyp());

            lbl_PolfinalD1.Content = najlepszeCztery[0];
            lbl_PolfinalD2.Content = najlepszeCztery[1];
            lbl_PolfinalD3.Content = najlepszeCztery[2];
            lbl_PolfinalD4.Content = najlepszeCztery[3];

            listBox_spotkania.ItemsSource = stan.FazaFinalowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            stan.CzyPolfinalRozpoczety = true;

            PrzelaczInterfaceRozgryki();
        }

        private void btn_RozpocznijFinal_Click(object sender, RoutedEventArgs e)
        {
            lbl_NazwaEtapu.Content = "Finał";

            Spotkanie spotkanie = stan.FazaFinalowa.RozegrajFinal(stan.Sedziowie);

            listBox_spotkania.ItemsSource = null;
            listBox_spotkania.Items.Add(spotkanie);
            listBox_spotkania.Items.Refresh();

            stan.CzyFinalRozpoczety = true;

            PrzelaczInterfaceRozgryki();
        }

        private void btn_ZakonczRozgrywke_Click(object sender, RoutedEventArgs e)
        {
            stan.CzyRozgrywkaRozpoczeta = false;
            stan.CzyPolfinalRozpoczety = false;
            stan.CzyFinalRozpoczety = false;
            listBox_spotkania.Items.Clear();

            PrzelaczInterfaceRozgryki();
        }

        private void btn_WprowadzWynik_Click(object sender, RoutedEventArgs e)
        {
            Spotkanie spotkanie;
            if (stan.CzyPolfinalRozpoczety)
                spotkanie = stan.FazaFinalowa.KolejneSpotkanie();
            else
                spotkanie = stan.FazaPoczatkowa.KolejneSpotkanie();

            DlgSpotkanie dlg = new DlgSpotkanie(spotkanie);

            if (true == dlg.ShowDialog())
            {
                WprowadzWynik(spotkanie, (Druzyna)dlg.cb_wygranaDruzyna.SelectedItem);
                KolejnyEtap();
            }
        }

        private void WprowadzWynik(Spotkanie spotkanie, Druzyna druzyna)
        {
            try
            {
                spotkanie.Zakoncz(druzyna);

                if (stan.CzyPolfinalRozpoczety)
                {

                    stan.FazaFinalowa.DodajPunkt(druzyna);

                    if (stan.CzyFinalRozpoczety)
                    {
                        lbl_zwyciezca.Content = druzyna.Nazwa;
                    }
                    else
                    {
                        if (lbl_FinalD1.Content == null)
                            lbl_FinalD1.Content = druzyna.Nazwa;
                        else
                            lbl_FinalD2.Content = druzyna.Nazwa;
                    }

                }
                else
                {

                    stan.FazaPoczatkowa.DodajPunkt(druzyna);
                }

                listBox_spotkania.Items.Refresh();
                OdswiezTabliceWynikow();
            }
            catch (NieprawidlowaDruzynaException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ZakonczoneSpotkanieException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        #region Zarządzanie UI

        private void ResetInterfaceu()
        {
            WylaczIngerfaceRozgrywka();
            listBox_sedziowie.ItemsSource = null;
            listBox_sedziowie.Items.Clear();
            listBox_druzyny.ItemsSource = null;
            listBox_druzyny.Items.Clear();
            listBox_spotkania.ItemsSource = null;
            listBox_spotkania.Items.Clear();
            listBox_tablicaWynikow.ItemsSource = null;
            listBox_tablicaWynikow.Items.Clear();
            lbl_zwyciezca.Content = null;
            lbl_FinalD1.Content = null;
            lbl_FinalD2.Content = null;
            lbl_PolfinalD1.Content = null;
            lbl_PolfinalD2.Content = null;
            lbl_PolfinalD3.Content = null;
            lbl_PolfinalD4.Content = null;
        }

        private void PrzelaczInterfaceRozgryki()
        {
            if (stan.CzyFinalRozpoczety)
                InterfaceFinal();
            else if (stan.CzyPolfinalRozpoczety)
                InterfacePolFinal();
            else if (stan.CzyRozgrywkaRozpoczeta)
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

        private void KolejnyEtap()
        {
            if (stan.CzyFinalRozpoczety)
            {
                if (stan.FazaFinalowa.KolejneSpotkanie() == null)
                {
                    btn_WprowadzWynik.Visibility = Visibility.Hidden;
                    btn_ZakonczRozgrywke.Visibility = Visibility.Visible;
                }
                return;
            }

            if (stan.CzyPolfinalRozpoczety)
            {
                if (stan.FazaFinalowa.KolejneSpotkanie() == null)
                {
                    btn_WprowadzWynik.Visibility = Visibility.Hidden;
                    btn_RozpocznijFinal.Visibility = Visibility.Visible;
                }
                return;
            }

            if (stan.FazaPoczatkowa.KolejneSpotkanie() == null)
            {
                btn_WprowadzWynik.Visibility = Visibility.Hidden;
                btn_RozpocznijPolfinal.Visibility = Visibility.Visible;
            }
        }

        private void OdswiezTabliceWynikow()
        {
            listBox_tablicaWynikow.ItemsSource = stan.FazaPoczatkowa.TablicaWynikow();
            listBox_tablicaWynikow.Items.Refresh();
        }

        private void tbx_NazwaDruzyna_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbx_NazwaDruzyna.Text == "")
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

        #endregion

        #region Zapis i odczyt stanu programu

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "stanProgramu",
                DefaultExt = ".data",
                Filter = "Data files|*.data"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
                BinarySerializer.Serialize(new FileInfo(dlg.FileName), stan);

        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "stanProgramu.data",
                DefaultExt = ".data",
                Filter = "Data files|*.data"
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
                WczytajStanProgramu((StanProgramu)BinarySerializer.Deserialize(new FileInfo(dlg.FileName)));

        }

        private void WczytajStanProgramu(StanProgramu s)
        {
            stan = s;

            ResetInterfaceu();

            if (stan.CzyFinalRozpoczety)
            {
                WczytajFazeFinalowa();
            }
            else if (stan.CzyPolfinalRozpoczety)
            {
                WczytajFazePolfinalowa();
            }
            else if (stan.CzyRozgrywkaRozpoczeta)
            {
                WczytajFazePoczatkowa();
            }

            listBox_druzyny.ItemsSource = stan.Druzyny;
            listBox_sedziowie.ItemsSource = stan.Sedziowie;
            listBox_sedziowie.Items.Refresh();
            listBox_druzyny.Items.Refresh();
        }

        private void WczytajFazeFinalowa()
        {
            lbl_NazwaEtapu.Content = "Finał";
            listBox_spotkania.Items.Add(stan.FazaFinalowa.Final);
            listBox_spotkania.Items.Refresh();

            List<Druzyna> najlepszeCztery = stan.FazaPoczatkowa.NajlepszeCztery();

            lbl_PolfinalD1.Content = najlepszeCztery[0];
            lbl_PolfinalD2.Content = najlepszeCztery[1];
            lbl_PolfinalD3.Content = najlepszeCztery[2];
            lbl_PolfinalD4.Content = najlepszeCztery[3];

            lbl_FinalD1.Content = stan.FazaFinalowa.Final.Druzyna1.Nazwa;
            lbl_FinalD2.Content = stan.FazaFinalowa.Final.Druzyna2.Nazwa;

            if (stan.FazaFinalowa.Final.CzyZakonczone)
                lbl_zwyciezca.Content = stan.FazaFinalowa.Final.WygranaDruzyna.Nazwa;
                
            WlaczInterfaceRozgrywka();
            InterfacePolFinal();
            InterfaceFinal();
            OdswiezTabliceWynikow();
            KolejnyEtap();
        }

        private void WczytajFazePolfinalowa()
        {
            lbl_NazwaEtapu.Content = "Połfinał";

            List<Druzyna> najlepszeCztery = stan.FazaPoczatkowa.NajlepszeCztery();

            lbl_PolfinalD1.Content = najlepszeCztery[0];
            lbl_PolfinalD2.Content = najlepszeCztery[1];
            lbl_PolfinalD3.Content = najlepszeCztery[2];
            lbl_PolfinalD4.Content = najlepszeCztery[3];

            List<Spotkanie> spotkania = stan.FazaFinalowa.Spotkania();

            if (spotkania[0].CzyZakonczone)
                lbl_FinalD1.Content = spotkania[0].WygranaDruzyna.Nazwa;

            if (spotkania[1].CzyZakonczone)
                lbl_FinalD2.Content = spotkania[1].WygranaDruzyna.Nazwa;

            listBox_spotkania.ItemsSource = stan.FazaFinalowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            WlaczInterfaceRozgrywka();
            InterfacePolFinal();
            OdswiezTabliceWynikow();
        }

        private void WczytajFazePoczatkowa()
        {
            lbl_NazwaEtapu.Content = "Faza Początkowa";

            listBox_spotkania.ItemsSource = stan.FazaPoczatkowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            WlaczInterfaceRozgrywka();
            OdswiezTabliceWynikow();
        }

        #endregion

    }
}
