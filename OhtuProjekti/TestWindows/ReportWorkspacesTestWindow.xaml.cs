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
using System.Windows.Shell;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ReservationData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for ReportWorkspacesTestWindow.xaml
    /// </summary>
    public partial class ReportWorkspacesTestWindow : Window
    {
        private OfficeRepo officeRepo = new OfficeRepo();
        private ReservationRepo reservationRepo = new ReservationRepo();
        private ReportRepo reportRepo = new ReportRepo();
        private ObservableCollection<Report> reports = new ObservableCollection<Report>();
        public ReportWorkspacesTestWindow()
        {
            InitializeComponent();
            cmbOffice.ItemsSource = officeRepo.GetOffices();
        }

        private void btnWorkspaces_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOffice.SelectedValue != null && calStart.SelectedDate != null && calEnd.SelectedDate != null )
            {
                Office office = cmbOffice.SelectedValue as Office;
                DateTime startDate = calStart.SelectedDate.Value;
                DateTime endDate = calEnd.SelectedDate.Value;
                WorkspaceReport workspaceReport = new WorkspaceReport(office, startDate, endDate);
                workspaceReport.Show();
            }

        }
        private void btnSerDev_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOffice.SelectedValue != null && calStart.SelectedDate != null && calEnd.SelectedDate != null)
            {
                Office office = cmbOffice.SelectedValue as Office;
                DateTime startDate = calStart.SelectedDate.Value;
                DateTime endDate = calEnd.SelectedDate.Value;
                ServicesDevicesReport servicesDevicesReport = new ServicesDevicesReport(office, startDate, endDate);
                servicesDevicesReport.Show();
            }

        }
    }
}
