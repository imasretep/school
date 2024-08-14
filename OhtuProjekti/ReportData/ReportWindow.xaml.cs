using System;
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
using vt_systems.DeleteData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;
using vt_systems.TestWindows;

namespace vt_systems.ReportData
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ReportRepo reportRepo = new ReportRepo();
        private ObservableCollection<Report> reports = new ObservableCollection<Report>();

        public ReportWindow()
        {
            InitializeComponent();

            // Haetaan toimipisteet ComboBoxiin
            cmbOffice.ItemsSource = officeRepo.GetOffices();

            // Kalentereissa oletuksena tämä päivä
            calStart.SelectedDate = DateTime.Today;
            calEnd.SelectedDate = DateTime.Today;
        }


        /// <summary>
        /// Toimitila raportti Button, avaa raportin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWorkspaces_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOffice.SelectedValue == null)
            {
                MessageBox.Show("Valitse toimipiste", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (cmbOffice.SelectedValue != null && calStart.SelectedDate != null && calEnd.SelectedDate != null)
            {
                if (calEnd.SelectedDate < calStart.SelectedDate)
                {
                    MessageBox.Show("Raportin alkamispäivä ei voi olla myöhempi kuin lopetuspäivä", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Office office = cmbOffice.SelectedValue as Office;
                DateTime startDate = calStart.SelectedDate.Value;
                DateTime endDate = calEnd.SelectedDate.Value;
                WorkspaceReport workspaceReport = new WorkspaceReport(office, startDate, endDate);
                workspaceReport.Show();
            }
        }


        /// <summary>
        /// Lisäpalvelut raportti Button, avaa raportin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerDev_Click(object sender, RoutedEventArgs e)
        {
            if(cmbOffice.SelectedValue == null)
            {
                MessageBox.Show("Valitse toimipiste", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (cmbOffice.SelectedValue != null && calStart.SelectedDate != null && calEnd.SelectedDate != null)
            {
                if(calEnd.SelectedDate < calStart.SelectedDate)
                {
                    MessageBox.Show("Raportin alkamispäivä ei voi olla myöhempi kuin lopetuspäivä", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Office office = cmbOffice.SelectedValue as Office;
                DateTime startDate = calStart.SelectedDate.Value;
                DateTime endDate = calEnd.SelectedDate.Value;
                ServicesDevicesReport servicesDevicesReport = new ServicesDevicesReport(office, startDate, endDate);
                servicesDevicesReport.Show();
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
        /// Painettaessa MainWindow:ssa Toimitilat Buttonia, avautuu WorkspaceWindow
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
        /// Painettaessa MainWindow:ssa Toimipisteet Buttonia, avautuu OfficeWindow
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
        /// Painettaessa MainWindow:ssa Laitteet Buttonia, avautuu DeviceWindow
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
        /// Painettaessa MainWindow:ssa Laskutus Buttonia, avautuu InvoiceWindow
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
        /// Painettaessa MainWindow:ssa Palvelut Buttonia, avautuu ServiceWindow
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
        /// Painettaessa MainWindow:ssa Varaukset Buttonia, avautuu ExistingReservationWindow
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
        /// Painettaessa Menusta Ohje, avautuu HelpWindow
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
        /// ReportWindow Shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportWindow_KeyDown(object sender, KeyEventArgs e)
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
    }
}
