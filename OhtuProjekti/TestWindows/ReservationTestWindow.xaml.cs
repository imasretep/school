using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
    /// Interaction logic for ReservationTestWindow.xaml
    /// </summary>
    public partial class ReservationTestWindow : Window
    {
        ReservationRepo reservationRepo = new ReservationRepo();
        ObservableCollection<Workspace> workspaces = new ObservableCollection<Workspace>();
        ObservableCollection<Service> services = new ObservableCollection<Service>();
        ObservableCollection<Device> devices = new ObservableCollection<Device>();
        OfficeRepo officeRepo = new OfficeRepo();
        public ReservationTestWindow()
        {
            InitializeComponent();
            this.DataContext = new Reservation();
            cmbOffices.ItemsSource = officeRepo.GetOffices();

        }

        private void GetReservation_Click(object sender, RoutedEventArgs e)
        {
            // Tyhjennetään listat yms.
            workspaces.Clear();
            services.Clear();
            devices.Clear();
            dgReservedWs.ItemsSource = workspaces;
            dgReservedServices.ItemsSource = null;
            dgReservedDevices.ItemsSource = null;

            txtResId.Text = string.Empty;
            txtResDate.Text = string.Empty;
            txtRelDate.Text = string.Empty;

            txtCustomerId.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;

            Reservation reservation = new Reservation();
            reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);
            reservation = reservationRepo.GetReservationWithData(reservation);

            foreach (var item in reservation.ReservedObjects)
            {
                if (item.Workspace != null)
                {
                    workspaces.Add(item.Workspace);
                }
                if (item.Service != null)
                {
                    services.Add(item.Service);
                }
                if (item.Device != null)
                {
                    devices.Add(item.Device);
                }
            }
            this.DataContext = reservation;
            CustomerRepo customerRepo = new CustomerRepo();
            Customer customer = new Customer();
            customer.Id = reservation.CustomerID;
            customer = customerRepo.GetCustomer(customer);

            txtCustomerId.DataContext = customer;
            txtFirstName.DataContext = customer;
            txtLastName.DataContext = customer;

            dgReservedWs.ItemsSource = workspaces;
            dgReservedServices.ItemsSource = services;
            dgReservedDevices.ItemsSource = devices;


            myCalandar.DisplayDateStart = reservation.ReservationDate;
            myCalandar.DisplayDateEnd = reservation.ReleaseDate;





        }

        private void DeleteReservation_Click(object sender, RoutedEventArgs e)
        {

            // HUOM! kaatuu jos asiakkaalla on lasku. -> Lasku pitää poistaa myös.
            Reservation reservation = new Reservation();
            reservation.ReservationId = Convert.ToInt32(reservationIdBox.Text);
            reservation = reservationRepo.GetReservationWithData(reservation);

            foreach (var item in reservation.ReservedObjects)
            {
                if (item.Service != null)
                {
                    reservationRepo.RemoveServiceFromReservation(reservation, item.Service);
                }
                if (item.Device != null)
                {
                    reservationRepo.RemoveDeviceFromReservation(reservation, item.Device);
                }
                if (item.Workspace != null)
                {
                    reservationRepo.RemoveWorkspaceFromReservation(reservation, item.Workspace);
                }

            }

            reservationRepo.RemoveCustomerFromReservation(reservation, reservation.CustomerID);
            reservationRepo.RemoveOfficeFromReservation(reservation, reservation.OfficeID);

            // Tätä ei tarvitse välttämättä, koska edelliset poistavat kaikki rivit varaukselta, jolloin koko varaus häviää.
            //reservationRepo.DeleteReservation(reservation);

        }


        private void NewReservation_Click(object sender, RoutedEventArgs e)
        {
            NewReservationTestWindow newReservationTestWindow = new NewReservationTestWindow();
            newReservationTestWindow.Show();
        }

        private void NewReservationExistingCustomer_Click(object sender, RoutedEventArgs e)
        {
            NewReservationExistingCustomer newReservationExistingCustomer = new NewReservationExistingCustomer();
            newReservationExistingCustomer.Show();
        }

        private void UpdateReservation_Click(object sender, RoutedEventArgs e)
        {
            UpdateReservationTestWindow updateReservationTestWindow = new UpdateReservationTestWindow();
            updateReservationTestWindow.Show();

        }

        private void GetReservationinfo_Click(object sender, RoutedEventArgs e)
        {
            
            var office = cmbOffices.SelectedItem as Office;
            if(office != null)
            {
                dgBookedWorkspaces.ItemsSource = reservationRepo.GetBookedWorkspaces(office);
            }

        }
    }
}
