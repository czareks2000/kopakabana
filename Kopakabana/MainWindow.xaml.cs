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
        private List<Typ> typyGry = new List<Typ>();

        public MainWindow()
        {
            InitializeComponent();

            typyGry.Add(new Typ("Siatkowka Plażowa", 3));
            typyGry.Add(new Typ("2 Ognie", 1));
            typyGry.Add(new Typ("Przeciąganie liny", 1));
            
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
    }
}
