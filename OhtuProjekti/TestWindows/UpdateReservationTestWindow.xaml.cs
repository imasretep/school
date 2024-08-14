using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
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
using vt_systems.DeviceData;
using vt_systems.OfficeData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for UpdateReservationTestWindow.xaml
    /// </summary>
    public partial class UpdateReservationTestWindow : Window
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

        public UpdateReservationTestWindow()
        {
            InitializeComponent();
            this.DataContext = new Reservation();



        }

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
                reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);

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
                }
            }

          
            if (sender == btnServices)
            {
                if (cmbServices.SelectedItem != null)
                {
                    Service selectedService = cmbServices.SelectedItem as Service;
                    ReservationObject reservationObject = new ReservationObject();
                    selectedService.Quantity = Convert.ToInt32(txtServicesAmount.Text);

                    foreach (var item in services)
                    {

                        if (item.ServiceName == selectedService.ServiceName)
                        {
                            item.Quantity += selectedService.Quantity;
                            cmbServices.SelectedItem = null;
                            txtServicesAmount.Text = string.Empty;
                            return;

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
                    selectedDevice.Quantity = Convert.ToInt32(txtDevicesAmount.Text);

                    foreach (var item in devices)
                    {

                        if (item.Name == selectedDevice.Name)
                        {
                            item.Quantity += selectedDevice.Quantity;
                            cmbServices.SelectedItem = null;
                            txtServicesAmount.Text = string.Empty;
                            return;
                        }

                    }

                    reservationObject.Device = selectedDevice;
                    devices.Add(selectedDevice);
                    reservedObjects.Add(reservationObject);
                    dgReservedDevices.ItemsSource = devices;
                }
            }
        }

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

            if (txtDescription != null)
            {
                reservation.ReservationInfo = txtDescription.Text;
            }

            // Lisätään kaikki (Toimitila, palvelut ja laitteet) varaukselle.
            reservation.ReservedObjects = reservedObjects;

            // Lisätään asiakkaan tiedot varaukselle
            reservation.Customer = customer;

            reservationRepo.UpdateReservation(reservation);

        }

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
    }
}
