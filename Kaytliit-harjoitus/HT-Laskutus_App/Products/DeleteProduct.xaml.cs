using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for DeleteProduct.xaml
    /// </summary>
    public partial class DeleteProduct : Window
    {
        public DeleteProduct()
        {
            InitializeComponent();
            this.DataContext = new Product();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

            int productID;
            var repo = new InvoiceAppRepo();
            var deleteProduct = (Product)this.DataContext;
            bool canBeDeleted = repo.GetProductsInvoice(deleteProduct);


            // Syötteen tarkistus
            if (int.TryParse(productIDBox.Text, out productID))
            {

                // Onko tuote laskuriveillä
                if (canBeDeleted == false)
                {
                    string errorMessage = "Tuotetta ei voida poistaa, koska tuote on lisättynä laskuille";
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }

                //var repo = new InvoiceAppRepo();
                repo.DeleteProduct(deleteProduct);
                this.DialogResult = true;

            }
            else
            {
                string errorMessage = "Anna kelvollinen tuotteen tunniste";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
            }
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
