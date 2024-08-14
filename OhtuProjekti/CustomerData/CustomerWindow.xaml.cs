using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using vt_systems.CustomerData;
using vt_systems.DeleteData;
using vt_systems.InvoiceData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private CustomerRepo customerRepo = new CustomerRepo();
        private InvoiceRepo invoiceRepo = new InvoiceRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();

        public CustomerWindow()
        {
            InitializeComponent();

            firstNameBox.IsEnabled = false;
            lastNameBox.IsEnabled = false;
            CompanyBox.IsEnabled = false;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;
            cmbStatus.IsEnabled = false;


            // Luodaan uusi asiakas
            this.DataContext = new Customer();

            // Haetaan asiakkaat DataGridiin Lista asiakkaista -välilehdellä
            dgCustomers.ItemsSource = customerRepo.GetCustomers();
        }

        /// <summary>
        /// Haetaan asiakkaan tiedot Asiakasnumeron perusteella Muokkaa asiakastietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    CompanyBox.IsEnabled = true;
                    addressBox.IsEnabled = true;
                    postalBox.IsEnabled = true;
                    cityBox.IsEnabled = true;
                    phoneBox.IsEnabled = true;
                    emailBox.IsEnabled = true;
                    cmbStatus.IsEnabled = true;

                    firstNameBox.Text = customer.FirstName;
                    lastNameBox.Text = customer.LastName;
                    CompanyBox.Text = customer.CompanyName;
                    addressBox.Text = customer.StreetAddress;
                    postalBox.Text = customer.PostalCode;
                    cityBox.Text = customer.City;
                    phoneBox.Text = customer.Phone;
                    emailBox.Text = customer.Email;

                    if (customer.IsInActive == false)
                    {
                        cmbStatus.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbStatus.SelectedIndex = 1;
                    }
                }
                else
                {
                    myCustomerIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen asiakasnumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Tallennetaan haetun asiakkaan tiedot Muokkaa asiakastietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            var updateCustomer = this.DataContext as Customer;
            updateCustomer.FirstName = firstNameBox.Text;
            updateCustomer.LastName = lastNameBox.Text;
            updateCustomer.CompanyName = CompanyBox.Text;
            updateCustomer.StreetAddress = addressBox.Text;
            updateCustomer.PostalCode = postalBox.Text;
            updateCustomer.City = cityBox.Text;
            updateCustomer.Phone = phoneBox.Text;
            updateCustomer.Email = emailBox.Text;

            if (cmbStatus.SelectedIndex == 0)
            {
                updateCustomer.IsInActive = false;
            }

            else
            {
                updateCustomer.IsInActive = true;
            }

            //Tarkistetaan että tietokentät eivät ole tyhjät
            if (updateCustomer.FirstName == string.Empty)
            {
                MessageBox.Show("Syötä etunimi!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.LastName == string.Empty)
            {
                MessageBox.Show("Syötä sukunimi!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.City == string.Empty)
            {
                MessageBox.Show("Syötä postitoimipaikka!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateCustomer.Email == string.Empty)
            {
                MessageBox.Show("Syötä sähköpostiosoite!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!updateCustomer.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            customerRepo.UpdateCustomer(updateCustomer);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgCustomers.ItemsSource = customerRepo.GetCustomers();
            ResetAll();
        }


        /// <summary>
        /// Tallennetaan uusi asiakas Lisää asiakas -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new Customer
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                CompanyName = txtCompanyName.Text,
                StreetAddress = txtAddress.Text,
                City = txtCity.Text,
                PostalCode = txtPostal.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
            };

            //Tarkistetaan että tietokentät eivät ole tyhjät
            if (newCustomer.FirstName == string.Empty)
            {
                MessageBox.Show("Syötä etunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.LastName == string.Empty)
            {
                MessageBox.Show("Syötä sukunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä lähiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.City == string.Empty)
            {
                MessageBox.Show("Syötä kaupunki", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.FirstName == string.Empty)
            {
                MessageBox.Show("Syötä etunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.LastName == string.Empty)
            {
                MessageBox.Show("Syötä sukunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.City == string.Empty)
            {
                MessageBox.Show("Syötä postitoimipaikka", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newCustomer.Email == string.Empty)
            {
                MessageBox.Show("Syötä sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!newCustomer.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            customerRepo.AddNewCustomer(newCustomer);
            MessageBox.Show("Asiakkaan lisääminen onnistui!");
            dgCustomers.ItemsSource = customerRepo.GetCustomers();
            ResetAll();
        }


        /// <summary>
        /// Poistetaan asiakas Poista asiakas -välilendellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {

            var customer = (Customer)dgCustomers.SelectedItem;

            ObservableCollection<Invoice> customersInvoices = new ObservableCollection<Invoice>();
            ObservableCollection<Reservation> customerReservations = new ObservableCollection<Reservation>();

            // Jos asiakasta ei ole valittu datagridissä, annetaan virheilmoitus
            if (dgCustomers.SelectedIndex == -1)
            {
                MessageBox.Show("Valitse poistettava asiakas...", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // haetaan asiakkaan laskut ja varaukset
            customersInvoices = invoiceRepo.GetInvoiceLines(customer);
            customerReservations = reservationRepo.GetReservationLines(customer);

            // jos laskuja tai varauksia löytyy, annetaan tästä ilmoitus
            if (customersInvoices.Count != 0 || customerReservations.Count != 0)
            {
                MessageBox.Show("Ei voida poistaa, asiakkaalla on järjestelmässä aktiivisia varauksia ja laskuja", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // jos asiakkaalla ei ole aktiivisia laskuja ja varauksia, poisteaan asiakastieto tietokannasta
            if (customersInvoices.Count == 0 && customerReservations.Count == 0)
            {
                var result = MessageBox.Show("Haluatko varmasti poistaa valitun asiakkaan?", "Asiakastietojen poisto.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    customerRepo.DeleteCustomer(customer);
                    dgCustomers.ItemsSource = customerRepo.GetCustomers();
                }
            }
        }


        /// <summary>
        /// Estää muiden kuin numeroiden syöttämisen tekstikenttään, johon on määritelty tämä toiminto (PreviewTextInput).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        /// Valitun välilehden otsikko on oranssilla, muut mustalla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderFontChange_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tAdd.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Orange;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Black;
            }

            if (tSearch.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Orange;
                tbList.Foreground = Brushes.Black;
            }

            if (tList.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Orange;
            }
        }

        /// <summary>
        /// Tyhjentää käyttöliittymän tekstikentät
        /// </summary>
        private void ResetAll()
        {
            myCustomerIdBox.Text = string.Empty;
            myCustomerIdBox.IsEnabled = true;

            firstNameBox.Text = string.Empty;
            lastNameBox.Text = string.Empty;
            CompanyBox.Text = string.Empty;
            addressBox.Text = string.Empty;
            postalBox.Text = string.Empty;
            cityBox.Text = string.Empty;
            phoneBox.Text = string.Empty;
            emailBox.Text = string.Empty;

            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPostal.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;

            firstNameBox.IsEnabled = false;
            lastNameBox.IsEnabled = false;
            CompanyBox.IsEnabled = false;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;
            cmbStatus.IsEnabled = false;
        }


        /// <summary>
        /// Sulkee ohjelman.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Toimipisteet..., avautuu OfficeWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeWindow_Click(object sender, RoutedEventArgs e)
        {
            OfficeWindow officeWindow = new OfficeWindow();
            officeWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Laitteet..., avautuu DeviceWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWindow_Click(object sender, RoutedEventArgs e)
        {
            DeviceWindow deviceWindow = new DeviceWindow();
            deviceWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Toimitilat..., avautuu WorkspaceWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceWindow_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceWindow workspaceWindow = new WorkspaceWindow();
            workspaceWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Laskut..., avautuu NewInvoiceWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Invoice_Click(object sender, RoutedEventArgs e)
        {
            NewInvoiceWindow invoiceWindows = new NewInvoiceWindow();
            invoiceWindows.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa ExistingReservationWindow:sta Uusi varaus Buttonia, avautuu ReservationWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationWindow_Click(object sender, RoutedEventArgs e)
        {
            ReservationWindow reservationWindow = new ReservationWindow();
            reservationWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Varaukset..., avautuu ExistingReservationWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingReservationWindow_Click(object sender, RoutedEventArgs e)
        {
            ExistingReservationWindow existingReservationWindow = new ExistingReservationWindow();
            existingReservationWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Palvelut..., avautuu ServiceWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta Raportointi..., avautuu RaportWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta Admin..., avautuu DeleteDataWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta Ohje, avautuu HelpWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpWindow_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }


        /// <summary>
        /// CustomerWindow Shortcuts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Laitteet -ikkuna
            if (e.Key == Key.F2)
            {
                DeviceWindow_Click(sender, e);
            }

            // Palvelut -ikkuna
            else if (e.Key == Key.F3)
            {
                ServiceWindow_Click(sender, e);
            }

            // Varaukset -ikkuna
            else if (e.Key == Key.F4)
            {
                ExistingReservationWindow_Click(sender, e);
            }

            // Toimitilat -ikkuna
            else if (e.Key == Key.F5)
            {
                WorkspaceWindow_Click(sender, e);
            }

            // Laskut -ikkuna
            else if (e.Key == Key.F6)
            {
                Invoice_Click(sender, e);
            }

            // Toimipisteet -ikkuna
            else if (e.Key == Key.F7)
            {
                OfficeWindow_Click(sender, e);
            }

            // Raportointi -ikkuna
            else if (e.Key == Key.F8)
            {
                ReportWindow_Click(sender, e);
            }

            // Admin -ikkuna
            else if (e.Key == Key.F9)
            {
                Admin_Click(sender, e);
            }
        }


        /// <summary>
        /// Mahdollistaa ikkunan liikuttelun.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi.Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myCustomerIdBox_MouseEnter(object sender, MouseEventArgs e)
        {
            myCustomerIdBox.Focusable = true;
        }


        //private void test(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    btnGetCustomer.Foreground = Brushes.White;
        //    btnGetCustomer.Background = Brushes.Orange;
        //}
    }
}
