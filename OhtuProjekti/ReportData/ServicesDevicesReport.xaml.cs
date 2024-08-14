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
using vt_systems.DeviceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for ServicesDevicesReport.xaml
    /// </summary>
    public partial class ServicesDevicesReport : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ReportRepo reportRepo = new ReportRepo();
        private ObservableCollection<Report> reportsService = new ObservableCollection<Report>();
        private ObservableCollection<Report> reportsDevice = new ObservableCollection<Report>();
        public ServicesDevicesReport(Office office, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            string report = office.City + "  " + startDate.ToString("dd.MM.yyyy") + " - " + endDate.ToString("dd.MM.yyyy");
            txtOffice.Text = report;
            double sum = 0;
            reportsService = reportRepo.GetReportDataServices(office, startDate, endDate);
            reportsDevice  = reportRepo.GetReportDataDevices(office, startDate, endDate);
            dgReportDevice.Visibility = Visibility.Collapsed;
            dgReportService.Visibility = Visibility.Collapsed;


            dgReportService.ItemsSource = reportsService;
            dgReportDevice.ItemsSource = reportsDevice;

            foreach (var item in reportsService)
            {
                sum += item.Sum;
                
            }
            foreach (var item in reportsDevice)
            {
                sum += item.Sum;
            }

            txtSum.Text = sum.ToString() + " €";

            if(reportsService.Count > 0)
            {
                dgReportService.Visibility = Visibility.Visible;
            }
            if(reportsDevice.Count > 0) 
            {
                dgReportDevice.Visibility = Visibility.Visible;
            }
        }


        /// <summary>
        /// Printtaus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(grdReport, "Lasku");
            }
            this.Close();
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
