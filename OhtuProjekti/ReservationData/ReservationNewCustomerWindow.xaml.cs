﻿using System;
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
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for ReservationNewCustomerWindow.xaml
    /// </summary>
    public partial class ReservationNewCustomerWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private WorkSpaceRepo workSpaceRepo = new WorkSpaceRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private CustomerRepo customerRepo = new CustomerRepo();
        private ObservableCollection<ReservationObject> reservedObjects = new ObservableCollection<ReservationObject>();
        ObservableCollection<Service> services = new ObservableCollection<Service>();
        ObservableCollection<Device> devices = new ObservableCollection<Device>();


        public ReservationNewCustomerWindow()
        {
            InitializeComponent();

            cmbOffice.ItemsSource = officeRepo.GetOffices();

            // Varausta ei voi tehdä menneisyyteen, kalentereissa oletuksena tämä päivä
            calStart.DisplayDateStart = DateTime.Today;
            //calStart.SelectedDate = DateTime.Today;
            calEnd.DisplayDateStart = DateTime.Today;
            //calEnd.SelectedDate = DateTime.Today;

            tAddDevice.IsEnabled = false;
            tAddService.IsEnabled = false;
        }


        private void cmbOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Office selectedOffice = cmbOffice.SelectedItem as Office;
            cmbWorkspace.ItemsSource = workSpaceRepo.GetWorkspaces(selectedOffice);
            cmbServices.ItemsSource = officeRepo.GetOfficeServices(selectedOffice);
            cmbDevices.ItemsSource = officeRepo.GetOfficeDevices(selectedOffice);

        }

        private void cmbWorkspace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calStart.BlackoutDates.Clear();
            calEnd.BlackoutDates.Clear();
            ObservableCollection<Reservation> reservation = new ObservableCollection<Reservation>();
            Workspace workspace = cmbWorkspace.SelectedItem as Workspace;

            if (workspace != null)
            {
                reservation = reservationRepo.GetBookedDates(workspace);
            }

            foreach (var item in reservation)
            {
                CalendarDateRange range = new CalendarDateRange(item.ReservationDate, item.ReleaseDate);
                calStart.BlackoutDates.Add(range);
                calEnd.BlackoutDates.Add(range);
            }

        }


        /// <summary>
        /// Varauksen tallentaminen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveReservation_Click(object sender, RoutedEventArgs e)
        {
            // Ohjelma huomauttaa, jos valittujen palveluiden määrää ei ole syötetty
            if (sender == btnServices)
            {
                if (cmbServices.SelectedItem != null)
                {
                    Service selectedService = cmbServices.SelectedItem as Service;
                    ReservationObject reservationObject = new ReservationObject();
                    if (txtServicesAmount.Text != string.Empty)
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
                    selectedService.Quantity = Convert.ToInt32(txtServicesAmount.Text);
                    services.Add(selectedService);
                    reservationObject.Service = selectedService;
                    reservedObjects.Add(reservationObject);
                    dgReservedServices.ItemsSource = services;
                }
            }

            // Ohjelma huomauttaa, jos valittujen laitteiden määrää ei ole syötetty
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
                    selectedDevice.Quantity = Convert.ToInt32(txtDevicesAmount.Text);
                    reservationObject.Device = selectedDevice;
                    devices.Add(selectedDevice);
                    reservedObjects.Add(reservationObject);
                    dgReservedDevices.ItemsSource = devices;
                }
            }

            if (sender == btnBook)
            {
                Reservation reservation = new Reservation();
                ReservationObject reservationObject = new ReservationObject();
                Customer customer = new Customer();
                Office office = new Office();
                Workspace workspace = new Workspace();
                office = cmbOffice.SelectedItem as Office;
                workspace = cmbWorkspace.SelectedItem as Workspace;

                // Lisätään toimitila varaukselle menevään listaan (ObservableCollection).
                reservationObject.Workspace = workspace;
                reservedObjects.Add(reservationObject);

                // Ohjelma ilmoittaa jos tarvittavia asiakastietoja ei ole syötetty.
                if(txtFirstName.Text ==string.Empty)
                {
                    MessageBox.Show("Syötä etunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtLastName.Text == string.Empty)
                {
                    MessageBox.Show("Syötä Sukunimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtAddress.Text == string.Empty)
                {
                    MessageBox.Show("Syötä katuosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtPostal.Text == string.Empty)
                {
                    MessageBox.Show("Syötä postinumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtCity.Text == string.Empty)
                {
                    MessageBox.Show("Syötä postitoimipaikka", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtPhone.Text == string.Empty)
                {
                    MessageBox.Show("Syötä Puhelinnumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                if (txtEmail.Text == string.Empty)
                {
                    MessageBox.Show("Syötä sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                if (!txtEmail.Text.Contains("@"))
                {
                    MessageBox.Show("Syötä kelvollinen sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                // Asiakkaan tiedot
                customer.FirstName = txtFirstName.Text;
                customer.LastName = txtLastName.Text;
                customer.CompanyName =txtCompany.Text;
                customer.StreetAddress = txtAddress.Text;
                customer.PostalCode = txtPostal.Text;
                customer.City = txtCity.Text;
                customer.Phone = txtPhone.Text;
                customer.Email = txtEmail.Text;

                // Varauksen toimipisteen ja toimitilan valinnan tarkistukset
                if (office == null)
                {
                    MessageBox.Show("Valitse toimipiste", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (workspace == null)
                {
                    MessageBox.Show("Valitse toimitila", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                // Toimipiste varaukselle
                reservation.OfficeID = office.OfficeID;

                // Varauksen aloituspäivän valinnan tarkistus
                if (calStart.SelectedDate == null)
                {
                    MessageBox.Show("Valitse varauksen aloituspäivä.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                // Varauksen aloituspäivä.
                reservation.ReservationDate = (DateTime)calStart.SelectedDate;
                // Varauksen lopetuspäivän valinnan tarkistus
                if (calEnd.SelectedDate == null)
                {
                    MessageBox.Show("Valitse varauksen Päättymispäivä.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }
                // Varauksen lopetusspäivä
                reservation.ReleaseDate = (DateTime)calEnd.SelectedDate;
                if (calStart.SelectedDate > calEnd.SelectedDate)
                {
                    MessageBox.Show("Varauksen lopetuspäivä ei voi olla ennen varauksen aloituspäivää", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetTabHeaders();
                    return;
                }

                if (txtDescription != null)
                {
                    reservation.ReservationInfo = txtDescription.Text;
                }

                // Lisätään kaikki (Toimitila, palvelut ja laitteet) varaukselle.
                reservation.ReservedObjects = reservedObjects;

                // Viedään uusi asiakas tietokantaan ja haetaan sieltä asiakkaan id.
                customerRepo.AddNewCustomer(customer);
                customer = customerRepo.GetCustomerID(customer);

                // Lisätään asiakkaan tiedot varaukselle
                reservation.Customer = customer;


                reservationRepo.AddNewReservation(reservation);
                reservation = reservationRepo.GetReservationId(reservation, workspace);

                reservationRepo.AddNewReservedObject(reservation);

                MessageBox.Show("Varauksen tallennus onnistui!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void ResetTabHeaders()
        {
            tAdd.IsSelected = true;
            tAddDevice.IsEnabled = false;
            tAddService.IsEnabled = false;
            tbAdd.Foreground = Brushes.Orange;
            tbAddService.Foreground = Brushes.Black;
        }


        /// <summary>
        /// Sulkee ikkunan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Haluatko varmasti poistua tallentamatta varausta?", "Huomautus", MessageBoxButton.YesNo, MessageBoxImage.Information);

            //if (result == MessageBoxResult.Yes)
            //{
            //    MessageBox.Show("Varausta ei ole tallennettu", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            //    Close();
            //}
            //else
            //{
            //    return;
            //}

            Close();
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
                tbAddDevice.Foreground = Brushes.Black;
                tbAddService.Foreground = Brushes.Black;
            }

            if (tAddDevice.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Black;
                tbAddDevice.Foreground = Brushes.Orange;
                tbAddService.Foreground = Brushes.Black;
            }

            if (tAddService.IsSelected == true)
            {
                tbAdd.Foreground = Brushes.Black;
                tbAddDevice.Foreground = Brushes.Black;
                tbAddService.Foreground = Brushes.Orange;
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
        /// Painettaessa MainWindow:ssa Asiakkaat Buttonia, avautuu CustomerWindow.
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
        /// Uusi varaus -välilehdellä Seuraava Button, vie seuraavalle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTabDevice_Click(object sender, RoutedEventArgs e)
        {
            tAddDevice.IsEnabled = true;
            tAddDevice.IsSelected = true;
            tAdd.IsEnabled = false;
            tbAdd.Foreground = Brushes.Black;
            tbAddDevice.Foreground = Brushes.Orange;
        }


        /// <summary>
        /// Lisää laitteet -välilehdellä Seuraava Vutton, vie seuraavalla välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextTabService_Click(object sender, RoutedEventArgs e)
        {
            tAddDevice.IsEnabled = false;
            tAddService.IsEnabled = true;
            tAddService.IsSelected = true;
            tbAdd.Foreground = Brushes.Black;
            tbAddDevice.Foreground = Brushes.Black;
            tbAddService.Foreground = Brushes.Orange;
        }


        /// <summary>
        /// Lisää laitteet -välilehdellä Edellinen Button, vie edelliselle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousTabInfo_Click(object sender, RoutedEventArgs e)
        {
            tAddService.IsEnabled = false;
            tAdd.IsEnabled = true;
            tAdd.IsSelected = true;
            tbAdd.Foreground = Brushes.Orange;
            tbAddDevice.Foreground = Brushes.Black;

        }


        /// <summary>
        /// Lisää palvelut -välilehdellä Edellinen Button, vie edelliselle välilehdelle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousTabDevices_Click(object sender, RoutedEventArgs e)
        {
            tAddDevice.IsEnabled = true;
            tAddDevice.IsSelected = true;
            tAddService.IsEnabled = false;
            tbAddDevice.Foreground = Brushes.Orange;
            tbAddService.Foreground = Brushes.Black;
        }


        /// <summary>
        /// MainWindow Shortcuts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
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
    }
}

