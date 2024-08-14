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
using System.Xml.Linq;
using vt_systems.DeviceData;
using vt_systems.OfficeData;
using vt_systems.ServiceData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for ServiceTestWindow.xaml
    /// </summary>
    public partial class ServiceTestWindow : Window
    {
        private ServiceRepo serviceRepo = new ServiceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();

        public ServiceTestWindow()
        {
            InitializeComponent();

            myServiceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceBox.IsEnabled = false;
            priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            priceWBox.IsEnabled = false;
            priceMBox.IsEnabled = false;


            this.DataContext = new Service();
            dgService.ItemsSource = serviceRepo.GetServices();
            cmbOffices.ItemsSource = officeRepo.GetOffices();
        }

        private void GetService_Click(object sender, RoutedEventArgs e)
        {
            var service = new Service();
            int serviceID;

            service = this.DataContext as Service;

            if (int.TryParse(myServiceIdBox.Text, out serviceID))
            {
                service = this.DataContext as Service;

                if (serviceRepo.GetService(service) != null)
                {
                    myServiceIdBox.IsEnabled = false;
                    nameBox.IsEnabled = true;
                    descriptionBox.IsEnabled = true;
                    priceBox.IsEnabled = true;
                    priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    priceWBox.IsEnabled = true;
                    priceMBox.IsEnabled = true;

                    nameBox.Text = service.ServiceName;
                    descriptionBox.Text =service.ServiceDescription;
                    priceBox.Text =service.ServicePrice.ToString();
                    priceHBox.Text = service.PriceByHour.ToString();
                    priceDBox.Text = service.PriceByDay.ToString();
                    priceWBox.Text = service.PriceByWeek.ToString();
                    priceMBox.Text = service.PriceByMonth.ToString();

                }
                else
                {
                    myServiceIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen palvelu id");
            }
        }

        private void UpdateService_Click(object sender, RoutedEventArgs e)
        {

            var updateService = this.DataContext as Service;
            updateService.ServiceName = nameBox.Text;
            updateService.ServiceDescription = descriptionBox.Text;
            updateService.ServicePrice = Convert.ToDouble(priceBox.Text);
            updateService.PriceByHour = Convert.ToDouble(priceHBox.Text);
            updateService.PriceByDay = Convert.ToDouble(priceDBox.Text);
            updateService.PriceByWeek = Convert.ToDouble(priceWBox.Text);
            updateService.PriceByMonth = Convert.ToDouble(priceMBox.Text);

            if (updateService.ServiceName == string.Empty || updateService.ServiceDescription == string.Empty)
            {
                MessageBox.Show("Tietojen päivitys ei onnistunut!");
                return;
            }
            serviceRepo.UpdateService(updateService);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgService.ItemsSource = serviceRepo.GetServices();
        }



        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (txtName.Text != string.Empty && txtDescription.Text != string.Empty && cmbOffices.SelectedItem != null)
            {
                var newService = new Service
                {

                    ServiceName = txtName.Text,
                    ServiceDescription = txtDescription.Text,
                    ServicePrice = Convert.ToDouble(txtPriceBox.Text),
                    PriceByHour = Convert.ToDouble(txtPriceH.Text),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text),
                    PriceByWeek = Convert.ToDouble(txtPriceW.Text),
                    PriceByMonth = Convert.ToDouble(txtPriceM.Text),
                    OfficeID = office.OfficeID


                };
                serviceRepo.AddNewService(newService, office);
                MessageBox.Show("Palvelun lisääminen onnistui!");
                dgService.ItemsSource = serviceRepo.GetServices();
            }
            else
            {
                MessageBox.Show("Palvelun lisäys ei onnistunut!");
                return;
            }
        }




        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            var deleteService = this.DataContext as Service;
            int serviceID;
            if (int.TryParse(deleteServiceIdBox.Text, out serviceID))
            {

                serviceRepo.DeleteService(deleteService);
                dgService.ItemsSource = serviceRepo.GetServices();
                return;

            }
            else
            {
                MessageBox.Show("Tietojen päivitys ei onnistunut!");
            }
        }

    }
}
