using HT_Laskutus_App.Invoices;
using MySqlConnector;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InvoiceAppRepo repo;

        public MainWindow()
        {
            InitializeComponent();
            repo = new InvoiceAppRepo();
            repo.CreateInvoiceAppDb();
            repo.CreateProductsTable();
            repo.CreateCustomersTable();
            repo.AddDefaultProducts();
            repo.AddDefaultCustomers();
            repo.CreateInvoicesTable();
            repo.CreateInvoiceRowsTable();
            repo.AddDefaultInvoices();

            myDataGrid.ItemsSource = repo.GetProducts();
            myCustomerGrid.ItemsSource = repo.GetCustomers();

        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {

            AddProduct addProductWindow = new AddProduct();
            addProductWindow.ShowDialog();
            myDataGrid.ItemsSource = repo.GetProducts();

        }

        private void UpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            UpdateProduct updateProductWindow = new UpdateProduct();
            updateProductWindow.ShowDialog();
            myDataGrid.ItemsSource = repo.GetProducts();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteProduct deleteProductWindow = new DeleteProduct();
            deleteProductWindow.ShowDialog();
            myDataGrid.ItemsSource = repo.GetProducts();
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddNewCustomer addNewCustomerWindow = new AddNewCustomer();
            addNewCustomerWindow.ShowDialog();
            myCustomerGrid.ItemsSource = repo.GetCustomers();
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            UpdateCustomer updateCustomerWindow = new UpdateCustomer();
            updateCustomerWindow.ShowDialog();
            myCustomerGrid.ItemsSource = repo.GetCustomers();
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            DeleteCustomer deleteCustomerWindow = new DeleteCustomer();
            deleteCustomerWindow.ShowDialog();
            myCustomerGrid.ItemsSource = repo.GetCustomers();
        }

        private void AllInvoicesButton_Click(object sender, RoutedEventArgs e)
        {
            AllInvoices allInvoicesWindow = new AllInvoices();
            allInvoicesWindow.ShowDialog();
            myDataGrid.ItemsSource = repo.GetProducts();

        }

        private void AddNewInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            SelectNewOrExistingUser selectNewOrExistingUser = new SelectNewOrExistingUser();
            selectNewOrExistingUser.ShowDialog();
            myCustomerGrid.ItemsSource = repo.GetCustomers();
            myDataGrid.ItemsSource = repo.GetProducts();
        }

        private void DeleteInvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteInvoice deleteInvoice = new DeleteInvoice();
            deleteInvoice.ShowDialog();
            myDataGrid.ItemsSource = repo.GetProducts();
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
