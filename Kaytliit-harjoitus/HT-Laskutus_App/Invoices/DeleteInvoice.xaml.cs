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

namespace HT_Laskutus_App.Invoices
{
    /// <summary>
    /// Interaction logic for DeleteInvoice.xaml
    /// </summary>
    public partial class DeleteInvoice : Window
    {
        public DeleteInvoice()
        {
            InitializeComponent();
            this.DataContext = new Invoice();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var deleteInvoice = (Invoice)this.DataContext;
            var repo = new InvoiceAppRepo();
            Invoice productsToWarehouse = repo.GetInvoiceData(deleteInvoice);

            // Tarkistetaan syöte
            if(myInvoiceIdBox.Text == "0")
            {
                string errorMessage = "Anna kelvollinen laskunumero";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                return;
            }

            // Lisätään laskulla olleet tuotteet takaisin varastoon
            foreach (var item in productsToWarehouse.InvoiceRow)
            {
                repo.AddToWarehouse(item);
            }

            int invoiceID;
            
            // Poistetaan lasku
            if (int.TryParse(myInvoiceIdBox.Text, out invoiceID))
            {

                if(repo.DeleteInvoice(deleteInvoice) == true)
                {
                    string successMessage = "Laskunumero: " + deleteInvoice.InvoiceID + " poistaminen onnistui";
                    Done successWindow = new Done(successMessage);
                    successWindow.ShowDialog();
                    this.DialogResult = true;
                }
                else 
                { 
                string errorMessage = "Laskunumerolla " + deleteInvoice.InvoiceID + " ei löytynyt laskua";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
                }
            }
            else
            {
                string errorMessage = "Anna kelvollinen laskunumero";
                Error errorWindow = new Error(errorMessage);
                errorWindow.ShowDialog();
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
