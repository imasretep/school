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
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for WorkspaceReport.xaml
    /// </summary>
    public partial class WorkspaceReport : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ReportRepo reportRepo = new ReportRepo();
        private ObservableCollection<Report> reports = new ObservableCollection<Report>();
        public WorkspaceReport(Office office, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();
            string report = office.City + "  " + startDate.ToString("dd.MM.yyyy") + " - " + endDate.ToString("dd.MM.yyyy");
            txtOffice.Text = report;

            double sum = 0;
            reports = reportRepo.GetReportDataWorkspaces(office, startDate, endDate);
            dgReport.ItemsSource = reports;

            foreach (var item in reports)
            {
                sum += item.Sum;
            }
            txtSum.Text = sum.ToString() + " €";
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
