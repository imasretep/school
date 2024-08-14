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
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        private Customer customer;
        private InvoiceAppRepo repo = new InvoiceAppRepo();
        public UpdateCustomer()
        {
            InitializeComponent();
            this.DataContext = new Customer();
            firstNameBox.IsEnabled = false;
            lastNameBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            addressBox.IsEnabled = false;
            txtPostal.IsEnabled = false;
            txtCity.IsEnabled = false;

        }

        private void GetCustomer_Click(object sender, RoutedEventArgs e)
        {
            int customerID;

            if (int.TryParse(myCustomerIdBox.Text, out customerID))
            {
                customer = (Customer)this.DataContext;
                if (repo.GetCustomer(customer) != null)
                {
                    myCustomerIdBox.IsEnabled = false;

                    firstNameBox.IsEnabled = true;
                    lastNameBox.IsEnabled = true;
                    phoneBox.IsEnabled = true;
                    addressBox.IsEnabled = true;
                    txtPostal.IsEnabled = true;
                    txtCity.IsEnabled = true;

                    firstNameBox.Text = customer.FirstName;
                    lastNameBox.Text = customer.LastName;
                    phoneBox.Text = customer.Phone;
                    addressBox.Text = customer.Address;
                    txtPostal.Text = customer.Postal;
                    txtCity.Text = customer.City;
                }
                else
                {
                    myCustomerIdBox.IsEnabled = true;
                }

            }
            else
            {
                string errorMessage = "Anna kelvollinen asiakasnumero";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var updateCustomer = (Customer)this.DataContext;

            updateCustomer.FirstName = firstNameBox.Text;
            updateCustomer.LastName = lastNameBox.Text;
            updateCustomer.LastName = lastNameBox.Text;
            updateCustomer.Phone = phoneBox.Text;
            updateCustomer.Address = addressBox.Text;
            updateCustomer.Postal = txtPostal.Text;
            updateCustomer.City = txtCity.Text;

            if (updateCustomer.FirstName == string.Empty || updateCustomer.LastName == string.Empty || updateCustomer.Phone == string.Empty || updateCustomer.Address == string.Empty || updateCustomer.Postal == string.Empty || updateCustomer.City == string.Empty)
            {
                string errorMessage = "Täytä tarvittavat tiedot";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }

            repo.UpdateCustomer(updateCustomer);
            string successMessage = "Asiakkaan tietojen päivitys onnistui";
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
