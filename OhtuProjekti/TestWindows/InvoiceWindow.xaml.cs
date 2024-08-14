using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vt_systems.CustomerData;
using vt_systems.DeleteData;
using vt_systems.ReportData;
using vt_systems.ReservationData;
using vt_systems.ServiceData;

namespace vt_systems.InvoiceData
{
    /// <summary>
    /// Interaction logic for InvoiceWindow.xaml
    /// </summary>
    public partial class InvoiceWindow : Window
    {
        //    private Invoice Invoice = new Invoice();

        //    public InvoiceWindow()
        //    {
        //        var repo = new InvoiceRepo();
        //        var customerRepo = new CustomerRepo();
        //        var reservationRepo = new ReservationRepo();

        //        InitializeComponent();
        //        this.DataContext = Invoice;

        //        customers_list.ItemsSource = customerRepo.GetCustomers();
        //        reservations_list.ItemsSource = reservationRepo.GetReservations();
        //        invoices_list.ItemsSource = repo.GetAllInvoices();
        //    }

        //    private void CloseWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        Close();
        //    }

        //    private void HeaderFontChange_MouseUp(object sender, MouseButtonEventArgs e)
        //    {
        //        if (tAdd.IsSelected == true)
        //        {
        //            tbAdd.Foreground = Brushes.Orange;
        //            tbSearch.Foreground = Brushes.Black;
        //            tbAllInvoices.Foreground = Brushes.Black;
        //        }
        //        if (tSearch.IsSelected == true)
        //        {
        //            tbAdd.Foreground = Brushes.Black;
        //            tbSearch.Foreground = Brushes.Orange;
        //            tbAllInvoices.Foreground = Brushes.Black;
        //        }
        //        if (tAllInvoices.IsSelected == true)
        //        {
        //            tbAdd.Foreground = Brushes.Black;
        //            tbSearch.Foreground = Brushes.Black;
        //            tbAllInvoices.Foreground = Brushes.Orange;
        //        }
        //    }

        //    private void UpdateInvoice_Click(object sender, RoutedEventArgs e)
        //    {

        //    }

        //    private void PrintInvoice_Click(object sender, RoutedEventArgs e)
        //    {

        //    }

        //    private void BillingDateChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        DateTime billing = billingdate.SelectedDate.Value;

        //        Invoice.DueDate = billing.AddDays(14);
        //    }

        //    private void SelectedCustomer(object sender, EventArgs e)
        //    {
        //        if (customers_list.SelectedItem != null)
        //        {
        //            var customer = customers_list.SelectedItem as Customer;

        //            Invoice.Customer = customer;
        //            Invoice.CustomerID = customer.Id;
        //        }
        //    }

        //    private void price_addition_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            price_addition.Focusable = false;
        //        }
        //    }

        //    private void price_addition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //    {
        //        price_addition.Focusable = true;
        //    }

        //    private void price_addition_LostFocus(object sender, RoutedEventArgs e)
        //    {

        //        if (double.TryParse(price_addition.Text, out var price))
        //        {
        //            Invoice.TotalPrice = Invoice.ReservationPrice + price;

        //            Invoice.PriceAddition = price;
        //        }

        //        if (int.TryParse(price_addition.Text, out var value))
        //        {
        //            Invoice.TotalPrice = Invoice.ReservationPrice + (double)value;

        //            Invoice.PriceAddition = (double)value;
        //        }

        //        Invoice.PriceAddition = 0;
        //    }

        //    private void ClearForm(object sender, RoutedEventArgs e)
        //    {
        //        var customer = customers_list.SelectedItem as Customer;
        //        var reservation = reservations_list.SelectedItem as Reservation;

        //        if (reservations_list.SelectedItem != null && customers_list.SelectedItem != null)
        //        {
        //            Invoice = new Invoice(customer, reservation);
        //            this.DataContext = Invoice;
        //        }
        //        else if (reservations_list.SelectedItem != null && customers_list.SelectedItem == null)
        //        {
        //            Invoice = new Invoice(reservation);
        //            this.DataContext = Invoice;
        //        }
        //        else if (reservations_list.SelectedItem == null && customers_list.SelectedItem != null)
        //        {
        //            Invoice = new Invoice(customer);
        //            this.DataContext = Invoice;
        //        }
        //        else
        //        {
        //            Invoice = new Invoice();
        //            this.DataContext = Invoice;
        //        }
        //    }

