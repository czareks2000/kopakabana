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
using System.Windows.Shapes;

namespace Kopakabana
{
    /// <summary>
    /// Interaction logic for DlgSpotkanie.xaml
    /// </summary>
    public partial class DlgSpotkanie : Window
    {
        public DlgSpotkanie()
        {
            InitializeComponent();
        }

        public DlgSpotkanie(Spotkanie spotkanie)
        {
            InitializeComponent();
            lbl_Sedziowie2.Visibility = Visibility.Hidden;

            lbl_druzyna1.Content = spotkanie.Druzyna1.Nazwa;
            lbl_druzyna2.Content = spotkanie.Druzyna2.Nazwa;

            lbl_sedzia1.Content = spotkanie.Sedziowie[0].Imie + " " + spotkanie.Sedziowie[0].Nazwisko;
            if (spotkanie.Sedziowie.Count == 3)
            {
                lbl_Sedziowie2.Visibility = Visibility.Visible;
                lbl_sedzia2.Content = spotkanie.Sedziowie[1].Imie + " " + spotkanie.Sedziowie[1].Nazwisko;
                lbl_sedzia3.Content = spotkanie.Sedziowie[2].Imie + " " + spotkanie.Sedziowie[2].Nazwisko;
            }

            cb_wygranaDruzyna.ItemsSource = spotkanie.GetDruzyny();
        }

        public DlgSpotkanie(Spotkanie spotkanie, bool czyPodglad)
            : this(spotkanie)
        {
            if (czyPodglad)
            {
                Title = "Podgląd";
                if (spotkanie.CzyZakonczone)
                    cb_wygranaDruzyna.SelectedItem = spotkanie.WygranaDruzyna;
                else
                {
                    lbl_wygranaDruzyna.Visibility = Visibility.Hidden;
                    cb_wygranaDruzyna.Visibility = Visibility.Hidden;
                }

                cb_wygranaDruzyna.IsEnabled = false;
                btn_zakoncz.Content = "OK";
            }
        }

        private void btn_zakoncz_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
