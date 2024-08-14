using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vt_systems.OfficeData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for OfficeTestWindow.xaml
    /// </summary>
    public partial class OfficeTestWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        public OfficeTestWindow()
        {
            InitializeComponent();
            myOfficeIdBox.IsEnabled = true;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;


            this.DataContext = new Office();
            dgOffices.ItemsSource = officeRepo.GetOffices();


        }

        private void GetOffice_Click(object sender, RoutedEventArgs e)
        {
            var office = new Office();
            int officeID;

            if (int.TryParse(myOfficeIdBox.Text, out officeID))
            {
                office = this.DataContext as Office;
                if (officeRepo.GetOffice(office) != null)
                {
                    myOfficeIdBox.IsEnabled = false;
                    addressBox.IsEnabled = true;
                    postalBox.IsEnabled = true;
                    cityBox.IsEnabled = true;
                    phoneBox.IsEnabled = true;
                    emailBox.IsEnabled = true;

                    addressBox.Text = office.StreetAddress;
                    postalBox.Text = office.PostalCode;
                    cityBox.Text = office.City;
                    phoneBox.Text = office.Phone;
                    emailBox.Text = office.Email;

                }
                else
                {
                    myOfficeIdBox.IsEnabled = true;
                }

            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimipiste id");
            }
        }

        private void UpdateOffice_Click(object sender, RoutedEventArgs e)
        {

            var updateOffice = this.DataContext as Office;
            updateOffice.StreetAddress = addressBox.Text;
            updateOffice.PostalCode = postalBox.Text;
            updateOffice.City = cityBox.Text;
            updateOffice.Phone = phoneBox.Text;
            updateOffice.Email = emailBox.Text;

            if (updateOffice.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite!");
                return;
            }

            if (updateOffice.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero");
                return;
            }

            if (updateOffice.City == string.Empty)
            {
                MessageBox.Show("Syötä postitoimipaikka");
                return;
            }

            if (updateOffice.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero");
                return;
            }

            if (updateOffice.Email == string.Empty)
            {
                MessageBox.Show("Syötä sähköpostiosoite");
                return;
            }
            if (!updateOffice.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite");
                return;
            }
            officeRepo.UpdateOffice(updateOffice);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgOffices.ItemsSource = officeRepo.GetOffices();

        }

        private void AddNewOffice_Click(object sender, RoutedEventArgs e)
        {
            var newOffice = new Office
            {

                StreetAddress = txtAddress.Text,
                City = txtCity.Text,
                PostalCode = txtPostal.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,

            };

            if (newOffice.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite");
                return;
            }

            if (newOffice.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero");
                return;
            }

            if (newOffice.City == string.Empty)
            {
                MessageBox.Show("Syötä postitoimipaikka");
                return;
            }

            if (newOffice.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero");
                return;
            }

            if (newOffice.Email == string.Empty)
            {
                MessageBox.Show("Syötä sähköpostiosoite");
                return;
            }

            if (!newOffice.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite");
                return;
            }
            officeRepo.AddNewOffice(newOffice);
            MessageBox.Show("Toimitilan lisääminen onnistui!");
            dgOffices.ItemsSource = officeRepo.GetOffices();
        }

        private void DeleteOffice_Click(object sender, RoutedEventArgs e)
        {
            var deleteOffice = this.DataContext as Office;
            int officeID;
            if (int.TryParse(deleteOfficeIdBox.Text, out officeID))
            {
                deleteOffice.OfficeID = officeID;

                bool zeroDevices = officeRepo.GetOfficeDevicesAmount(deleteOffice);
                bool zeroServices = officeRepo.GetOfficeServicesAmount(deleteOffice);
                bool zeroReservations = officeRepo.GetOfficeReservationsAmount(deleteOffice);
                bool zeroWorkspace = officeRepo.GetOfficeWorkspaceAmount(deleteOffice);

                if (zeroDevices == true && zeroServices == true && zeroReservations == true && zeroWorkspace == true)
                {
                    officeRepo.DeleteOffice(deleteOffice);
                    dgOffices.ItemsSource = officeRepo.GetOffices();
                    return;
                }

                MessageBox.Show("´Toimitilaa ei voida poistaa. Poista ensin kaikki toimitilat, varaukset, laitteet ja palvelut.");


            }
            else
            {
                MessageBox.Show("ÄÄÄÄÄ!");
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
