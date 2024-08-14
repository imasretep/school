using System;
using System.Collections;
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
using System.Xml.Linq;

namespace HT_Laskutus_App
{
    /// <summary>
    /// Interaction logic for InvoiceDetailsWindow.xaml
    /// </summary>
    public partial class InvoiceDetailsWindow : Window
    {
        private InvoiceAppRepo repo;
        private Invoice newInvoice = new Invoice();
        private ObservableCollection<Product> productsInOrder = new ObservableCollection<Product>();
        private ObservableCollection<Product> savePointWarehouse = new ObservableCollection<Product>();
        private Invoice removedProductsSavePoint = new Invoice();

        public InvoiceDetailsWindow(Invoice invoice)
        {
            InitializeComponent();
            repo = new InvoiceAppRepo();

            myInvoiceDetailWindow.Title = "Laskunumero: " + invoice.InvoiceID +
                                          " Henkilö: " + invoice.Customer.FirstName +
                                          " " + invoice.Customer.LastName;
            labelWindowHeader.Content = myInvoiceDetailWindow.Title;

            comMyProducts.ItemsSource = repo.GetProducts();
            savePointWarehouse = repo.GetProducts();
            newInvoice = repo.GetInvoiceData(invoice);
            newInvoice.InvoiceID = invoice.InvoiceID;
            removedProductsSavePoint.InvoiceID = invoice.InvoiceID;

            // Tuodaan tuotteita laskuriveiltä productsInOrder observablecollectioniin
            foreach (var item in newInvoice.InvoiceRow)
            {
                productsInOrder.Add(item);

            }

            myInvoicesDataGrid.ItemsSource = productsInOrder;
            this.DataContext = newInvoice;
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


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            foreach (var item in savePointWarehouse)
            {
                repo.SavePoint(item);
            }

            Close();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Invoice invoice = new Invoice();
            Customer customer = new Customer();

            customer = newInvoice.Customer;
            invoice.InvoiceDate = newInvoice.InvoiceDate;
            invoice.DueDate = newInvoice.DueDate;
            invoice.Notes = newInvoice.Notes;
            invoice.CustomerID = newInvoice.CustomerID;
            invoice.InvoiceID = newInvoice.InvoiceID;
            invoice.Customer = customer;

            // Lisätään tuotteet laskuriveille
            foreach (var item in productsInOrder)
            {
                invoice.InvoiceRow.Add(new Product
                {

                    Name = item.Name,
                    Amount = item.Amount,
                    Number = item.Number,

                });

            }

            foreach (var item in removedProductsSavePoint.InvoiceRow)
            {
                repo.RemoveProduct(removedProductsSavePoint, item);
            }



            repo.UpdateInvoice(invoice);
            string successMessage = "Laskun päivitys onnistui";
            Done successWindow = new Done(successMessage);
            successWindow.ShowDialog();
            this.DialogResult = true;
        }

        private void myAddButton_Click(object sender, RoutedEventArgs e)
        {
            int quantity;

            // Tarkistetaan syötteen oikeellisuus
            if (int.TryParse(txtBoxQuantity.Text, out quantity))
            {
                if (comMyProducts.SelectedItem == null)
                {
                    string errorMessage = "Valitse tuote";
                    Error errorWindow = new Error(errorMessage);
                    errorWindow.ShowDialog();
                    return;
                }

                if (quantity <= 0)
                {
                    string errorMessage = "Määrä täytyy olla enemmän kuin " + txtBoxQuantity.Text;
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
                newInvoice.InvoiceTotal += productPrice * productQuantity;


                // Jos tuote on jo olemassa laskulla, lisätään siihen vain lisää
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

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Rectangle;
            var row = button?.DataContext as Product;

            // Poistetaan tuote listalta sekä myös lisätään määrä varastoon takaisin.
            if (row != null)
            {

                newInvoice.InvoiceTotal -= row.PriceTotal;
                productsInOrder.Remove(row);

                removedProductsSavePoint.InvoiceRow.Add(row);
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
    }
}
