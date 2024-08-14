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
    /// Interaction logic for DeleteCustomer.xaml
    /// </summary>
    public partial class DeleteCustomer : Window
    {

        public DeleteCustomer()
        {
            InitializeComponent();
            this.DataContext = new Customer();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteCustomer = (Customer)this.DataContext;
            var repo = new InvoiceAppRepo();
            bool canBeDeleted = repo.GetCustomerInvoice(deleteCustomer);

            int customerID;

            // Tarkistetaan syöte
            if (int.TryParse(myCustomerIdBox.Text, out customerID))
            {
                // Onko asiakkaalla laskuja
                if (canBeDeleted == false)
                {
                    string errorMessage = "Asiakasta ei voida poistaa, koska asiakkaalla on avoimia laskuja";
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }
                repo.DeleteCustomer(deleteCustomer);

                this.DialogResult = true;

            }
            else
            {
                string errorMessage = "Anna kelvollinen asiakasnumero";
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
