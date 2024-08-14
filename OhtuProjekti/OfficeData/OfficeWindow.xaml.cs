using System;
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
using System.Xml.Linq;
using vt_systems.DeleteData;
using vt_systems.DeviceData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for OfficeWindow.xaml
    /// </summary>
    public partial class OfficeWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private WorkSpaceRepo workSpaceRepo = new WorkSpaceRepo();
        private DeviceRepo deviceRepo = new DeviceRepo();
        private ServiceRepo serviceRepo = new ServiceRepo();
        private ObservableCollection<Workspace> workspaces = new ObservableCollection<Workspace>();
        private ObservableCollection<Device> devices = new ObservableCollection<Device>();
        private ObservableCollection<Service> services = new ObservableCollection<Service>();
        public OfficeWindow()
        {
            InitializeComponent();

            myOfficeIdBox.IsEnabled = true;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            // Luodaan uusi toimipiste
            this.DataContext = new Office();

            // Haetaan toimipisteet DataGridiin Lista toimipisteistä -välilehdellä
            dgOffices.ItemsSource = officeRepo.GetOffices();

            //lisaaBtn.Style = this.FindResource("ButtonColorChange") as Style;
        }


        /// <summary>
        /// Haetaan toimipiste tiedot Toimipiste ID:n perusteella Muokkaa toimipistetietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetOffice_Click(object sender, RoutedEventArgs e)
        {
            var office = new Office();
            int officeID;

            if (int.TryParse(myOfficeIdBox.Text, out officeID))
            {
                office = this.DataContext as Office;
                if (officeRepo.GetOffice(office) != null)
                {
                    myOfficeIdBox.IsEnabled = false;
                    addressBox.IsEnabled = true;
                    postalBox.IsEnabled = true;
                    cityBox.IsEnabled = true;
                    phoneBox.IsEnabled = true;
                    emailBox.IsEnabled = true;
                    cmbIsActive.IsEnabled = true;

                    addressBox.Text = office.StreetAddress;
                    postalBox.Text = office.PostalCode;
                    cityBox.Text = office.City;
                    phoneBox.Text = office.Phone;
                    emailBox.Text = office.Email;

                    if (office.IsInActive != true)
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
                    myOfficeIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimipiste id", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Päivitetään toimipiste Muokkaa toimipistetietoja -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateOffice_Click(object sender, RoutedEventArgs e)
        {
            var updateOffice = this.DataContext as Office;
            updateOffice.StreetAddress = addressBox.Text;
            updateOffice.PostalCode = postalBox.Text;
            updateOffice.City = cityBox.Text;
            updateOffice.Phone = phoneBox.Text;
            updateOffice.Email = emailBox.Text;

            if (cmbIsActive.SelectedIndex == 0)
            {
                updateOffice.IsInActive = false;
                workspaces = workSpaceRepo.GetInactiveWorkspaces(updateOffice);
                devices = deviceRepo.GetInactiveDevices(updateOffice);
                services = serviceRepo.GetInactiveServices(updateOffice);
                foreach (var item in workspaces)
                {
                    item.IsInActive = false;
                    workSpaceRepo.UpdateWorkspace(item);
                }
                foreach (var item in devices)
                {
                    item.IsInActive = false;
                    deviceRepo.UpdateDevice(item);
                }
                foreach (var item in services)
                {
                    item.IsInActive = false;
                    serviceRepo.UpdateService(item);
                }
            }

            else
            {
                updateOffice.IsInActive = true;

                workspaces = workSpaceRepo.GetWorkspaces(updateOffice);
                devices = deviceRepo.GetDevices(updateOffice);
                services = serviceRepo.GetServices(updateOffice);
                foreach (var item in workspaces)
                {
                    item.IsInActive = true;
                    workSpaceRepo.UpdateWorkspace(item);
                }
                foreach (var item in devices)
                {
                    item.IsInActive = true;
                    deviceRepo.UpdateDevice(item);
                }
                foreach (var item in services)
                {
                    item.IsInActive = true;
                    serviceRepo.UpdateService(item);
                }
            }

            if (updateOffice.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateOffice.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateOffice.City == string.Empty)
            {
                MessageBox.Show("Syötä Postitoimipaikka!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateOffice.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateOffice.Email == string.Empty)
            {
                MessageBox.Show("Lisää sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (!updateOffice.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            officeRepo.UpdateOffice(updateOffice);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgOffices.ItemsSource = officeRepo.GetOffices();
            ResetAll();
        }


        /// <summary>
        /// Lisätään toimipiste Lisää toimipiste -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewOffice_Click(object sender, RoutedEventArgs e)
        {
            var newOffice = new Office
            {
                StreetAddress = txtAddress.Text,
                City = txtCity.Text,
                PostalCode = txtPostal.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
            };

            if (newOffice.StreetAddress == string.Empty)
            {
                MessageBox.Show("Syötä katuosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newOffice.PostalCode == string.Empty)
            {
                MessageBox.Show("Syötä postinumero!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newOffice.City == string.Empty)
            {
                MessageBox.Show("Syötä Postitoimipaikka!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newOffice.Phone == string.Empty)
            {
                MessageBox.Show("Syötä puhelinnumero", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (newOffice.Email == string.Empty)
            {
                MessageBox.Show("Lisää sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!newOffice.Email.Contains("@"))
            {
                MessageBox.Show("Syötä kelvollinen sähköpostiosoite", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            officeRepo.AddNewOffice(newOffice);
            MessageBox.Show("Toimipisteen lisääminen onnistui!");
            dgOffices.ItemsSource = officeRepo.GetOffices();
            ResetAll();
        }


        /// <summary>
        /// Poistetaan toimipiste Lista toimipisteistä -välilehdellä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteOffice_Click(object sender, RoutedEventArgs e)
        {
            //var deleteOffice = this.DataContext as Office;
            //int officeID;
            //if (int.TryParse(deleteOfficeIdBox.Text, out officeID))
            //{
            //    deleteOffice.OfficeID = officeID;

            //    bool zeroDevices = officeRepo.GetOfficeDevicesAmount(deleteOffice);
            //    bool zeroServices = officeRepo.GetOfficeServicesAmount(deleteOffice);
            //    bool zeroReservations = officeRepo.GetOfficeReservationsAmount(deleteOffice);
            //    bool zeroWorkspace = officeRepo.GetOfficeWorkspaceAmount(deleteOffice);

            //    if (zeroDevices == true && zeroServices == true && zeroReservations == true && zeroWorkspace == true)
            //    {
            //        officeRepo.DeleteOffice(deleteOffice);
            //        dgOffices.ItemsSource = officeRepo.GetOffices();
            //        return;
            //    }

            //    MessageBox.Show("Toimitilaa ei voida poistaa. Poista ensin kaikki toimitilat, varaukset, laitteet ja palvelut.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);


            //}
            //else
            //{
            //    MessageBox.Show("ÄÄÄÄÄ!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Painettaessa Menusta Asiakkaat... , avautuu CustomerWindow.
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
        /// Painettaessa Menusta Admin..., avautuu DeleteDataWindow.
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
        /// OfficeWindow Shortcuts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeWindow_KeyDown(object sender, KeyEventArgs e)
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

        // Käyttöliittymän tekstikenttien resetointi
        private void ResetAll()
        {
            myOfficeIdBox.IsEnabled = true;
            addressBox.IsEnabled = false;
            postalBox.IsEnabled = false;
            cityBox.IsEnabled = false;
            phoneBox.IsEnabled = false;
            emailBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            myOfficeIdBox.Text = string.Empty;
            addressBox.Text = string.Empty;
            postalBox.Text = string.Empty;
            cityBox.Text = string.Empty;
            phoneBox.Text = string.Empty;
            emailBox.Text = string.Empty;
            cmbIsActive.SelectedIndex = -1;

            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtPostal.Text = string.Empty;
        }


        //Mahdollistaa ikkunan liikuttelun.
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        /// <summary>
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi. Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.*/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myOfficeIdBox_MouseEnter(object sender, MouseEventArgs e)
        {
            myOfficeIdBox.Focusable = true;
        }
    }
}
