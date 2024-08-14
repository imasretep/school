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
using vt_systems.DeviceData;
using vt_systems.OfficeData;
using vt_systems.WorkspaceData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for DeviceTestWindow.xaml
    /// </summary>
    public partial class DeviceTestWindow : Window
    {
        private DeviceRepo deviceRepo = new DeviceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();

        public DeviceTestWindow()
        {
            InitializeComponent();

            myDeviceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            priceWBox.IsEnabled = false;
            priceMBox.IsEnabled = false;


            this.DataContext = new Device();
            dgDevices.ItemsSource = deviceRepo.GetDevices();
            cmbOffices.ItemsSource = officeRepo.GetOffices();
        }

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
                    priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    priceWBox.IsEnabled = true;
                    priceMBox.IsEnabled = true;

                    nameBox.Text = device.Name;
                    descriptionBox.Text = device.Description;
                    priceHBox.Text = device.PriceByHour.ToString();
                    priceDBox.Text = device.PriceByDay.ToString();
                    priceWBox.Text = device.PriceByWeek.ToString();
                    priceMBox.Text = device.PriceByMonth.ToString();

                    

                }
                else
                {
                    myDeviceIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimitila id");
            }
        }

        private void UpdateDevice_Click(object sender, RoutedEventArgs e)
        {

            var updateDevice = this.DataContext as Device;
            updateDevice.Name = nameBox.Text;
            updateDevice.Description = descriptionBox.Text;
            updateDevice.PriceByHour = Convert.ToDouble(priceHBox.Text);
            updateDevice.PriceByDay = Convert.ToDouble(priceDBox.Text);
            updateDevice.PriceByWeek = Convert.ToDouble(priceWBox.Text);
            updateDevice.PriceByMonth = Convert.ToDouble(priceMBox.Text);

            if (updateDevice.Name == string.Empty || updateDevice.Description == string.Empty)
            {
                MessageBox.Show("Tietojen päivitys ei onnistunut!");
                return;
            }
            deviceRepo.UpdateDevice(updateDevice);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgDevices.ItemsSource = deviceRepo.GetDevices();
        }



        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (cmbOffices.SelectedItem != null)
            {
                var newDevice = new Device
                {

                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    PriceByHour = Convert.ToDouble(txtPriceH.Text),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text),
                    PriceByWeek = Convert.ToDouble(txtPriceW.Text),
                    PriceByMonth = Convert.ToDouble(txtPriceM.Text),
                    OfficeID = office.OfficeID


                };

                if (newDevice.Name == string.Empty)
                {
                    MessageBox.Show("Syötä laitteen nimi");
                    return;
                }
                if (newDevice.Description == string.Empty)
                {
                    MessageBox.Show("Syötä laitteen kuvaus");
                    return;
                }
                deviceRepo.AddNewDevice(newDevice, office);
                MessageBox.Show("Laitteen lisääminen onnistui!");
                dgDevices.ItemsSource = deviceRepo.GetDevices();
            }
            else
            {
                MessageBox.Show("Laitteen lisääminen ei onnistunut!");
                return;
            }
        }




        private void DeleteDevice_Click(object sender, RoutedEventArgs e)
        {
            var deleteDevice = this.DataContext as Device;
            int deviceID;
            if (int.TryParse(deleteDeviceIdBox.Text, out deviceID))
            {

                deviceRepo.DeleteDevice(deleteDevice);
                dgDevices.ItemsSource = deviceRepo.GetDevices();
                return;

            }
            else
            {
                MessageBox.Show("Tietojen päivitys ei onnistunut!");
            }
        }


    }
}
