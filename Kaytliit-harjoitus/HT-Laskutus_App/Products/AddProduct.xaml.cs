using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
            this.DataContext = new Product();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {

            var newProduct = (Product)this.DataContext;
            var repo = new InvoiceAppRepo();

            // Tarkistetaan onko radiobuttoni painettuna
            if (rb1.IsChecked == true)
            {
                newProduct.Unit = "Kpl";
            }
            else
            {
                newProduct.Unit = "h";
            }

            // Muitten syötteiden tarkistus
            if (newProduct.Name == string.Empty || newProduct.Price == 0 || newProduct.Amount == 0 || (rb1.IsChecked == false && rb2.IsChecked == false))
            {
                string errorMessage = "Täytä tarvittavat tiedot";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }

            repo.AddNewProduct(newProduct);
            string successMessage = "Tuotteen lisääminen onnistui";
            Done successWindow = new Done(successMessage);
            successWindow.ShowDialog();
            this.DialogResult = true;

        }

        // Ikkunan asetukset
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void rctClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void rctClose_MouseEnter(object sender, MouseEventArgs e)
        {
            rctClose.Fill = Brushes.Red;
        }

        private void rctClose_MouseLeave(object sender, MouseEventArgs e)
        {
            rctClose.Fill = Brushes.DarkRed;
        }

        private void rctMinimize_MouseEnter(object sender, MouseEventArgs e)
        {

            rctMinimize.Fill = Brushes.Gold;
        }

        private void rctMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            rctMinimize.Fill = Brushes.DarkGoldenrod;
        }


        private void rctMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
