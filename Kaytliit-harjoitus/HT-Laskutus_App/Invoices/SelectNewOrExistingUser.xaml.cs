using HT_Laskutus_App.Invoices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for SelectNewOrExistingUser.xaml
    /// </summary>
    public partial class SelectNewOrExistingUser : Window
    {
        public SelectNewOrExistingUser()
        {
            InitializeComponent();
        }

        // Uusi asiakas
        private void NewUserInvoice_Click(object sender, RoutedEventArgs e)
        {
            AddNewInvoice newInvoiceWindow = new AddNewInvoice();
            newInvoiceWindow.ShowDialog();
        }

        // Olemassa oleva asiakas
        private void ExistingUserInvoice_Click(object sender, RoutedEventArgs e)
        {
            
            AddExistingCustomerInvoice addExistingCustomerInvoice = new AddExistingCustomerInvoice();
            addExistingCustomerInvoice.ShowDialog();
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
