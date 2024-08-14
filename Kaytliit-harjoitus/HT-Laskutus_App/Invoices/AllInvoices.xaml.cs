using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for AllInvoices.xaml
    /// </summary>
    public partial class AllInvoices : Window
    {
        private InvoiceAppRepo repo;

        public AllInvoices()
        {
            InitializeComponent();
            repo = new InvoiceAppRepo();

            myInvoicesDataGrid.ItemsSource = repo.GetAllInvoiceID();
        }

        private void myInvoicesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var invoice = myInvoicesDataGrid.SelectedItem as Invoice;

            if (invoice != null)
            {
                // Luo uusi ikkuna laskun tiedoilla
                InvoiceDetailsWindow detailsWindow = new InvoiceDetailsWindow(invoice);
                detailsWindow.ShowDialog();
            }
        }


        // Ikkunan asetukset
        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void rctClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void rctClose_MouseEnter(object sender, MouseEventArgs e)
        {
            rctClose.Fill = Brushes.Red;
        }

        private void rctClose_MouseLeave(object sender, MouseEventArgs e)
        {
            rctClose.Fill = Brushes.DarkRed;
        }

        private void rctMinimize_MouseEnter(object sender, MouseEventArgs e)
        {

            rctMinimize.Fill = Brushes.Gold;
        }

        private void rctMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            rctMinimize.Fill = Brushes.DarkGoldenrod;
        }


        private void rctMinimize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
