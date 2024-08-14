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
using vt_systems.DeleteData;
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;

namespace vt_systems.ServiceData
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ServiceRepo serviceRepo = new ServiceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();

        public ServiceWindow()
        {
            InitializeComponent();

            myServiceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            //priceBox.IsEnabled = false;
            //priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            //priceWBox.IsEnabled = false;
            //priceMBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            // Luodaan uusi palvelu
            this.DataContext = new Service();

            // Haetaan palvelut DataGridiin Lista palveluista -välilehdellä
            dgService.ItemsSource = serviceRepo.GetServices();

            // Valitaan toimipiste ComboBoxiin Lisää palvelu -välilehdellä
            cmbOffices.ItemsSource = officeRepo.GetOffices();
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
        /// Painettaessa ExistingReservationWindow:sta Uusi varaus Buttonia, avautuu ReservationWindow
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
        /// Painettaessa Menusta Varaukset..., avautuu ExistingReservationWindow
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
        }


        /// <summary>
        /// Haetaan palveluiden tiedot Palvelu ID:n perusteella Muokkaa palvelutietoja -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    //priceBox.IsEnabled = true;
                    //priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    //priceWBox.IsEnabled = true;
                    //priceMBox.IsEnabled = true;
                    cmbIsActive.IsEnabled = true;

                    nameBox.Text = service.ServiceName;
                    descriptionBox.Text = service.ServiceDescription;
                    //priceBox.Text = service.ServicePrice.ToString();
                    //priceHBox.Text = service.PriceByHour.ToString();
                    priceDBox.Text = service.PriceByDay.ToString();
                    //priceWBox.Text = service.PriceByWeek.ToString();
                    //priceMBox.Text = service.PriceByMonth.ToString();

                    if (service.IsInActive != true)
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
                    myServiceIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen palvelu id", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Päivitetään palvelun tiedot Muokkaa palvelutietoja -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateService_Click(object sender, RoutedEventArgs e)
        {
            var updateService = this.DataContext as Service;
            updateService.ServiceName = nameBox.Text;
            updateService.ServiceDescription = descriptionBox.Text;

            //if (priceHBox.Text == string.Empty)
            //{
            //    priceHBox.Text = "0";
            //}
            if (priceDBox.Text == string.Empty)
            {
                MessageBox.Show("Syötä palvelulle hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (priceDBox.Text == "0")
            {
                var result = MessageBox.Show("Haluatko varmasti asettaa palvelun hinnaksi 0€?", "Palvelun hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            //if (priceWBox.Text == string.Empty)
            //{
            //    priceWBox.Text = "0";
            //}
            //if (priceMBox.Text == string.Empty)
            //{
            //    priceMBox.Text = "0";
            //}
            //updateService.ServicePrice = Convert.ToDouble(priceBox.Text, CultureInfo.InvariantCulture);
            //updateService.PriceByHour = Convert.ToDouble(priceHBox.Text, CultureInfo.InvariantCulture);
            updateService.PriceByDay = Convert.ToDouble(priceDBox.Text, CultureInfo.InvariantCulture);
            //updateService.PriceByWeek = Convert.ToDouble(priceWBox.Text, CultureInfo.InvariantCulture);
            //updateService.PriceByMonth = Convert.ToDouble(priceMBox.Text, CultureInfo.InvariantCulture);

            if (cmbIsActive.SelectedIndex == 0)
            {
                updateService.IsInActive = false;
            }

            else
            {
                updateService.IsInActive = true;
            }


            if (updateService.ServiceName == string.Empty)
            {
                MessageBox.Show("Syötä laitteen nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateService.ServiceDescription == string.Empty)
            {
                MessageBox.Show("Syötä laitteen kuvaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            serviceRepo.UpdateService(updateService);
            MessageBox.Show("Tietojen päivitys onnistui!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            dgService.ItemsSource = serviceRepo.GetServices();
            ResetAll();
        }


        /// <summary>
        /// Lisätään uusi palvelu Lisää palvelu -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (cmbOffices.SelectedItem != null)
            {

                if (txtPriceD.Text == string.Empty)
                {
                    MessageBox.Show("Syötä palvelun hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (txtPriceD.Text == "0")
                {
                    var result = MessageBox.Show("Haluatko varmasti asettaa palvelun hinnaksi 0€?", "Palvelun hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                var newService = new Service
                {

                    ServiceName = txtName.Text,
                    ServiceDescription = txtDescription.Text,
                    //ServicePrice = Convert.ToDouble(txtPriceBox.Text, CultureInfo.InvariantCulture),
                    //PriceByHour = Convert.ToDouble(txtPriceH.Text, CultureInfo.InvariantCulture),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text, CultureInfo.InvariantCulture),
                    //PriceByWeek = Convert.ToDouble(txtPriceW.Text, CultureInfo.InvariantCulture),
                    //PriceByMonth = Convert.ToDouble(txtPriceM.Text, CultureInfo.InvariantCulture),
                    OfficeID = office.OfficeID
                };

                if (newService.ServiceName == string.Empty)
                {
                    MessageBox.Show("Syötä palvelun nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (newService.ServiceDescription == string.Empty)
                {
                    MessageBox.Show("Syötä palvelun kuvaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                serviceRepo.AddNewService(newService, office);
                MessageBox.Show("Palvelun lisääminen onnistui!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                dgService.ItemsSource = serviceRepo.GetServices();
                ResetAll();
            }

            else
            {
                MessageBox.Show("Palvelun lisäys ei onnistunut!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }


        /// <summary>
        /// Poistetaan palvelu Lista palveluista -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            var service = dgService.SelectedItem as Service;

            ObservableCollection<ReservationObject> reservedServices = new ObservableCollection<ReservationObject>();

            if (dgService.SelectedIndex == -1)
            {
                MessageBox.Show("Valitse poistettava palvelu...", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            reservedServices = reservationRepo.GetReservedServices(service);

            if (reservedServices.Count != 0)
            {
                var result = MessageBox.Show("Laitetta ei voida poistaa. Palvelu on käytössä varauksessa ja/tai aktiivisessa laskussa", "Laitteen poisto.", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    return;
                }
            }

            if (reservedServices.Count == 0)
            {
                var result = MessageBox.Show("Haluatko varmasti poistaa valitun palvelun?", "Laitteen poisto.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    serviceRepo.DeleteService(service);
                    dgService.ItemsSource = serviceRepo.GetServices();
                }
            }
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
        /// Sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Estää muiden kuin numeroiden syöttämisen tekstikenttään, johon on määritelty tämä toiminto (PreviewTextInput)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        //private void PriceChanged(object sender, TextChangedEventArgs e)
        //{

        //    if (priceBox.Text == string.Empty)
        //    {
        //        priceBox.Text = "0";
        //    }
        //}


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
        /// ServiceWindow Shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_KeyDown(object sender, KeyEventArgs e)
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


        private void ResetAll()
        {
            myServiceIdBox.Text = string.Empty;
            nameBox.Text = string.Empty;
            descriptionBox.Text = string.Empty;
            priceDBox.Text = string.Empty;
            cmbIsActive.SelectedIndex = -1;
            myServiceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            txtName.Text = string.Empty;
            txtPriceD.Text = string.Empty;
            txtDescription.Text = string.Empty;
            cmbOffices.SelectedIndex = -1;
        }

        private void myServiceIdBox_MouseEnter(object sender, MouseEventArgs e)
        {
            myServiceIdBox.Focusable= true;
        }
    }
}
