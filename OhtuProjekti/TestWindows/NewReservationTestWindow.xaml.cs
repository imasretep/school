﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NewReservationTestWindow.xaml
    /// </summary>
    public partial class NewReservationTestWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private WorkSpaceRepo workSpaceRepo = new WorkSpaceRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private CustomerRepo customerRepo = new CustomerRepo();
        private ObservableCollection<ReservationObject> reservedObjects = new ObservableCollection<ReservationObject>();
        ObservableCollection<Service> services = new ObservableCollection<Service>();
        ObservableCollection<Device> devices = new ObservableCollection<Device>();
        public NewReservationTestWindow()
        {
            InitializeComponent();

            cmbOffice.ItemsSource = officeRepo.GetOffices();


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (sender == btnServices)
            {
                if (cmbServices.SelectedItem != null)
                {
                    Service selectedService = cmbServices.SelectedItem as Service;
                    ReservationObject reservationObject = new ReservationObject();
                    selectedService.Quantity = Convert.ToInt32(txtServicesAmount.Text);
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

                if(txtDescription != null)
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



            }
        }
    }
}

