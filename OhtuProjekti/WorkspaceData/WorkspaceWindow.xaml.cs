using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using vt_systems.DeleteData;
using vt_systems.InvoiceData;
using vt_systems.OfficeData;
using vt_systems.ReportData;
using vt_systems.ServiceData;
using vt_systems.WorkspaceData;

namespace vt_systems
{
    /// <summary>
    /// Interaction logic for WorkspaceWindow.xaml
    /// </summary>
    public partial class WorkspaceWindow : Window
    {
        private WorkSpaceRepo workspaceRepo = new WorkSpaceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();

        public WorkspaceWindow()
        {
            InitializeComponent();

            myWorkspaceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            //priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            //priceWBox.IsEnabled = false;
            //priceMBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;

            // Luodaan uusi toimitila
            this.DataContext = new Workspace();

            // Haetaan toimitilat DataGridiin Lista toimitiloista -välilehdellä
            dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();

            // Valitaan toimipiste ComboBoxiin Lisää toimitila -välilehdellä
            cmbOffices.ItemsSource = officeRepo.GetOffices();
        }


        /// <summary>
        /// Muokkaa tietoja -välilehdellä Hae Button, hakee toimitilan ID:n mukaan toimitilan tiedot. Huomauttaa jos ID:t ei löydy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var workspace = new Workspace();
            int workspaceID;