        //    private void GenerateReferenceNum(object sender, RoutedEventArgs e)
        //    {

        //        if (reservations_list.SelectedItem != null && customers_list.SelectedItem != null)
        //        {
        //            string value = string.Empty;

        //            string basepart = Invoice.BillingDate.ToString("yyMM");

        //            Random random = new Random();

        //            int first = random.Next(1, 1000);
        //            int second = random.Next(1, 900);
        //            int third = random.Next(1, 90);

        //            int randNum = first + second + third;

        //            int end = random.Next(1, 100) * (Invoice.Customer.Id + Invoice.Reservation.ReservationId);

        //            value = basepart + randNum.ToString() + end.ToString();

        //            Invoice.ReferenceNum = value;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Valitse ensin asiakas ja varaus.");
        //        }
        //    }

        //    private void ReservationSelected(object sender, RoutedEventArgs e)
        //    {
        //        if (reservations_list.SelectedItem != null)
        //        {
        //            Invoice.ReservationPrice = 0;
        //            var reservationRepo = new ReservationRepo();
        //            var reservation = reservations_list.SelectedItem as Reservation;
        //            var data = reservationRepo.GetReservationWithData(reservation);
        //            Invoice.Reservation = data;

        //            foreach (var item in Invoice.Reservation.ReservedObjects)
        //            {
        //                TimeSpan period = reservation.ReleaseDate - reservation.ReservationDate;
        //                int days = period.Days + 1;

        //                double sum = 0.00;

        //                if (item.Device != null)
        //                {
        //                    sum = sum + (double)item.Device.PriceByDay * (double)item.Device.Quantity;
        //                }

        //                if (item.Service != null)
        //                {
        //                    sum = sum + (double)item.Service.PriceByDay * (double)days;
        //                }

        //                if (item.Workspace != null)
        //                {
        //                    sum = sum + (double)item.Workspace.PriceByDay * (double)days;
        //                }

        //                Invoice.ReservationPrice = Invoice.ReservationPrice + sum;
        //            }

        //            Invoice.TotalPrice = Invoice.ReservationPrice;
        //        }
        //    }

        //    private void SaveNewInvoice(object sender, RoutedEventArgs e)
        //    {
        //        var repo = new InvoiceRepo();

        //        if (Invoice.Customer.Id != 0)
        //        {

        //            if (Invoice.Reservation.ReservationId != 0)
        //            {

        //                if (Invoice.ReferenceNum != string.Empty)
        //                {

        //                    repo.AddNewInvoice(Invoice);

        //                }
        //                else
        //                {

        //                    MessageBox.Show("Laskulla on oltava viite ennen tallennusta.");
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Laskulla on varaus ennen tallennusta.");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Laskulla on oltava asiakas tallennusta.");
        //        }

        //        invoices_list.ItemsSource = repo.GetAllInvoices();
        //    }

        //    private void FetchInvoices(object sender, RoutedEventArgs e)
        //    {
        //        var repo = new InvoiceRepo();

        //        if (fetch_by_invoice.Text != null)
        //        {
        //            int num = 0;

        //            if (int.TryParse(fetch_by_invoice.Text, out num))
        //            {
        //                invoices_list.ItemsSource = repo.GetInvoicesByID(num);
        //            }
        //        }

        //        if(fetch_by_reservation.Text != null)
        //        {
        //            int num = 0;

        //            if (int.TryParse(fetch_by_reservation.Text, out num))
        //            {
        //                invoices_list.ItemsSource = repo.GetInvoicesByReservation(num);
        //            }
        //        }

        //        if(fetch_by_date.SelectedDate != null)
        //        {
        //            string date = fetch_by_date.SelectedDate.Value.ToString("yyyy-MM-dd");

        //            invoices_list.ItemsSource = repo.GetInvoicesByDate(date);
        //        }

