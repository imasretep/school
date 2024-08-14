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

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for ReportTestWindow.xaml
    /// </summary>
    public partial class ReportTestWindow : Window
    {
        public ReportTestWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(sender == btnReportWorkspaces)
            {
                ReportWorkspacesTestWindow reportWorkspacesTestWindow = new ReportWorkspacesTestWindow();
                reportWorkspacesTestWindow.Show();
            }
        }
    }
}
