using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using vt_systems.DeleteData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.TestWindows;
using vt_systems.CustomerData;
using vt_systems.OfficeData;

namespace vt_systems.InvoiceData
{
    /// <summary>
    /// Interaction logic for NewInvoiceWindow.xaml
    /// </summary>
    public partial class NewInvoiceWindow : Window
    {
        private InvoiceRepo invoiceRepo = new InvoiceRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ObservableCollection<Invoice> invoicesList = new ObservableCollection<Invoice>();

        public NewInvoiceWindow()
        {
            InitializeComponent();

            //invoices_list.ItemsSource = invoiceRepo.GetAllInvoices();
            reservations_list.ItemsSource = UnbilledReservations();
            GetInvoicesList();
        }


        /// <summary>
        /// Sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Valitun välilehden otsikko on oranssilla, muut mustalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderFontChange_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tAdd.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Orange;
                tbSearch.Foreground = Brushes.Black;
            }

            if (tSearch.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Orange;
            }
        }


        /// <summary>
        /// Laskut -välilehdellä Avaa Button avaa valitun laskun, jos laskua ei ole valittu, huomauttaa ohjelma asiasta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenInvoice(object sender, RoutedEventArgs e)
        {
            var item = invoices_list.SelectedItem as Invoice;

            if (item != null)
            {
                var invoice = new Print(item);
                invoice.ShowDialog();
            }
            else
            {
                MessageBox.Show("Valitse ensin lasku.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            GetInvoicesList();
        }


        /// <summary>
        /// Mahdollistaa ikkunan liikuttelun
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
        /// Painettaessa Menusta Toimitilat..., avautuu WorkspaceWindow
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
        /// Painettaessa Menusta Asiakkaat..., avautuu CustomerWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Toimipisteet..., avautuu OfficeWindow
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
        /// Painettaessa Menusta Laitteet..., avautuu DeviceWindow
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
        /// Painettaessa Menusta Palvelut..., avautuu ServiceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Mensuta Varaukset..., avautuu ExistingReservationWindow
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
        /// Painettaessa Menusta Raportointi..., avautuu RaportWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Admin..., avautuu DeleteDataWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Ohje, avautuu HelpWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpWindow_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
            Close();
        }


        /// <summary>
        /// NewInvoiceWindow Shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewInvoiceWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Asiakkaat -ikkuna
            if (e.Key == Key.F1)
            {
                CustomerWindow_Click(sender, e);
            }

            // Laitteet -ikkuna
            else if (e.Key == Key.F2)
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
        /// Laskut -välilehdellä painettaessa Hae Buttonia, haetaan syötetyllä nimellä tai varausnumerolla olevia laskuja.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchInvoices(object sender, RoutedEventArgs e)
        {
            GetInvoicesList();

            first_name.Text = string.Empty;
            last_name.Text = string.Empty;
            reservation_num.Text = string.Empty;
        }


        /// <summary>
        /// Haetaan laskut asiakkana nimen tai varausnumeron perusteella
        /// </summary>
        private void GetInvoicesList()
        {
            invoicesList = new ObservableCollection<Invoice>();

            string firstName = first_name.Text;
            string lastName = last_name.Text;
            string reservation = reservation_num.Text;

            if (firstName != string.Empty && lastName != string.Empty && reservation == string.Empty)
            {
                var importData = invoiceRepo.GetInvoicesViaFullName(firstName, lastName);

                foreach (var item in importData)
                {
                    if (!item.IsPaid || show_billed.IsChecked == true)
                    {
                        invoicesList.Add(item);
                    }
                }
                invoices_list.ItemsSource = invoicesList;
            }
            else if (firstName != string.Empty && reservation == string.Empty)
            {
                var importData = invoiceRepo.GetInvoicesViaFirstName(firstName);

                foreach (var item in importData)
                {
                    if (!item.IsPaid || show_billed.IsChecked == true)
                    {
                        invoicesList.Add(item);
                    }
                }
                invoices_list.ItemsSource = invoicesList;
            }
            else if (lastName != string.Empty && reservation == string.Empty)
            {
                var importData = invoiceRepo.GetInvoicesViaLastName(lastName);

                foreach (var item in importData)
                {
                    if (!item.IsPaid || show_billed.IsChecked == true)
                    {
                        invoicesList.Add(item);
                    }
                }
                invoices_list.ItemsSource = invoicesList;
            }
            else if (reservation != string.Empty)
            {
                if (int.TryParse(reservation, out var id))
                {
                    var importData = invoiceRepo.GetInvoicesByReservation(id);

                    foreach (var item in importData)
                    {
                        if (!item.IsPaid || show_billed.IsChecked == true)
                        {
                            invoicesList.Add(item);
                        }
                    }
                    invoices_list.ItemsSource = invoicesList;
                }
                else
                {
                    MessageBox.Show("Virheellinen varaustunnus. Yritä uudelleen.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                var importData = invoiceRepo.GetAllInvoices();

                foreach (var item in importData)
                {
                    if (!item.IsPaid || show_billed.IsChecked == true)
                    {
                        invoicesList.Add(item);
                    }
                }
                invoices_list.ItemsSource = invoicesList;
            }
            PaymentChecker();
        }


        /// <summary>
        /// Haetaan maksamattomat varaukset
        /// </summary>
        /// <returns> unbilled </returns>
        private ObservableCollection<Reservation> UnbilledReservations()
        {
            ObservableCollection<Reservation> unbilled = new ObservableCollection<Reservation>();
            var collection = reservationRepo.GetReservations();

            foreach (var item in collection)
            {
                if (item.IsBilled == false)
                {
                    unbilled.Add(item);
                }
            }
            return unbilled;
        }


        /// <summary>
        /// Laskuttamattomat varaukset -välilehdellä painettaessa Luo lasku Buttonia avautuu lasku. Jos varausta ei ole valittu, ilmoittaa ohjelma siitä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInvoiceFromReservation(object sender, RoutedEventArgs e)
        {
            var item = reservations_list.SelectedItem as Reservation;
            if (item != null)
            {
                var invoice = new Invoice(item);
                var invoicewin = new Print(invoice);

                invoicewin.ShowDialog();

                item.IsBilled = true;

                if (invoicewin.DialogResult == true)
                {
                    reservationRepo.UpdateReservationStatus(item);
                }
            }
            else
            {
                MessageBox.Show("Valitse ensin varaus, jolle haluat laskun luoda.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            reservations_list.ItemsSource = UnbilledReservations();
            GetInvoicesList();
        }


        /// <summary>
        /// Tarkistaa onko maksu myöhässä
        /// </summary>
        private void PaymentChecker()
        {
            DateTime today = DateTime.Today;

            foreach (var item in invoicesList)
            {
                if (item.IsPaid == false)
                {
                    if (item.DueDate < today)
                    {
                        item.IsPaymentLate = true;
                    }
                    else
                    {
                        item.IsPaymentLate = false;
                    }
                }
            }
        }


        /// <summary>
        /// Asettaa valitun rivin maksetuksi Merkitse maksetuksi Buttonista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSetPaid(object sender, RoutedEventArgs e)
        {
            var invoice = invoices_list.SelectedItem as Invoice;

            if (invoices_list.SelectedItem != null)
            {
                if (invoice.IsPaid == false)
                {
                    invoice.IsPaid = true;

                    invoiceRepo.ChangePaymentStatus(invoice);
                }
                else
                {
                    invoice.IsPaid = false;

                    invoiceRepo.ChangePaymentStatus(invoice);
                }
                GetInvoicesList();
            }
        }


        /// <summary>
        /// Riippuen valitus rivin statuksesta, muuttuu Buttonin nimi Laskut -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoices_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (invoices_list.SelectedItem != null)
            {
                var invoice = invoices_list.SelectedItem as Invoice;

                if (invoice.IsPaid)
                {
                    set_billing_status.Content = "Poista merkintä";
                }
                else
                {
                    set_billing_status.Content = "Merkitse maksetuksi";
                }
            }
        }


        /// <summary>
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi. Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reservation_num_MouseEnter(object sender, MouseEventArgs e)
        {
            reservation_num.Focusable = true;
        }

        private void reservations_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (reservations_list.SelectedItem != null)
            {
                var reservation = reservations_list.SelectedItem as Reservation;

                info_reservation_num.Text = reservation.ReservationId.ToString();
                info_reservation_date.SelectedDate = reservation.ReleaseDate;

                var customer = new Customer();
                customer.Id = reservation.CustomerID;

                var customerRepo = new CustomerRepo();
                customer = customerRepo.GetCustomer(customer);

                info_reservation_cid.Text = customer.Id.ToString();
                info_reservation_cname.Text = customer.FirstName + " " + customer.LastName;

                var reservationRepo = new ReservationRepo();
                reservation = reservationRepo.GetReservationWithData(reservation);

                foreach (var item in reservation.ReservedObjects)
                {
                    if (item.Workspace != null)
                    {
                        if (item.Workspace.WSName != string.Empty)
                        {
                            info_reservation_workspace.Text = item.Workspace.WSName;
                        }
                    }
                }

                var officeRepo = new OfficeRepo();
                var office = new Office();

                office.OfficeID = reservation.OfficeID;
                office = officeRepo.GetOffice(office);

                info_reservation_city.Text = office.City;

            }



        }
    }
}
