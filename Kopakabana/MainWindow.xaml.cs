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

        public MainWindow()
        {
            InitializeComponent();

            lbl_NazwaEtapu.Visibility = Visibility.Hidden;
            listBox_spotkania.Visibility = Visibility.Hidden;
            separatorRozgrywka.Visibility = Visibility.Hidden;
            lbl_tablicaWynikow.Visibility = Visibility.Hidden;
            listBox_tablicaWynikow.Visibility = Visibility.Hidden;
            btn_WprowadzWynik.Visibility = Visibility.Hidden;
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
                fazaPoczatkowa = new FazaPoczatkowa(druzyny, sedziowie, (TypGry)dlg.cbx_TypGry.SelectedItem);

                czyRozgrywkaRozpoczeta = true;
                btn_StartRozgrywka.Visibility = Visibility.Hidden;
                lbl_NazwaEtapu.Visibility = Visibility.Visible;
                listBox_spotkania.Visibility = Visibility.Visible;
                separatorRozgrywka.Visibility = Visibility.Visible;
                lbl_tablicaWynikow.Visibility = Visibility.Visible;
                listBox_tablicaWynikow.Visibility = Visibility.Visible;
                btn_WprowadzWynik.Visibility = Visibility.Visible;
                listBox_spotkania.ItemsSource = fazaPoczatkowa.Spotkania();
                listBox_spotkania.Items.Refresh();
                listBox_tablicaWynikow.ItemsSource = fazaPoczatkowa.TablicaWynikow();
                listBox_tablicaWynikow.Items.Refresh();
            }

        }

        private void btn_WprowadzWynik_Click(object sender, RoutedEventArgs e)
        {
            DlgSpotkanie dlg = new DlgSpotkanie();
            Spotkanie spotkanie = fazaPoczatkowa.KolejneSpotkanie();
            dlg.lbl_druzyna1.Content = spotkanie.Druzyna1.Nazwa;
            dlg.lbl_druzyna2.Content = spotkanie.Druzyna2.Nazwa;
            List<Osoba> sedziowieSpotkania = spotkanie.GetSedziowie();
            dlg.lbl_sedzia1.Content = sedziowieSpotkania[0].Imie + " " + sedziowieSpotkania[0].Nazwisko;
            if (sedziowieSpotkania.Count == 3)
            {
                dlg.lbl_sedzia2.Content = sedziowieSpotkania[1].Imie + " " + sedziowieSpotkania[1].Nazwisko;
                dlg.lbl_sedzia3.Content = sedziowieSpotkania[2].Imie + " " + sedziowieSpotkania[2].Nazwisko;
            }
            dlg.cb_wygranaDruzyna.ItemsSource = spotkanie.GetDruzyny();
            if (true == dlg.ShowDialog())
            {
                spotkanie.Zakoncz((Druzyna)dlg.cb_wygranaDruzyna.SelectedItem);
                fazaPoczatkowa.DodajPunkt((Druzyna)dlg.cb_wygranaDruzyna.SelectedItem);
                listBox_spotkania.Items.Refresh();
            }
        }
    }
}
