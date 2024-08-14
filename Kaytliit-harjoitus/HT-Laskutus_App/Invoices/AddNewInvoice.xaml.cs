using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
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

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for AddNewInvoice.xaml
    /// </summary>
    public partial class AddNewInvoice : Window
    {
        InvoiceAppRepo repo;
        private ObservableCollection<Product> productsInOrder = new ObservableCollection<Product>();
        private ObservableCollection<Product> savePointWarehouse = new ObservableCollection<Product>();
        private Invoice invoice = new Invoice();


        public AddNewInvoice()
        {
            InitializeComponent();

            repo = new InvoiceAppRepo();
            this.DataContext = invoice;
            comMyProducts.ItemsSource = repo.GetProducts();
            savePointWarehouse = repo.GetProducts();
            myProductGrid.ItemsSource = productsInOrder;


        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int quantity;

            // Tarkistetaan syöte
            if (int.TryParse(txtBoxQuantity.Text, out quantity))
            {
                if(quantity <= 0)
                {
                    string errorMessage = "Määrä täytyy olla enemmän kuin " + txtBoxQuantity.Text;
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }

                if (comMyProducts.SelectedItem == null)
                {
                    string errorMessage = "Valitse tuote";
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }



                var selectedProduct = (Product)comMyProducts.SelectedItem;
                selectedProduct.Amount = quantity;

                if (!repo.GetWarehouseQuantity(selectedProduct))
                {
                    string errorMessage = "Tuotetta ei ole tarpeeksi varastossa";
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }

                var productName = ((Product)comMyProducts.SelectedItem).Name;
                var productNumber = ((Product)comMyProducts.SelectedItem).Number;
                var productPrice = ((Product)comMyProducts.SelectedItem).Price;
                var productUnit = ((Product)comMyProducts.SelectedItem).Unit;
                var productQuantity = Convert.ToInt32(txtBoxQuantity.Text);

                invoice.InvoiceTotal += productPrice * productQuantity;

                // Tarkistetaan onko tuote jo valmiina listalla -> jos on, niin lisätään siihen
                foreach (var item in productsInOrder)
                {
                    if (item.Name == productName)
                    {
                        item.Amount += productQuantity;
                        item.PriceTotal = item.Price * item.Amount;
                        comMyProducts.SelectedItem = null;
                        txtBoxQuantity.Text = string.Empty;

                        var removeProduct = new Product
                        {
                            Name = productName,
                            Amount = productQuantity,
                            Number = productNumber,
                        };
                   
                        repo.RemoveFromWarehouse(removeProduct);
                        return;
                    }

                }

                var newProduct = new Product
                {
                    Name = productName,
                    Price = productPrice,
                    Number = productNumber,
                    Amount = productQuantity,
                    Unit = productUnit,
                    PriceTotal = productPrice * productQuantity
                };

                productsInOrder.Add(newProduct);
                repo.RemoveFromWarehouse(newProduct);
                comMyProducts.SelectedItem = null;
                txtBoxQuantity.Text = string.Empty;
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in savePointWarehouse)
            {
                repo.SavePoint(item);
            }

            Close();
        }

        private void CreateInvoice_Click(object sender, RoutedEventArgs e)
        {

            if (txtFirstName.Text == string.Empty || txtLastName.Text == string.Empty || txtPhone.Text == string.Empty || txtHomeAddress.Text == string.Empty || myProductGrid.Items.Count > 0 && myProductGrid.Items.Count < 1)
            {
                string errorMessage = "Täytä tarvittavat tiedot ja lisää vähintään yksi tuote laskulle";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }

            Invoice invoice = new Invoice();
            Customer customer = new Customer();

            customer.FirstName = txtFirstName.Text;
            customer.LastName = txtLastName.Text;
            customer.Phone = txtPhone.Text;
            customer.Address = txtHomeAddress.Text;
            customer.Postal = txtPostal.Text;
            customer.City = txtCity.Text;
            invoice.Notes = txtNotes.Text;


            // Lisätään "ostoskorista" laskuriveille tuotteet
            foreach (var item in productsInOrder)
            {
                invoice.InvoiceRow.Add(new Product
                {
                    Name = item.Name,
                    Amount = item.Amount,
                    Number = item.Number,
                });

            }

            int baseNumber = repo.RefBasePartGenerator();
            invoice.ReferenceNumber = repo.RefNumberGenerator(baseNumber);
            invoice.Customer = customer;
            repo.CreateInvoiceNewCustomer(invoice, customer);
            string successMessage = "Laskun luonti onnistui";
            Done successWindow = new Done(successMessage);
            successWindow.ShowDialog();
            this.DialogResult = true;

        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Rectangle;
            var row = button?.DataContext as Product;
            if (row != null)
            {
                invoice.InvoiceTotal -= row.PriceTotal;
                productsInOrder.Remove(row);
                repo.AddToWarehouse(row);

            }
        }

        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = Brushes.LightPink;
            }
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = sender as Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = Brushes.DarkRed;
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
            foreach (var item in savePointWarehouse)
            {
                repo.SavePoint(item);
            }
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
