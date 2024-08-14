using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using vt_systems.ReservationData;

namespace vt_systems.InvoiceData
{
    /// <summary>
    /// Interaction logic for Print.xaml
    /// </summary>
    public partial class Print : Window
    {
        private Invoice invoice = new Invoice();

        public Print(Invoice invoice)
        {
            InitializeComponent();

            this.invoice = invoice;
            this.invoice.ImportData();
            GetInvoiceRows();
            this.DataContext = this.invoice;

            // Kalenterissa oletuksena eräpäivä
            due_datepicker.SelectedDate = invoice.DueDate;
        }


        /// <summary>
        /// Avaa tulostusikkunan, josta voi valita joko paperille tulostamisen jos tulostin löytyy, tai sitten tallentaa lasku PDF-muodossa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintInvoice(object sender, RoutedEventArgs e)
        {
            additional_changes.Visibility = Visibility.Hidden;

            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                printDlg.PrintVisual(gr_invoice, "Lasku");
                this.Close();
            }

            additional_changes.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Mahdollistaa ikkunan liikuttelun.
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


        /// <summary>
        /// Hakee laskulle laskurivit.
        /// </summary>
        private void GetInvoiceRows()
        {
            ObservableCollection<ReservationObject> objects = this.invoice.Reservation.ReservedObjects;

            this.invoice.ServiceRows.Clear();
            this.invoice.DeviceRows.Clear();
            this.invoice.WorkspaceRows.Clear();
            this.invoice.TotalPrice = 0;

            foreach (ReservationObject obj in objects)
            {
                if (obj.Service != null)
                {
                    var item = new InvoiceRow();

                    item.Name = obj.Service.ServiceName;
                    item.Time = (this.invoice.Reservation.ReleaseDate - this.invoice.Reservation.ReservationDate).TotalDays + 1;
                    item.Qty = obj.Service.Quantity;
                    item.UnitPrice = obj.Service.PriceByDay;
                    item.WholePrice = item.Time * item.UnitPrice * item.Qty;

                    this.invoice.ServiceRows.Add(item);
                    service_list.ItemsSource = this.invoice.ServiceRows;

                    this.invoice.RowTotal = this.invoice.RowTotal + item.WholePrice;
                    this.invoice.TotalPrice = this.invoice.RowTotal;
                }

                if (obj.Device != null)
                {
                    var item = new InvoiceRow();

                    item.Name = obj.Device.Name;
                    item.Time = (this.invoice.Reservation.ReleaseDate - this.invoice.Reservation.ReservationDate).TotalDays + 1;
                    item.Qty = obj.Device.Quantity;
                    item.UnitPrice = obj.Device.PriceByDay;
                    item.WholePrice = item.Time * item.UnitPrice * item.Qty;

                    this.invoice.DeviceRows.Add(item);
                    device_list.ItemsSource = this.invoice.DeviceRows;


                    this.invoice.RowTotal = this.invoice.RowTotal + item.WholePrice;
                    this.invoice.TotalPrice = this.invoice.RowTotal;
                }

                if (obj.Workspace != null)
                {
                    var item = new InvoiceRow();

                    item.Name = obj.Workspace.WSName;
                    item.Time = (this.invoice.Reservation.ReleaseDate - this.invoice.Reservation.ReservationDate).TotalDays + 1;
                    item.UnitPrice = obj.Workspace.PriceByDay;
                    item.WholePrice = item.Time * item.UnitPrice;

                    this.invoice.WorkspaceRows.Add(item);
                    workspace_list.ItemsSource = this.invoice.WorkspaceRows;


                    this.invoice.RowTotal = this.invoice.RowTotal + item.WholePrice;
                    this.invoice.TotalPrice = this.invoice.RowTotal;
                }

            }

            this.invoice.TotalPrice += this.invoice.PriceAddition;
        }


        /// <summary>
        /// Loppusummaan lisääminen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToTotal(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(change_ammount.Text, out double ammount))
            {
                this.invoice.PriceAddition += ammount;
                this.invoice.TotalPrice += ammount;
            }
            else
            {
                MessageBox.Show("Tarkista syöte.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Loppusummasta vähentäminen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecreaseFromTotal(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(change_ammount.Text, out double ammount))
            {
                this.invoice.PriceAddition -= ammount;
                this.invoice.TotalPrice -= ammount;
            }
            else
            {
                MessageBox.Show("Tarkista syöte.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Painike tapahtuman kuuntelija, joka vaihtaa laskun eräpäivän.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDueDate(object sender, RoutedEventArgs e)
        {
            if (due_datepicker.SelectedDate != null)
            {
                this.invoice.DueDate = due_datepicker.SelectedDate.Value;
            }
        }


        /// <summary>
        /// Laskun sulkeminen Sulje Buttonista. Ennen sulkemista varmistaa tallennetaanko muutoksen. Päivittää olemassaolevan laskun tai lisää uuden laskun.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseInvoiceWin(object sender, RoutedEventArgs e)
        {
            var ask = MessageBox.Show("Tallennetaanko muutokset?", string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (ask != MessageBoxResult.No)
            {
                var invoiceRepo = new InvoiceRepo();
                var invoices = invoiceRepo.GetAllInvoices();
                bool exists = false;

                foreach (var invoice in invoices)
                {
                    if (invoice.ReservationID == this.invoice.ReservationID)
                    {
                        exists = true;
                        this.invoice.ID = invoice.ID;
                        break;
                    }
                }

                if (exists)
                {
                    invoiceRepo.UpdateInvoice(this.invoice);
                    DialogResult = true;

                }
                else
                {
                    invoiceRepo.AddNewInvoice(this.invoice);
                    DialogResult = true;

                }
            }
            else
            {
                this.Close();
            }
        }
    }
}