            if (int.TryParse(myWorkspaceIdBox.Text, out workspaceID))
            {
                workspace = this.DataContext as Workspace;
                if (workspaceRepo.GetWorkspace(workspace) != null)
                {
                    myWorkspaceIdBox.IsEnabled = false;
                    nameBox.IsEnabled = true;
                    descriptionBox.IsEnabled = true;
                    //priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    //priceWBox.IsEnabled = true;
                    //priceMBox.IsEnabled = true;
                    cmbIsActive.IsEnabled = true;

                    nameBox.Text = workspace.WSName;
                    descriptionBox.Text = workspace.WSDescription;
                    //priceHBox.Text = workspace.PriceByHour.ToString();
                    priceDBox.Text = workspace.PriceByDay.ToString();
                    //priceWBox.Text = workspace.PriceByWeek.ToString();
                    //priceMBox.Text = workspace.PriceByMonth.ToString();

                    if (workspace.IsInActive != true)
                    {
                        cmbIsActive.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbIsActive.SelectedIndex = 1;
                    }
                }
                else
                {
                    myWorkspaceIdBox.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimitila id", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        /// <summary>
        /// Tallennetaan haetun toimitilan tiedot Muokkaa toimitilatietoja -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var updateWorkspace = this.DataContext as Workspace;
            updateWorkspace.WSName = nameBox.Text;
            updateWorkspace.WSDescription = descriptionBox.Text;
            //updateWorkspace.PriceByHour = Convert.ToDouble(priceHBox.Text, CultureInfo.InvariantCulture);
            if (priceDBox.Text == string.Empty)
            {
                MessageBox.Show("Syötä toimitilalle hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
                //priceDBox.Text = "0";
            }

            if (priceDBox.Text == "0")
            {
                var result = MessageBox.Show("Haluatko varmasti asettaa toimitilan hinnaksi 0€?", "Toimitilan hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            updateWorkspace.PriceByDay = Convert.ToDouble(priceDBox.Text, CultureInfo.InvariantCulture);
            //updateWorkspace.PriceByWeek = Convert.ToDouble(priceWBox.Text, CultureInfo.InvariantCulture);
            //updateWorkspace.PriceByMonth = Convert.ToDouble(priceMBox.Text, CultureInfo.InvariantCulture);

            if (cmbIsActive.SelectedIndex == 0)
            {
                updateWorkspace.IsInActive = false;
            }
            else
            {
                updateWorkspace.IsInActive = true;
            }

            if (updateWorkspace.WSName == string.Empty)
            {
                MessageBox.Show("Syötä toimitilan nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (updateWorkspace.WSDescription == string.Empty)
            {
                MessageBox.Show("Syötä toimitilan kuvaus!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            workspaceRepo.UpdateWorkspace(updateWorkspace);
            MessageBox.Show("Tietojen päivitys onnistui!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
            ResetAll();
        }


        /// <summary>
        /// Tallennetaan uusi toimitila Lisää toimitila -välilehdellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (cmbOffices.SelectedItem != null)
            {
                if (txtPriceD.Text == string.Empty)
                {
                    MessageBox.Show("Syötä toimitilan hinta", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (txtPriceD.Text == "0")
                {
                    var result = MessageBox.Show("Haluatko varmasti asettaa toimitilan hinnaksi 0€?", "Toimitilan hinta.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                var newWorkspace = new Workspace
                {
                    WSName = txtName.Text,
                    WSDescription = txtDescription.Text,
                    //PriceByHour = Convert.ToDouble(txtPriceH.Text, CultureInfo.InvariantCulture),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text, CultureInfo.InvariantCulture),
                    //PriceByWeek = Convert.ToDouble(txtPriceW.Text, CultureInfo.InvariantCulture),
                    //PriceByMonth = Convert.ToDouble(txtPriceM.Text, CultureInfo.InvariantCulture),
                    OfficeID = office.OfficeID
                };

                if(txtName.Text == string.Empty )
                {
                    MessageBox.Show("Syötä toimitilan nimi", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if(txtDescription.Text == string.Empty)
                {
                    MessageBox.Show("Syötä toimitilan kuvaus", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                workspaceRepo.AddNewWorkspace(newWorkspace, office);
                MessageBox.Show("Toimipisteen lisääminen onnistui!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
                ResetAll();
            }

            else
            {
                MessageBox.Show("Toimitilan lisääminen ei onnistunut.", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }


        /// <summary>
        /// Poistetaan toimitila Poista toimitila -välilendellä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteWorkspace_Click(object sender, RoutedEventArgs e)
        {
            //var deleteWorkspace = this.DataContext as Workspace;
            //int workspaceID;
            //if (int.TryParse(deleteWorkspaceIdBox.Text, out workspaceID))
            //{
            //    workspaceRepo.DeleteWorkspace(deleteWorkspace);
            //    dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
            //    return;
            //}

            //else
            //{
            //    MessageBox.Show("ÄÄÄÄÄ!", "Huomautus", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
        }


        /// <summary>
        /// Valitun välilehden otsikko on oranssilla, muut mustalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderFontChange_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //if (tDelete.IsSelected == true)
            //{
            //    //tbDelete.Foreground = Brushes.Orange;
            //    tbAdd.Foreground= Brushes.Black;
            //    tbSearch.Foreground= Brushes.Black;
            //    tbList.Foreground= Brushes.Black;
            //}
            if (tAdd.IsSelected == true)
            {
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Orange;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Black;
            }

            if (tSearch.IsSelected == true)
            {
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Orange;
                tbList.Foreground = Brushes.Black;
            }

            if (tList.IsSelected == true)
            {
                //tbDelete.Foreground = Brushes.Black;
                tbAdd.Foreground = Brushes.Black;
                tbSearch.Foreground = Brushes.Black;
                tbList.Foreground = Brushes.Orange;
            }
        }


        /// <summary>
        /// Sulkee ikkunan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Estää muiden kuin numeroiden syöttämisen tekstikenttään, johon on määritelty tämä toiminto (PreviewTextInput)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        /// <summary>
        /// Painettaessa Menusta Asiakkaat..., avautuu CustomerWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow();
            customerWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Toimipisteet..., avautuu OfficeWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OfficeWindow_Click(object sender, RoutedEventArgs e)
        {
            OfficeWindow officeWindow = new OfficeWindow();
            officeWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Laitteet..., avautuu DeviceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeviceWindow_Click(object sender, RoutedEventArgs e)
        {
            DeviceWindow deviceWindow = new DeviceWindow();
            deviceWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Laskut..., avautuu Invoice2Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Invoice_Click(object sender, RoutedEventArgs e)
        {
            NewInvoiceWindow invoiceWindows = new NewInvoiceWindow();
            invoiceWindows.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa ExistingReservationWindow:sta Uusi varaus Buttonia, avautuu ReservationWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReservationWindow_Click(object sender, RoutedEventArgs e)
        {
            ReservationWindow reservationWindow = new ReservationWindow();
            reservationWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Varaukset..., avautuu ExistingReservationWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExistingReservationWindow_Click(object sender, RoutedEventArgs e)
        {
            ExistingReservationWindow existingReservationWindow = new ExistingReservationWindow();
            existingReservationWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Palvelut..., avautuu ServiceWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceWindow_Click(object sender, RoutedEventArgs e)
        {
            ServiceWindow serviceWindow = new ServiceWindow();
            serviceWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Raportointi..., avautuu RaportWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportWindow_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow();
            reportWindow.Show();
            Close();
        }


        /// <summary>
        ///Painettaessa Menusta Admin..., avautuu DeleteDataWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DeleteDataWindow deleteDataWindow = new DeleteDataWindow();
            deleteDataWindow.Show();
            Close();
        }


        /// <summary>
        /// Painettaessa Menusta Ohje, avautuu HelpWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpWindow_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
            Close();
        }


        /// <summary>
        /// WorkspaceWindow Shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkspaceWindow_KeyDown(object sender, KeyEventArgs e)
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

        // resetoi käyttöliittymän textboksien sisällön
        private void ResetAll()
        {
            myWorkspaceIdBox.Text = string.Empty;
            nameBox.Text = string.Empty;
            descriptionBox.Text = string.Empty;
            priceDBox.Text = string.Empty;
            cmbIsActive.SelectedIndex = -1;
            myWorkspaceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            cmbIsActive.IsEnabled = false;


            txtName.Text = string.Empty;
            txtPriceD.Text = string.Empty;
            txtDescription.Text = string.Empty;
            cmbOffices.SelectedIndex = -1;
        }


        /// <summary>
        /// Kun hiiri tulee textBox:n päälle, Focusable muuttuu trueksi.Mahdollistaa oranssin kehyksen näkymisen välilehteä avattaessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myWorkspaceIdBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            myWorkspaceIdBox.Focusable = true;
        }
    }
}
