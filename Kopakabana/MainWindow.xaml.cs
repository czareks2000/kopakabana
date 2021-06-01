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
    /// Klasa MainWindow zarządza logiką interfejsu użytkownika
    /// </summary>
    public partial class MainWindow : Window
    {
        private StanProgramu stan;
        /// <summary>
        /// Konstruktor głównego okna
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //utworzenie stanu programu        
            stan = new StanProgramu();

            PrzelaczInterfaceRozgryki();

            listBox_druzyny.ItemsSource = stan.Druzyny;
            listBox_sedziowie.ItemsSource = stan.Sedziowie;
        }

        #region Zarządzanie Sędziami
        /// <summary>
        /// Obsługa przycisku dodawania sędziów
        /// </summary>
        private void btn_DodajSedziego_Click(object sender, RoutedEventArgs e)
        {
            stan.Sedziowie.Add(new Osoba(tbx_ImieSedzia.Text, tbx_NazwiskoSedzia.Text));
            tbx_ImieSedzia.Clear();
            tbx_NazwiskoSedzia.Clear();
            listBox_sedziowie.Items.Refresh();
        }
        /// <summary>
        /// Obsługa przycisku usuwania sędziów
        /// </summary>
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
        /// <summary>
        /// Obsługa przycisku dodawania drużyny
        /// </summary>
        private void btn_DodajDruzyne_Click(object sender, RoutedEventArgs e)
        {
            stan.Druzyny.Add(new Druzyna(tbx_NazwaDruzyna.Text));
            tbx_NazwaDruzyna.Clear();
            listBox_druzyny.Items.Refresh();
        }
        /// <summary>
        /// Obsługa przycisku usuwania drużyny
        /// </summary>
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
        /// <summary>
        /// Obsługa przycisku rozpoczynającego rozgrywkę
        /// </summary>
        private void btn_StartRozgrywka_Click(object sender, RoutedEventArgs e)
        {
            //okno w którym wybieramy typ rozgrywki
            DlgTypGry dlg = new DlgTypGry();
            if (true == dlg.ShowDialog())
            {
                RozpocznijRozgrywke((TypGry)dlg.cbx_TypGry.SelectedItem);
            }
        }
        /// <summary>
        /// Funkcja rozpoczynająca rozgrywkę, wywoływana po wciśnięciu przyciusku Rozpocznij Rozgrywkę
        /// </summary>
        private void RozpocznijRozgrywke(TypGry typGry)
        {
            try
            {
                //utworzenia rozgrywki
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
        /// <summary>
        /// Obsługa przycisku rozpoczynającego półfinał
        /// </summary>
        private void btn_RozpocznijPolfinal_Click(object sender, RoutedEventArgs e)
        {
            lbl_NazwaEtapu.Content = "Połfinał";

            //pobranie 4 najlepszych drużyn z tabeli wyników
            List<Druzyna> najlepszeCztery = stan.FazaPoczatkowa.NajlepszeCztery();

            stan.FazaFinalowa = new FazaFinalowa(najlepszeCztery, stan.Sedziowie, stan.FazaPoczatkowa.GetTyp());

            lbl_PolfinalD1.Content = najlepszeCztery[0];
            lbl_PolfinalD2.Content = najlepszeCztery[1];
            lbl_PolfinalD3.Content = najlepszeCztery[2];
            lbl_PolfinalD4.Content = najlepszeCztery[3];

            lbl_FinalD1.Content = null;
            lbl_FinalD2.Content = null;

            listBox_spotkania.ItemsSource = stan.FazaFinalowa.Spotkania();
            listBox_spotkania.Items.Refresh();

            stan.CzyPolfinalRozpoczety = true;

            PrzelaczInterfaceRozgryki();
        }
        /// <summary>
        /// Obsługa przycisku rozpoczynającego finał
        /// </summary>
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
        /// <summary>
        /// Obsługa przycisku kończącego rozgrywkę
        /// </summary>
        private void btn_ZakonczRozgrywke_Click(object sender, RoutedEventArgs e)
        {
            //ustawianie zmiennych na podstawowe
            stan.CzyRozgrywkaRozpoczeta = false;
            stan.CzyPolfinalRozpoczety = false;
            stan.CzyFinalRozpoczety = false;
            listBox_spotkania.Items.Clear();

            PrzelaczInterfaceRozgryki();
            ResetInterfaceu();
        }
        /// <summary>
        /// Obsługa przycisku zatwierdzającego wybór wygranej drużyny
        /// </summary>
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
        /// <summary>
        /// Funkcja umożliwiająca wprowadzenie wygranej drużyny
        /// </summary>
        private void WprowadzWynik(Spotkanie spotkanie, Druzyna druzyna)
        {
            try
            {
                //zakończenia spotkania
                spotkanie.Zakoncz(druzyna);

                //sprawdzenie w jakiej fazie jest rozgrywka (od stanu rozgrywki zależy czy drużyna dostanie punkt do tabeli, czy przejdzie do dalszej fazy rozgrywek)
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
        /// <summary>
        /// Funkcja odpowiada za zresetowanie interfejsu
        /// </summary>
        private void ResetInterfaceu(bool czyWczytywanieStanu = false)
        {
            WylaczIngerfaceRozgrywka();
            // jeżeli funkcja wywołana wywołana jest przy wczytywaniu stanu, resetuje listy sędziów i drużyn
            if (czyWczytywanieStanu)
            {
                listBox_sedziowie.ItemsSource = null;
                listBox_sedziowie.Items.Clear();
                listBox_druzyny.ItemsSource = null;
                listBox_druzyny.Items.Clear();
            }
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
        /// <summary>
        /// Funkcja odpowiada za przełączenie interfejsu graficznego
        /// </summary>
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
        /// <summary>
        /// Interfejs graficzny finału
        /// </summary>
        private void InterfaceFinal()
        {
            btn_WprowadzWynik.Visibility = Visibility.Visible;
            btn_RozpocznijFinal.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Interfejs graficzny półfinału
        /// </summary>
        private void InterfacePolFinal()
        {
            btn_WprowadzWynik.Visibility = Visibility.Visible;
            btn_RozpocznijPolfinal.Visibility = Visibility.Hidden;
            border_FazaFinalowa.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Funkcja włącza interfejs rozgrywki
        /// </summary>
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
        /// <summary>
        /// Funkcja wyłącza interfejs rozgrywki
        /// </summary>
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
        /// <summary>
        /// Funckja sprawdza czy aktualny etap jest zakończony
        /// Jeżeli jest zakończony, rozpoczyna kolejny etap
        /// </summary>
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
        /// <summary>
        /// Funkcja odświeża tablicę wyników
        /// </summary>
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
        /// <summary>
        /// Funkcja wyświetla okno ze szczegółami wybranego spotkania
        /// </summary>
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
        /// <summary>
        /// Obsługa przycisku zapisującego stan gry
        /// </summary>
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
        /// <summary>
        /// Obsługa przycisku wczytującego stan gry
        /// </summary>
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
        /// <summary>
        /// Funkcja wczytująca stan gry
        /// </summary>
        private void WczytajStanProgramu(StanProgramu s)
        {
            stan = s;

            ResetInterfaceu(true);

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
        /// <summary>
        /// Funkcja wczytująca faze finałową
        /// </summary>
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
        /// <summary>
        /// Funkcja wczytująca faze półfinałową
        /// </summary>
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
        /// <summary>
        /// Funkcja wczytująca faze początkową
        /// </summary>
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
