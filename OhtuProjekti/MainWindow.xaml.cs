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
using System.Windows.Navigation;
using System.Windows.Shapes;
using vt_systems.OfficeData;
using vt_systems.Database;

using vt_systems.TestWindows;
using vt_systems.InvoiceData;
using vt_systems.ServiceData;
using vt_systems.ReportData;
using vt_systems.DeleteData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Privaatit muuttujat tietokannan luontia varten
        private DatabaseScripts DBscripts;

        public MainWindow()
        {
            InitializeComponent();

            // Tarkistetaan onko tietokanta olemassa. Jos kantaa ei löydy, alustetaan tietokanta.
            DBscripts = new DatabaseScripts();
            bool DBexists = DBscripts.CheckIfDBExists();

            if (DBexists == false)
            {
                DBscripts.IniateDatabase();
            }            
        }


        /// <summary>
        /// Sulkee ohjelman
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Toimitilat Buttonia, avautuu WorkspaceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceWindow_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceWindow workspaceWindow = new WorkspaceWindow();
            workspaceWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Asiakkaat Buttonia, avautuu CustomerWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Toimipisteet Buttonia, avautuu OfficeWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeWindow_Click(object sender, RoutedEventArgs e)
        {
            OfficeWindow officeWindow = new OfficeWindow();
            officeWindow.Show();
        }

        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Laitteet Buttonia, avautuu DeviceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWindow_Click(object sender, RoutedEventArgs e)
        {
            DeviceWindow deviceWindow = new DeviceWindow();
            deviceWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Palvelut Buttonia, avautuu ServiceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Varaukset Buttonia, avautuu ExistingReservationWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingReservationWindow_Click(object sender, RoutedEventArgs e)
        {
            ExistingReservationWindow existingReservationWindow = new ExistingReservationWindow();
            existingReservationWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta / MainWindow:ssa Laskutus Buttonia, avautuu InvoiceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Invoice_Click(object sender, RoutedEventArgs e)
        {
            NewInvoiceWindow invoiceWindows = new NewInvoiceWindow();
            invoiceWindows.Show();
        }


        /// <summary>
        /// Painettaessa Menusta Raportointi..., avautuu ReportWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
        }


        /// <summary>
        /// Painettaessa Menusta Admin..., avautuu DeleteDataWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.Show();
        }


        /// <summary>
        /// Menusta Ohje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpWindow_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }


        /// <summary>
        /// MainWindow Pikanäppäimet, joilla saadaan eri ikkunoita auki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Asiakkaat -ikkuna
            if (e.Key == Key.F1)
            {
                CustomerWindow_Click(sender, e);
            }

            // Laitteet -ikkuna
            else if (e.Key == Key.F2)
            {
                DeviceWindow_Click(sender, e);
            }

            // Palvelut -ikkuna
            else if (e.Key == Key.F3)
            {
                ServiceWindow_Click(sender, e);
            }

            // Varaukset -ikkuna
            else if (e.Key == Key.F4)
            {
                ExistingReservationWindow_Click(sender, e);
            }

            // Toimitilat -ikkuna
            else if (e.Key == Key.F5)
            {
                WorkspaceWindow_Click(sender, e);
            }

            // Laskut -ikkuna
            else if (e.Key == Key.F6)
            {
                Invoice_Click(sender, e);
            }

            // Toimipisteet -ikkuna
            else if (e.Key == Key.F7)
            {
                OfficeWindow_Click(sender, e);
            }

            // Raportointi -ikkuna
            else if (e.Key == Key.F8)
            {
                ReportWindow_Click(sender, e);
            }

            // Admin -ikkuna
            else if (e.Key == Key.F9)
            {
                Admin_Click(sender, e);
            }
        }


        /// <summary>
        /// Mahdollistaa ikkunan liikuttelun
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
        /// Avaa Laskutuksen -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintInvoice(object sender, RoutedEventArgs e)
        {
            PrintWindow printWindow = new PrintWindow();
            printWindow.Show();
        }

        /// <summary>
        /// Avaa Raportoinnin -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportRepo_Click(object sender, RoutedEventArgs e)
        {
            ReportTestWindow reportTestWindow = new ReportTestWindow();
            reportTestWindow.Show();
        }

        /// <summary>
        /// Avaa Asiakkaat -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerRepo_Click(object sender, RoutedEventArgs e)
        {
            CustomerTestWindow customerTestWindow = new CustomerTestWindow();
            customerTestWindow.Show();
        }

        /// <summary>
        /// Avaa Toimipiste -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeRepo_Click(object sender, RoutedEventArgs e)
        {
            OfficeTestWindow officeTestWindow = new OfficeTestWindow();
            officeTestWindow.Show();
        }

        /// <summary>
        /// Avaa Toimitila -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceRepo_Click(object sender, RoutedEventArgs e)
        {
            WorkspaceTestWindow workspaceTestWindow = new WorkspaceTestWindow();
            workspaceTestWindow.Show();
        }

        /// <summary>
        /// Avaa Varaukset -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationRepo_Click(object sender, RoutedEventArgs e)
        {
            ReservationTestWindow reservationTestWindow = new ReservationTestWindow();
            reservationTestWindow.Show();
        }

        /// <summary>
        /// Avaa Laitteet -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceRepo_Click(object sender, RoutedEventArgs e)
        {
            DeviceTestWindow deviceTestWindow = new DeviceTestWindow();
            deviceTestWindow.Show();
        }


        /// <summary>
        /// Avaa Palvelut -testi-ikkunan
        /// Tänne vievä menuitem on laitettu kommentteihin, koska ei ole tarkoituksenmukaista käyttäjien päästä tänne
        /// Mutta testi-ikkunat säilytetään mahdollista jatkokehitystä varten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceRepo_Click(object sender, RoutedEventArgs e)
        {
            ServiceTestWindow serviceTestWindow = new ServiceTestWindow();
            serviceTestWindow.Show();
        }
    }
}
