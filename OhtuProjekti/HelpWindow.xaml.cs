using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
using vt_systems.CustomerData;
using static System.Net.Mime.MediaTypeNames;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();

            // Pikanappäinten ohjeen tekstejä
            shortcut.Text = " F1 = Asiakkaat" + "\t\t" + " F2 = Laitteet" + "\t\t" + " F3 = Palvelut" + "\r\n" + " F4 = Varaukset" + "\t\t" + " F5 = Toimitilat" + "\t\t" + " F6 = Laskutus" + "\r\n" + " F7 = Toimipisteet" + "\t\t" + " F8 = Raportointi" + "\t\t" + " F9 = Admin";
            shortcut2.Text = " Alt+T = Tiedosto -> (Alt+O = Toiminnot,  Alt+F4 = Sulje)" + "\r\n" + " Alt + I = Tietoja" + "\r\n" + " Alt + O = Ohje";

            // Menun ohjeen tekstejä
            menu.Text = " Toiminnot: Löydät kaikki avattavat sivut, paitsi se sivu, jolla juuri olet. (Asiakkaat, Laitteet, Palvelut, Varaukset, Toimitilat, Laskutus, Toimipisteet, Raportointi, Admin)."
                + "\r\n\r\n" + " Sulje: Voit sulkea ikkunan.";

            // Varauksen ohjeen tekstejä
            editReservation.Text = "Muokkaa tietoja -välilehdellä voit hakea varausnumerolla varauksen ja muokata sen tietoja. Täytä myös Muokkaa laitteta ja Muokkaa palveluita -välilehtien tiedot, jotta pääset tallentamaan muutokset." + "\r\n\r\n"
                + "Muokkaa laitteita -välilehdellä näet varatut laitteet listattuna. Täällä voit myös halutessasi poistaa tai lisätä laitteen varaukselle. Täytä myös Muokkaa palveluita -välilehtien tiedot, jotta pääset tallentamaan muutokset." + "\r\n\r\n"
                + "Muokkaa palveluita -välilehdellä näet varatut palvelut listattuna. Täällä voit myös halutessasi poistaa tai lisätä palvelun varaukselle. Tallenna lopuksi muutokset.";

            // Uuden varauksen ohjeen tekstejä
            newReservation.Text = "Lisää laitteet -välilehdellä voit lisätä laitteen varaukselle. Täytä myös Lisää palvelut -välilehtien tiedot, jotta pääset tallentamaan vatauksen." + "\r\n\r\n"
                + "Lisää palvelut -välilehdellä voit lisätä palvelun varaukselle. Tallenna lopuksi varaus.";
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
        /// Painettaessa Ohjeissa Pikanäppäimet Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShortcutInfo_Click(object sender, RoutedEventArgs e)
        {
            shortcutInfo.Visibility = Visibility.Visible;
            btnShortcut.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Menu Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuInfo_Click(object sender, RoutedEventArgs e)
        {
            menuInfo.Visibility = Visibility.Visible;
            btnMenu.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Asiakkaat Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerInfo_Click(object sender, RoutedEventArgs e)
        {
            customerInfo.Visibility = Visibility.Visible;
            btnCustomers.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Laitteet Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceInfo_Click(object sender, RoutedEventArgs e)
        {
            deviceInfo.Visibility = Visibility.Visible;
            btnDevices.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Palvelut Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceInfo_Click(object sender, RoutedEventArgs e)
        {
            serviceInfo.Visibility = Visibility.Visible;
            btnServices.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Varaukset Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationInfo_Click(object sender, RoutedEventArgs e)
        {
            reservationInfo.Visibility = Visibility.Visible;
            btnReservations.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Uusi varaus Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewReservationInfo_Click(object sender, RoutedEventArgs e)
        {
            newReservationInfo.Visibility = Visibility.Visible;
            btnNewReservation.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Toimitilat Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceInfo_Click(object sender, RoutedEventArgs e)
        {
            workspaceInfo.Visibility = Visibility.Visible;
            btnWorkspaces.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Laskut Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceInfo_Click(object sender, RoutedEventArgs e)
        {
            invoiceInfo.Visibility = Visibility.Visible;
            btnInvoices.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Toimipisteet Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeInfo_Click(object sender, RoutedEventArgs e)
        {
            officeInfo.Visibility = Visibility.Visible;
            btnOffices.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Raportti Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportInfo_Click(object sender, RoutedEventArgs e)
        {
            reportInfo.Visibility = Visibility.Visible;
            btnReports.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Admin Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdminInfo_Click(object sender, RoutedEventArgs e)
        {
            adminInfo.Visibility = Visibility.Visible;
            btnAdmin.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            reportInfo.Visibility = Visibility.Hidden;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            closeInfo.Visibility = Visibility.Hidden;
            btnClose.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Painettaessa Ohjeissa Sulkeminen Buttonia, näytetään kyseiset ohjeet ja muut piilotetaan. Valittu nappi vaaleampi kuin muut.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseInfo_Click(object sender, RoutedEventArgs e)
        {
            closeInfo.Visibility = Visibility.Visible;
            btnClose.Background = new SolidColorBrush(Color.FromRgb(255, 224, 178));

            shortcutInfo.Visibility = Visibility.Hidden;
            btnShortcut.Background = Brushes.Orange;
            menuInfo.Visibility = Visibility.Hidden;
            btnMenu.Background = Brushes.Orange;
            customerInfo.Visibility = Visibility.Hidden;
            btnCustomers.Background = Brushes.Orange;
            deviceInfo.Visibility = Visibility.Hidden;
            btnDevices.Background = Brushes.Orange;
            serviceInfo.Visibility = Visibility.Hidden;
            btnServices.Background = Brushes.Orange;
            reservationInfo.Visibility = Visibility.Hidden;
            btnReservations.Background = Brushes.Orange;
            newReservationInfo.Visibility = Visibility.Hidden;
            btnNewReservation.Background = Brushes.Orange;
            workspaceInfo.Visibility = Visibility.Hidden;
            btnWorkspaces.Background = Brushes.Orange;
            invoiceInfo.Visibility = Visibility.Hidden;
            btnInvoices.Background = Brushes.Orange;
            officeInfo.Visibility = Visibility.Hidden;
            btnOffices.Background = Brushes.Orange;
            reportInfo.Visibility = Visibility.Hidden;
            btnReports.Background = Brushes.Orange;
            adminInfo.Visibility = Visibility.Hidden;
            btnAdmin.Background = Brushes.Orange;
            ohjeCanvas.Visibility = Visibility.Hidden;
        }


        //Mahdollistaa ikkunan liikuttelun
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
