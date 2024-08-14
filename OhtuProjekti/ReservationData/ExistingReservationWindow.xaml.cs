using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.TestWindows;
using vt_systems.WorkspaceData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for ExistingReservationWindow.xaml
    /// </summary>
    public partial class ExistingReservationWindow : Window
    {
        private ReservationRepo reservationRepo = new ReservationRepo();
        private CustomerRepo customerRepo = new CustomerRepo();
        private OfficeRepo officeRepo = new OfficeRepo();
        private WorkSpaceRepo workSpaceRepo = new WorkSpaceRepo();
        private ObservableCollection<ReservationObject> reservedObjects = new ObservableCollection<ReservationObject>();
        private ObservableCollection<Reservation> bookedDates = new ObservableCollection<Reservation>();
        private ObservableCollection<Workspace> workspaces = new ObservableCollection<Workspace>();
        private ObservableCollection<Service> services = new ObservableCollection<Service>();
        private ObservableCollection<Device> devices = new ObservableCollection<Device>();

        public ExistingReservationWindow()
        {
            InitializeComponent();

            // Luodaan uusi varaus
            this.DataContext = new Reservation();
            cmbOffices.ItemsSource = officeRepo.GetOffices();
           

            tEditDevices.IsEnabled = false;
            tEditServices.IsEnabled = false;

            // Kalentereissa oletuksena tämä päivä - Kaataa ohjelman, siksi kommenttiin.
            //calStart.SelectedDate = DateTime.Today;
            //calEnd.SelectedDate = DateTime.Today;
        }


        /// <summary>
        /// Valitun välilehden otsikko on oranssilla, muut mustalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderFontChange_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tList.IsSelected == true)
            {
                tbList.Foreground = Brushes.Orange;
                tbEdit.Foreground = Brushes.Black;
                tbEditServices.Foreground = Brushes.Black;
                tbEditDevices.Foreground = Brushes.Black;
            }

            if (tEdit.IsSelected == true)
            {
                tbList.Foreground = Brushes.Black;
                tbEdit.Foreground = Brushes.Orange;
                tbEditServices.Foreground = Brushes.Black;
                tbEditDevices.Foreground = Brushes.Black;
            }
            if (tEditDevices.IsSelected == true)
            {
                tbList.Foreground = Brushes.Black;
                tbEdit.Foreground = Brushes.Black;
                tbEditServices.Foreground = Brushes.Black;
                tbEditDevices.Foreground = Brushes.Orange;
            }
            if (tEditServices.IsSelected == true)
            {
                tbList.Foreground = Brushes.Black;
                tbEdit.Foreground = Brushes.Black;
                tbEditServices.Foreground = Brushes.Orange;
                tbEditDevices.Foreground = Brushes.Black;
            }
        }


        /// <summary>
        /// Sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistua tallentamatta muutoksia?", "Huomautus", MessageBoxButton.YesNo, MessageBoxImage.Information);

            //if (result == MessageBoxResult.Yes)
            //{
            //    MessageBox.Show("Muutoksia ei ole tallennettu", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            //    Close();
            //}
            //else
            //{
            //    return;
            //}

            Close();
        }


        /// <summary>
        /// Muokkaa tietoja -välilehdellä Hae Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnGetReservation)
            {
                // Tyhjennetään listat yms.
                workspaces.Clear();
                services.Clear();
                devices.Clear();
                bookedDates.Clear();
                calStart.BlackoutDates.Clear();
                calEnd.BlackoutDates.Clear();
                dgReservedServices.ItemsSource = null;
                dgReservedDevices.ItemsSource = null;

                Reservation reservation = new Reservation();
                Customer customer = new Customer();
                Office office = new Office();

                if (reservationIdBox.Text != string.Empty)
                {
                    reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);
                }

                if (reservationRepo.GetReservation(reservation) != null)
                {
                    reservation = reservationRepo.GetReservationWithData(reservation);
                    customer.Id = reservation.CustomerID;
                    customer = customerRepo.GetCustomer(customer);
                    office.OfficeID = reservation.OfficeID;
                    office = officeRepo.GetOffice(office);

                    cmbOffice.ItemsSource = officeRepo.GetOffices();
                    cmbOffice.SelectedValue = office.City;
                    cmbWorkspace.ItemsSource = workSpaceRepo.GetWorkspaces(office);
                    txtDescription.Text = reservation.ReservationInfo;

                    reservation.Customer = customer;

                    foreach (var item in reservation.ReservedObjects)
                    {

                        if (item.Workspace != null)
                        {
                            // Varaus - toimitila
                            workspaces.Add(item.Workspace);
                            cmbWorkspace.SelectedValue = item.Workspace.WSName;
                        }

                        if (item.Service != null)
                        {
                            services.Add(item.Service);
                            reservedObjects.Add(item);
                        }

                        if (item.Device != null)
                        {
                            devices.Add(item.Device);
                            reservedObjects.Add(item);
                        }
                    }

                    // Varaus

                    calStart.SelectedDate = reservation.ReservationDate;
                    calEnd.SelectedDate = reservation.ReleaseDate;
                    txtDescription.Text = reservation.ReservationInfo;

                    // Asiakas
                    txtFirstName.Text = reservation.Customer.FirstName;
                    txtLastName.Text = reservation.Customer.LastName;
                    txtCompany.Text = reservation.Customer.CompanyName;
                    txtAddress.Text = reservation.Customer.StreetAddress;
                    txtPostal.Text = reservation.Customer.PostalCode;
                    txtCity.Text = reservation.Customer.City;
                    txtPhone.Text = reservation.Customer.Phone;
                    txtEmail.Text = reservation.Customer.Email;

                    // Laitteet ja palvelut
                    dgReservedServices.ItemsSource = services;
                    dgReservedDevices.ItemsSource = devices;


                    cmbServices.ItemsSource = officeRepo.GetOfficeServices(office);
                    cmbDevices.ItemsSource = officeRepo.GetOfficeDevices(office);


                    // Tarkistetaan toimitilan varaus päivämäärät.
                    Workspace workspace = cmbWorkspace.SelectedItem as Workspace;
                    if (workspace != null)
                    {
                        bookedDates = reservationRepo.GetBookedDates(workspace);
                    }

                    foreach (var item in bookedDates)
                    {
                        CalendarDateRange range = new CalendarDateRange(item.ReservationDate, item.ReleaseDate);

                        if (item.ReservationDate == reservation.ReservationDate && item.ReleaseDate == reservation.ReleaseDate)
                        {
                            // Valitun varauksen päivämäärät. (Vapautetaan kalenterista päivät, koska muuten kaatuu.)
                            range = new CalendarDateRange(item.ReservationDate, item.ReleaseDate);
                            calStart.BlackoutDates.Remove(range);
                            calEnd.BlackoutDates.Remove(range);
                        }

                        else
                        {
                            // Muut saman toimitilan varausten päivämäärät.
                            calStart.BlackoutDates.Add(range);
                            calEnd.BlackoutDates.Add(range);
                        }
                    }
                    btnGetReservation.IsEnabled = false;
                }
                myCalandar.DisplayDateStart = reservation.ReservationDate;
                myCalandar.DisplayDateEnd = reservation.ReleaseDate;
            }

            //int quantity;
            if (sender == btnServices)
            {
                if (cmbServices.SelectedItem != null)
                {
                    Service selectedService = cmbServices.SelectedItem as Service;
                    ReservationObject reservationObject = new ReservationObject();

                    if (txtServicesAmount.Text != string.Empty && txtServicesAmount.Text != "0")
                    {
                        selectedService.Quantity = Convert.ToInt32(txtServicesAmount.Text);
                    }

                    else
                    {
                        MessageBox.Show("Syötä määrä", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    foreach (var item in reservedObjects)
                    {
                        if (item.Service != null)
                        {
                            if (item.Service.ServiceName == selectedService.ServiceName)
                            {
                                item.Service.Quantity = selectedService.Quantity;
                                return;
                            }
                        }
                    }

                    services.Add(selectedService);
                    reservationObject.Service = selectedService;
                    reservedObjects.Add(reservationObject);
                    dgReservedServices.ItemsSource = services;
                }
            }

            if (sender == btnDevices)
            {
                if (cmbDevices.SelectedItem != null)
                {
                    Device selectedDevice = cmbDevices.SelectedItem as Device;
                    ReservationObject reservationObject = new ReservationObject();
                    if (txtDevicesAmount.Text != string.Empty)
                    {
                        selectedDevice.Quantity = Convert.ToInt32(txtDevicesAmount.Text);
                    }

                    if (txtDevicesAmount.Text != string.Empty && txtDevicesAmount.Text != "0")
                    {
                        selectedDevice.Quantity = Convert.ToInt32(txtDevicesAmount.Text);
                    }

                    else
                    {
                        MessageBox.Show("Syötä määrä", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    foreach (var item in reservedObjects)
                    {
                        if (item.Device != null)
                        {
                            if (item.Device.Name == selectedDevice.Name)
                            {
                                item.Device.Quantity = selectedDevice.Quantity;
                                return;
                            }
                        }
                    }
                    reservationObject.Device = selectedDevice;
                    devices.Add(selectedDevice);
                    reservedObjects.Add(reservationObject);
                    dgReservedDevices.ItemsSource = devices;
                }
            }
        }


        /// <summary>
        /// Varauksen palvelut -välilehdellä Tallenna muutokset Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation();
            ReservationObject reservationObject = new ReservationObject();
            Customer customer = new Customer();
            Office office = new Office();
            Workspace workspace = new Workspace();
            office = cmbOffice.SelectedItem as Office;
            workspace = cmbWorkspace.SelectedItem as Workspace;
            reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);

            // Lisätään toimitila varaukselle menevään listaan (ObservableCollection).
            reservationObject.Workspace = workspace;
            reservedObjects.Add(reservationObject);

            // Asiakkaan tiedot
            customer.FirstName = txtFirstName.Text;
            customer.LastName = txtLastName.Text;
            customer.CompanyName = txtCompany.Text;
            customer.StreetAddress = txtAddress.Text;
            customer.PostalCode = txtPostal.Text;
            customer.City = txtCity.Text;
            customer.Phone = txtPhone.Text;
            customer.Email = txtEmail.Text;

            // Toimipiste varaukselle
            reservation.OfficeID = office.OfficeID;

            // Varauksen päivämäärät sekä lisätieto.
            reservation.ReservationDate = (DateTime)calStart.SelectedDate;
            reservation.ReleaseDate = (DateTime)calEnd.SelectedDate;

            //Tarkistetaan varauspäivät, alku ei voi olla myöhempänä kuin loppu
            if (calStart.SelectedDate > calEnd.SelectedDate)
            {
                MessageBox.Show("Varauksen lopetuspäivä ei voi olla ennen varauksen aloituspäivää", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (txtDescription != null)
            {
                reservation.ReservationInfo = txtDescription.Text;
            }

            // Lisätään kaikki (Toimitila, palvelut ja laitteet) varaukselle.
            reservation.ReservedObjects = reservedObjects;

            // Lisätään asiakkaan tiedot varaukselle
            reservation.Customer = customer;

            reservationRepo.UpdateReservation(reservation);

            MessageBox.Show("Varauksen tiedot päivitetty", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }


        /// <summary>
        /// Varauksen palvelut -välilehdellä palvelun poistaminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation();
            var removeService = sender as Button;
            var service = removeService.DataContext as Service;

            if (service != null)
            {
                reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);
                reservationRepo.RemoveServiceFromReservation(reservation, service);
                services.Remove(service);
                for (int i = reservedObjects.Count - 1; i >= 0; i--)
                {
                    var item = reservedObjects[i];
                    if (item.Service == service)
                    {
                        reservedObjects.RemoveAt(i);
                    }
                }
            }
        }


        /// <summary>
        /// Varauksen laitteet -välilehdellä laitteen poistaminen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            Reservation reservation = new Reservation();
            var removeDevice = sender as Button;
            var device = removeDevice.DataContext as Device;

            if (device != null)
            {
                reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);
                reservationRepo.RemoveDeviceFromReservation(reservation, device);
                devices.Remove(device);
                for (int i = reservedObjects.Count - 1; i >= 0; i--)
                {
                    var item = reservedObjects[i];
                    if (item.Device == device)
                    {
                        reservedObjects.RemoveAt(i);
                    }
                }
            }
        }


        /// <summary>
        /// Avaa uuden ikkunan, jossa voi lisätä varauksen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Onko asiakas jo luotu järjestelmään?", "Valitse", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ReservationWindow reservationWindow = new ReservationWindow();
                reservationWindow.Show();
            }

            if (result == MessageBoxResult.No)
            {
                ReservationNewCustomerWindow reservationNewCustomerWindow = new ReservationNewCustomerWindow();
                reservationNewCustomerWindow.Show();
            }

            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
        }


        /// <summary>
        /// Estää muiden kuin numeroiden syöttämisen tekstikenttään, johon on määritelty tämä toiminto (PreviewTextInput)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        /// Painettaessa MainWindow:ssa Asiakkaat Buttonia, avautuu CustomerWindow
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
        /// Painettaessa Menusta Laskut..., avautuu Invoice2Window
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
        /// Painettaessa Menusta Palvelut..., avautuu ServiceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Show();
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
        }


        /// <summary>
        /// Menusta Tiedosto -> Toiminnot -> Admin...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.Show();
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
        }


        /// <summary>
        /// Muokkaa tietoja -välilehdellä Seuraava Button, siirtyy seuraavalle välilehdelle. Jos varausta ei ole haettu, ohjelma ilmoittaa siitä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTabDevices_Click(object sender, RoutedEventArgs e)
        {
            if (btnGetReservation.IsEnabled == true)
            {
                MessageBox.Show("Hae varaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                tEdit.IsEnabled = false;
                tEditDevices.IsEnabled = true;
                tEditDevices.IsSelected = true;
                tbEditDevices.Foreground = Brushes.Orange;
                tbEdit.Foreground = Brushes.Black;
            }
        }


        /// <summary>
        /// Varauksen laitteet -välilehdellä Seuraava Button, siirtyy seuraavalle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTabServices_Click(object sender, RoutedEventArgs e)
        {
            tEditDevices.IsEnabled = false;
            tEditServices.IsEnabled = true;
            tEditServices.IsSelected = true;
            tbEditServices.Foreground = Brushes.Orange;
            tbEditDevices.Foreground = Brushes.Black;
        }


        /// <summary>
        /// Varauksen laitteet -välilehdellä Edellinen Button, siirtyy edelliselle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousTabInfo_Click(object sender, RoutedEventArgs e)
        {
            tEditDevices.IsEnabled = false;
            tEdit.IsEnabled = true;
            tEdit.IsSelected = true;
            tbEdit.Foreground = Brushes.Orange;
            tbEditDevices.Foreground = Brushes.Black;

        }


        /// <summary>
        /// Varauksen palvelut -välilehdellä Edellinen Button, siirtyy edelliselle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousTabDevices_Click(object sender, RoutedEventArgs e)
        {
            tEditDevices.IsEnabled = true;
            tEditDevices.IsSelected = true;
            tEditServices.IsEnabled = false;
            tbEditDevices.Foreground = Brushes.Orange;
            tbEditServices.Foreground = Brushes.Black;
        }


        /// <summary>
        /// Lista varauksista -välilehdellä Hae Button, hakee toimipisteen mukaan varaukset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetReservationinfo_Click(object sender, RoutedEventArgs e)
        {

            var office = cmbOffices.SelectedItem as Office;
            if (office != null)
            {
                dgBookedWorkspaces.ItemsSource = reservationRepo.GetBookedWorkspaces(office);
            }

        }


        /// <summary>
        /// ExistingReservationWindow Shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingReservationWindow_KeyDown(object sender, KeyEventArgs e)
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
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi. Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reservationIdBox_MouseEnter(object sender, MouseEventArgs e)
        {
            reservationIdBox.Focusable = true;
        }
    }
}
