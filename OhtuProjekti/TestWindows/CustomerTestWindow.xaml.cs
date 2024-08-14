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
using vt_systems.CustomerData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for CustomerTestWindow.xaml
    /// </summary>
    /// 
    public partial class CustomerTestWindow : Window
    {
        private CustomerRepo customerRepo = new CustomerRepo();
        public CustomerTestWindow()
        {
            InitializeComponent();

            this.DataContext = new Customer();
            dgCustomers.ItemsSource = customerRepo.GetCustomers();


            myCustomerIdBox.IsEnabled = true;
            firstNameBox.IsEnabled = false;
            lastNameBox.IsEnabled = false;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;
        }

        private void GetCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer();
            int customerID;

            if (int.TryParse(myCustomerIdBox.Text, out customerID))
            {
                customer = (Customer)this.DataContext;
                if (customerRepo.GetCustomer(customer) != null)
                {
                    myCustomerIdBox.IsEnabled = false;

                    firstNameBox.IsEnabled = true;
                    lastNameBox.IsEnabled = true;
                    addressBox.IsEnabled = true;
                    postalBox.IsEnabled = true;
                    cityBox.IsEnabled = true;
                    phoneBox.IsEnabled = true;
                    emailBox.IsEnabled = true;

                    firstNameBox.Text = customer.FirstName;
                    lastNameBox.Text = customer.LastName;
                    addressBox.Text = customer.StreetAddress;
                    postalBox.Text = customer.PostalCode;
                    cityBox.Text = customer.City;
                    phoneBox.Text = customer.Phone;
                    emailBox.Text = customer.Email;

                }
                else
                {
                    myCustomerIdBox.IsEnabled = true;
                }

            }
            else
            {
                MessageBox.Show("Anna kelvollinen asiakasnumero");
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var updateCustomer = this.DataContext as Customer;
            updateCustomer.FirstName = firstNameBox.Text;
            updateCustomer.LastName = lastNameBox.Text;
            updateCustomer.StreetAddress = addressBox.Text;
            updateCustomer.PostalCode = postalBox.Text;
            updateCustomer.City = cityBox.Text;
            updateCustomer.Phone = phoneBox.Text;
            updateCustomer.Email = emailBox.Text;

            if (updateCustomer.FirstName == string.Empty || updateCustomer.LastName == string.Empty || updateCustomer.StreetAddress == string.Empty || updateCustomer.PostalCode == string.Empty || updateCustomer.City == string.Empty || updateCustomer.Phone == string.Empty || updateCustomer.Email == string.Empty)
            {
                MessageBox.Show("ÄÄÄÄÄÄÄÄH!");
                return;
            }
            customerRepo.UpdateCustomer(updateCustomer);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgCustomers.ItemsSource = customerRepo.GetCustomers();
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new Customer
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                StreetAddress = txtAddress.Text,
                City = txtCity.Text,
                PostalCode = txtPostal.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,

            };

            if (newCustomer.FirstName == string.Empty || newCustomer.LastName == string.Empty || newCustomer.StreetAddress == string.Empty || newCustomer.PostalCode == string.Empty || newCustomer.City == string.Empty || newCustomer.Phone == string.Empty || newCustomer.Email == string.Empty)
            {
                MessageBox.Show("ÄÄÄÄÄÄÄÄH!");
                return;
            }
            customerRepo.AddNewCustomer(newCustomer);
            MessageBox.Show("Asiakkaan lisääminen onnistui!");
            dgCustomers.ItemsSource = customerRepo.GetCustomers();
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var deleteCustomer = this.DataContext as Customer;
            int customerID;

            // Tarkistetaan syöte
            if (int.TryParse(deleteCustomerIdBox.Text, out customerID))
            {
                deleteCustomer.Id = customerID;
                customerRepo.DeleteCustomer(deleteCustomer);
                dgCustomers.ItemsSource = customerRepo.GetCustomers();


            }
            else
            {
                MessageBox.Show("ÄÄÄÄÄ!");
            }

        }



    }
}
