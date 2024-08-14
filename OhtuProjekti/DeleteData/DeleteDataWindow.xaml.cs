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
using vt_systems.Database;
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.DeleteData
{
    /// <summary>
    /// Interaction logic for DeleteDataWindow.xaml
    /// </summary>
    public partial class DeleteDataWindow : Window
    {
        // Käyttäjätunnus ja salasana
        private string username = "Admin";
        private string password = "salasana:D";

        private OfficeRepo officeRepo = new OfficeRepo();
        private WorkSpaceRepo workspaceRepo = new WorkSpaceRepo();
        private ServiceRepo serviceRepo = new ServiceRepo();
        private DeviceRepo deviceRepo = new DeviceRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private CustomerRepo customerRepo = new CustomerRepo();
        private InvoiceRepo invoiceRepo = new InvoiceRepo();
        private DatabaseScripts databaseScripts = new DatabaseScripts();

        public DeleteDataWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Kirjauduttaessa tarkistetaan käyttäjätunnus ja salasana, jos väärin, ohjelma ilmoittaa siitä käyttäjälle. Jos kirjautuminen onnistuu saa käyttäjä varoituksen liittyen tietojen poisto peruttamattomuudesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text != username || passwordBox.Password.ToString() != password)
            {
                MessageBox.Show("Väärä käyttäjätunnus tai salasana", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (usernameBox.Text == username && passwordBox.Password.ToString() == password)
            {
                MessageBox.Show("Tämän valikon kautta tehdyt poistot \r\npoistavat KAIKKI poistettavaan asiaan liittyvät tiedot!\r\nPoisto on peruuttamaton. ", "Varoitus!", MessageBoxButton.OK, MessageBoxImage.Warning);
                mainTabs.SelectedIndex = 1;
                dgOffices.ItemsSource = officeRepo.GetInactiveOffices();
                btnOffice.Visibility = Visibility.Visible;
                btnWorkspace.Visibility = Visibility.Visible;
                btnService.Visibility = Visibility.Visible;
                btnDevice.Visibility = Visibility.Visible;
                btnReservation.Visibility = Visibility.Visible;
                btnInvoice.Visibility = Visibility.Visible;
                btnCustomer.Visibility = Visibility.Visible;
                btnCreateDb.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// Mahdollistaa tabeilla liikkumisen. Klikattuina kyseinen nappi oranssi, muut harmaina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Navigation_Click(object sender, RoutedEventArgs e)
        {
            // Toimipisteet
            if (sender == btnOffice)
            {
                mainTabs.SelectedIndex = 1;
                dgOffices.ItemsSource = officeRepo.GetInactiveOffices();

                // Valittu nappi oranssi, muut harmaina
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }

            // Toimitilat
            if (sender == btnWorkspace)
            {
                mainTabs.SelectedIndex = 2;
                dgWorkspaces.ItemsSource = workspaceRepo.GetInactiveWorkspaces();

                // Valittu nappi oranssi, muut harmaina
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }

            // Palvelut
            if (sender == btnService)
            {
                mainTabs.SelectedIndex = 3;
                dgService.ItemsSource = serviceRepo.GetInactiveServices();

                // Valittu nappi oranssi, muut harmaina
                btnService.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }
            // Laitteet
            if (sender == btnDevice)
            {
                mainTabs.SelectedIndex = 4;
                dgDevices.ItemsSource = deviceRepo.GetInactiveDevices();

                // Valittu nappi oranssi, muut harmaina
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }
            // Laskut
            if (sender == btnInvoice)
            {
                mainTabs.SelectedIndex = 5;
                dgInvoice.ItemsSource = invoiceRepo.GetAllInvoices();

                // Valittu nappi oranssi, muut harmaina
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }

            // Varaukset
            if (sender == btnReservation)
            {
                mainTabs.SelectedIndex = 6;
                dgReservations.ItemsSource = reservationRepo.GetReservations();

                // Valittu nappi oranssi, muut harmaina
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }

            // Asiakkaat
            if (sender == btnCustomer)
            {
                mainTabs.SelectedIndex = 7;
                dgCustomers.ItemsSource = customerRepo.GetInactiveCustomers();

                // Valittu nappi oranssi, muut harmaina
                btnCustomer.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));
                btnWorkspace.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnService.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnDevice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnReservation.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnInvoice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
                btnOffice.Background = new SolidColorBrush(Color.FromRgb(238, 238, 238));
            }
        }


        /// <summary>
        /// DataGrideissä tiedon poistaminen Poista Buttoneista, ohjelma varmista ahaluatko varmasti poistaa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Rectangle;
            // Office
            if (button.Name == "btnOfficeDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun toimipisteen? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var office = button?.DataContext as Office;
                    officeRepo.DeleteOffice(office);
                    dgOffices.ItemsSource = officeRepo.GetInactiveOffices();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Workspace
            if (button.Name == "btnWorkspaceDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun toimitilan? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var workspace = button?.DataContext as Workspace;
                    workspaceRepo.DeleteWorkspace(workspace);
                    dgWorkspaces.ItemsSource = workspaceRepo.GetInactiveWorkspaces();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Service
            if (button.Name == "btnServiceDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun palvelun? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var service = button?.DataContext as Service;
                    serviceRepo.DeleteService(service);
                    dgService.ItemsSource = serviceRepo.GetInactiveServices();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Device
            if (button.Name == "btnDeviceDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun laitteen? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var device = button?.DataContext as Device;
                    deviceRepo.DeleteDevice(device);
                    dgDevices.ItemsSource = deviceRepo.GetInactiveDevices();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Laskut
            if (button.Name == "btnInvoiceDelete")
            {            
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun laskun? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var invoice = button?.DataContext as Invoice;
                    invoiceRepo.RemoveInvoice(invoice);
                    dgInvoice.ItemsSource = invoiceRepo.GetAllInvoices();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Varaukset
            if (button.Name == "btnReservationDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun varauksen? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var reservation = button?.DataContext as Reservation;
                    reservationRepo.DeleteReservation(reservation);
                    dgReservations.ItemsSource = reservationRepo.GetReservations();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            // Asiakkaat
            if (button.Name == "btnCustomerDelete")
            {
                MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa valitun asiakkaan? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var customer = button?.DataContext as Customer;
                    customerRepo.DeleteCustomer(customer);
                    dgCustomers.ItemsSource = customerRepo.GetInactiveCustomers();
                }
                else if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
        }


        /// <summary>
        /// Kun hiiri menee Poista Buttonin päälle, vaihtuu väri vaaleammaksi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = Brushes.LightPink;
            }
        }


        /// <summary>
        /// Kun hiiri poistuu Poista Buttonin päältä, vaihtuu väri takaisin tummemmaksi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = Brushes.DarkRed;
            }
        }


        /// <summary>
        /// Mahdollistaa ikkunan liikuttelun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }


        /// <summary>
        /// Sulkee ohjelman
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Yläpalkissa oleva Alusta Button poistaa kaikki tiedot ja populoi tietokannan testidatalla, ohjelma varmistaa haluatko varmasti tehdä näin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateDb_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistaa kaikki tiedot ja populoida tietokannan testidatalla? Toimintaa ei voida peruuttaa.", "Poista", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                databaseScripts.IniateDatabaseAdmin();
                
            }
            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