        //        fetch_by_invoice.Clear();
        //        fetch_by_reservation.Clear();
        //        fetch_by_date.SelectedDate = null;
        //    }

        //    private void fetch_by_invoice_TextChanged(object sender, TextChangedEventArgs e)
        //    {
        //        if (!int.TryParse(fetch_by_invoice.Text, out int num) && fetch_by_invoice.Text != string.Empty)
        //        {
        //            MessageBox.Show("Syötä tähän kenttään vain kokonaislukuja.");
        //            fetch_by_invoice.Clear();
        //        }
        //    }

        //    private void fetch_by_reservation_TextChanged(object sender, TextChangedEventArgs e)
        //    {
        //        if (!int.TryParse(fetch_by_reservation.Text, out int num) && fetch_by_reservation.Text != string.Empty)
        //        {
        //            MessageBox.Show("Syötä tähän kenttään vain kokonaislukuja.");
        //            fetch_by_reservation.Clear();
        //        }
        //    }

        //    private void ClearFetchForm(object sender, RoutedEventArgs e)
        //    {
        //        fetch_by_invoice.Clear();
        //        fetch_by_reservation.Clear();
        //        fetch_by_date.SelectedDate = null;
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Toimitilat..., avautuu WorkspaceWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void WorkspaceWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        WorkspaceWindow workspaceWindow = new WorkspaceWindow();
        //        workspaceWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Asiakkaat..., avautuu CustomerWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void CustomerWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        CustomerWindow customerWindow = new CustomerWindow();
        //        customerWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Toimipisteet..., avautuu OfficeWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void OfficeWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        OfficeWindow officeWindow = new OfficeWindow();
        //        officeWindow.Show();
        //        Close();
        //    }

        //    /// <summary>
        //    /// Painettaessa Menusta Laitteet..., avautuu DeviceWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void DeviceWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        DeviceWindow deviceWindow = new DeviceWindow();
        //        deviceWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Palvelut..., avautuu ServiceWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        ServiceWindow serviceWindow = new ServiceWindow();
        //        serviceWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Varaukset..., avautuu ExistingReservationWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void ExistingReservationWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        ExistingReservationWindow existingReservationWindow = new ExistingReservationWindow();
        //        existingReservationWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Raportointi..., avautuu RaportWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void ReportWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        ReportWindow reportWindow = new ReportWindow();
        //        reportWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Admin..., avautuu DeleteDataWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void Admin_Click(object sender, RoutedEventArgs e)
        //    {
        //        DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
        //        deleteDataWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// Painettaessa Menusta Ohje, avautuu HelpWindow
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void HelpWindow_Click(object sender, RoutedEventArgs e)
        //    {
        //        HelpWindow helpWindow = new HelpWindow();
        //        helpWindow.Show();
        //        Close();
        //    }


        //    /// <summary>
        //    /// InvoiceWindow Shortcuts
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void InvoiceWindow_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        // Asiakkaat -ikkuna
        //        if (e.Key == Key.F1)
        //        {
        //            CustomerWindow_Click(sender, e);
        //        }

        //        // Laitteet -ikkuna
        //        else if (e.Key == Key.F2)
        //        {
        //            DeviceWindow_Click(sender, e);
        //        }

        //        // Palvelut -ikkuna
        //        else if (e.Key == Key.F3)
        //        {
        //            ServiceWindow_Click(sender, e);
        //        }

        //        // Varaukset -ikkuna
        //        else if (e.Key == Key.F4)
        //        {
        //            ExistingReservationWindow_Click(sender, e);
        //        }

        //        // Toimitilat -ikkuna
        //        else if (e.Key == Key.F5)
        //        {
        //            WorkspaceWindow_Click(sender, e);
        //        }

        //        // Toimipisteet -ikkuna
        //        else if (e.Key == Key.F7)
        //        {
        //            OfficeWindow_Click(sender, e);
        //        }

        //        // Raportointi -ikkuna
        //        else if (e.Key == Key.F8)
        //        {
        //            ReportWindow_Click(sender, e);
        //        }

        //        // Admin -ikkuna
        //        else if (e.Key == Key.F9)
        //        {
        //            Admin_Click(sender, e);
        //        }
        //    }
    }
}
