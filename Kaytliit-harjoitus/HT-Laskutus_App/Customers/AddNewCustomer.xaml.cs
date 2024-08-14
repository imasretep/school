using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for AddNewCustomer.xaml
    /// </summary>
    public partial class AddNewCustomer : Window
    {
        public AddNewCustomer()
        {
            InitializeComponent();
            this.DataContext = new Customer();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new Customer
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Phone = txtPhone.Text,
                Address = txtHomeAddress.Text,
                Postal = txtPostal.Text,
                City = txtCity.Text,
            };

            if (newCustomer.FirstName == string.Empty || newCustomer.LastName == string.Empty || newCustomer.Phone == string.Empty || newCustomer.Address == string.Empty || newCustomer.Postal == string.Empty || newCustomer.City == string.Empty)
            {

                string errorMessage = "Täytä tarvittavat tiedot"; 
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }


            var repo = new InvoiceAppRepo();
            repo.AddNewCustomer(newCustomer);
            string successMessage = "Asiakkaan lisääminen onnistui";
            Done successWindow = new Done(successMessage);
            successWindow.ShowDialog();
            this.DialogResult = true;
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
