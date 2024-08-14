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

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for DeviceWindow.xaml
    /// </summary>
    public partial class DeviceWindow : Window
    {
        private ReservationRepo reservationRepo = new ReservationRepo();
        private DeviceRepo deviceRepo = new DeviceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();

        public DeviceWindow()
        {
            InitializeComponent();

            myDeviceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            //priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            //priceWBox.IsEnabled = false;
            //priceMBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            // Luodaan uusi laite
            this.DataContext = new Device();

            // Haetaan laitteet DataGridiin Lista laitteista -välilehdellä
            dgDevices.ItemsSource = deviceRepo.GetDevices();

            // Valitaan toimipaikka ComboBoxiin Lisää laite -välilehdellä
            cmbOffices.ItemsSource = officeRepo.GetOffices();
        }


        /// <summary>
        /// Haetaan laitteiden tiedot Laite ID:n perusteella Muokkaa laitetietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDevice_Click(object sender, RoutedEventArgs e)
        {
            var device = new Device();
            int deviceID;

            device = this.DataContext as Device;
            if (int.TryParse(myDeviceIdBox.Text, out deviceID))
            {
                device = this.DataContext as Device;

                if (deviceRepo.GetDevice(device) != null)
                {
                    myDeviceIdBox.IsEnabled = false;
                    nameBox.IsEnabled = true;
                    descriptionBox.IsEnabled = true;
                    //priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    //priceWBox.IsEnabled = true;
                    //priceMBox.IsEnabled = true;
                    cmbIsActive.IsEnabled = true;

                    nameBox.Text = device.Name;
                    descriptionBox.Text = device.Description;
                    //priceHBox.Text = device.PriceByHour.ToString();                   
                    priceDBox.Text = device.PriceByDay.ToString();
                    //priceWBox.Text = device.PriceByWeek.ToString();
                    //priceMBox.Text = device.PriceByMonth.ToString();
                    if (device.IsInActive == false)
                    {
                        cmbIsActive.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbIsActive.SelectedIndex = 1;
                    }
                }
                else
                {
                    myDeviceIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimitila id", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Päivitetään laitteiden tiedot Muokkaa laitetietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDevice_Click(object sender, RoutedEventArgs e)
        {
            var updateDevice = this.DataContext as Device;
            updateDevice.Name = nameBox.Text;
            updateDevice.Description = descriptionBox.Text;
            //updateDevice.PriceByHour = Convert.ToDouble(priceHBox.Text, CultureInfo.InvariantCulture);
            if (priceDBox.Text == string.Empty)
            {
                MessageBox.Show("Syötä laitteelle hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
                
            }
            if (priceDBox.Text == "0")
            {
                var result = MessageBox.Show("Haluatko varmasti asettaa laitteen hinnaksi 0€?", "Laitteen hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            updateDevice.PriceByDay = Convert.ToDouble(priceDBox.Text, CultureInfo.InvariantCulture);
            //updateDevice.PriceByWeek = Convert.ToDouble(priceWBox.Text, CultureInfo.InvariantCulture);
            //updateDevice.PriceByMonth = Convert.ToDouble(priceMBox.Text, CultureInfo.InvariantCulture);

            if (cmbIsActive.SelectedIndex == 0)
            {
                updateDevice.IsInActive = false;
            }
            else
            {
                updateDevice.IsInActive = true;
            }

            if (updateDevice.Name == string.Empty)
            {
                MessageBox.Show("Syötä laitteen nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateDevice.Description == string.Empty)
            {
                MessageBox.Show("Syötä laitteen kuvaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            deviceRepo.UpdateDevice(updateDevice);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgDevices.ItemsSource = deviceRepo.GetDevices();

            ResetAll();
        }


        /// <summary>
        /// Lisätään uusi laite Lisää laite -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (cmbOffices.SelectedItem != null)
            {
                if(txtPriceD.Text== string.Empty)
                {
                    MessageBox.Show("Syötä laitteen hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (txtPriceD.Text == "0")
                {
                    var result = MessageBox.Show("Haluatko varmasti asettaa laitteen hinnaksi 0€?", "Laitteen hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                var newDevice = new Device
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    //PriceByHour = Convert.ToDouble(txtPriceH.Text, CultureInfo.InvariantCulture),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text, CultureInfo.InvariantCulture),
                    //PriceByWeek = Convert.ToDouble(txtPriceW.Text, CultureInfo.InvariantCulture),
                    //PriceByMonth = Convert.ToDouble(txtPriceM.Text, CultureInfo.InvariantCulture),
                    OfficeID = office.OfficeID
                };

                if (newDevice.Name == string.Empty)
                {
                    MessageBox.Show("Syötä laitteen nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (newDevice.Description == string.Empty)
                {
                    MessageBox.Show("Syötä laitteen kuvaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                deviceRepo.AddNewDevice(newDevice, office);
                MessageBox.Show("Laitteen lisääminen onnistui!");
                dgDevices.ItemsSource = deviceRepo.GetDevices();
                ResetAll();
            }

            else
            {
                MessageBox.Show("Laitteen lisääminen ei onnistunut!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }


        /// <summary>
        /// Poistetaan laite Lista laitteista -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            var device = dgDevices.SelectedItem as Device;

            ObservableCollection<ReservationObject> reservedDevices = new ObservableCollection<ReservationObject>();

            if (dgDevices.SelectedIndex == -1)
            {
                MessageBox.Show("Valitse poistettava laite...", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            reservedDevices = reservationRepo.GetReservedDevices(device);

            if (reservedDevices.Count != 0)
            {
                var result = MessageBox.Show("Laitetta ei voida poistaa. Laite on käytössä varauksessa ja/tai aktiivisessa laskussa", "Laitteen poisto.", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    return;
                }
            }

            if (reservedDevices.Count == 0)
            {
                var result = MessageBox.Show("Haluatko varmasti poistaa valitun laitteen?", "Laitteen poisto.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    deviceRepo.DeleteDevice(device);
                    dgDevices.ItemsSource = deviceRepo.GetDevices();
                }
            }


            //var deleteDevice = this.DataContext as Device;
            //int deviceID;
            //if (int.TryParse(deleteDeviceIdBox.Text, out deviceID))
            //{

            //    deviceRepo.DeleteDevice(deleteDevice);
            //    dgDevices.ItemsSource = deviceRepo.GetDevices();
            //    return;

            //}
            //else
            //{
            //    MessageBox.Show("Tietojen päivitys ei onnistunut!");
            //}
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
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Orange;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Black;
            }

            if (tSearch.IsSelected == true)
            {
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Orange;
                tbList.Foreground = Brushes.Black;
            }

            if (tList.IsSelected == true)
            {
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Orange;
            }
        }


        /// <summary>
        /// Sulkee ikkunan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        ///  Estää muiden kuin numeroiden syöttämisen tekstikenttään, johon on määritelty tämä toiminto (PreviewTextInput).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        /// Painettaessa Menusta Asiakkaat..., avautuu CustomerWindow.
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
            Close();
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
            Close();
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
            Close();
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
            Close();
        }


        /// <summary>
        /// DeviceWindow Shortcuts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Asiakkaat -ikkuna
            if (e.Key == Key.F1)
            {
                CustomerWindow_Click(sender, e);
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
        /// Tyhjentää käyttöliittymän tekstikentät
        /// </summary>
        private void ResetAll()
        {
            myDeviceIdBox.Text = string.Empty;
            nameBox.Text = string.Empty;
            descriptionBox.Text = string.Empty;
            priceDBox.Text = string.Empty;
            cmbIsActive.SelectedIndex = -1;
            myDeviceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;


            txtName.Text = string.Empty;
            txtPriceD.Text = string.Empty;
            txtDescription.Text = string.Empty;
            cmbOffices.SelectedIndex = -1;
        }


        /// <summary>
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi. Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myDeviceIdBox_MouseEnter(object sender, MouseEventArgs e)
        {
            myDeviceIdBox.Focusable = true;
        }
    }
}
