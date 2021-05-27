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
    /// Interaction logic for DlgTypGry.xaml
    /// </summary>
    public partial class DlgTypGry : Window
    {
        public DlgTypGry()
        {
            InitializeComponent();
            List<TypGry> typyGry = new List<TypGry>();

            typyGry.Add(new TypGry("Siatkowka Plażowa", 3));
            typyGry.Add(new TypGry("2 Ognie", 1));
            typyGry.Add(new TypGry("Przeciąganie liny", 1));

            cbx_TypGry.ItemsSource = typyGry;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

    }
}
