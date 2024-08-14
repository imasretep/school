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

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for UpdateProduct.xaml
    /// </summary>
    public partial class UpdateProduct : Window
    {
        private Product product;
        private InvoiceAppRepo repo = new InvoiceAppRepo();
        public UpdateProduct()
        {
            InitializeComponent();
            this.DataContext = new Product();
            productNameBox.IsEnabled = false;
            productPriceBox.IsEnabled = false;
            amountBox.IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GetProduct_Click(object sender, RoutedEventArgs e)
        {
            int productNumber;

            // Syötteen tarkistus
            if (int.TryParse(myProductIdBox.Text, out productNumber))
            {
                product = (Product)this.DataContext;
                if (repo.GetProduct(product) != null) // Haetaan tuote
                {
                    myProductIdBox.IsEnabled = false;
                    productPriceBox.IsEnabled = true;
                    amountBox.IsEnabled = true;

                    productNameBox.Text = product.Name;
                    productPriceBox.Text = product.Price.ToString();
                    amountBox.Text = product.Amount.ToString();
                }
                else 
                {
                    myProductIdBox.IsEnabled = true;
                }


            }
            else
            {
                string errorMessage = "Anna kelvollinen tuotteen tunniste";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                this.DialogResult = true;
            }
            
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

            var updateProduct = (Product)this.DataContext;

            // Syötteiden tarkistus
            if (updateProduct.Price <= 0 || updateProduct.Amount <= -1)
            {
                string errorMessage = "Täytä tarvittavat tiedot";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }
            repo.UpdateProduct(updateProduct);
            string successMessage = "Tuotteen päivitys onnistui";
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
