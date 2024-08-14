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
using vt_systems.OfficeData;
using vt_systems.WorkspaceData;

namespace vt_systems.TestWindows
{
    /// <summary>
    /// Interaction logic for WorkspaceTestWindow.xaml
    /// </summary>
    public partial class WorkspaceTestWindow : Window
    {
        private WorkSpaceRepo workspaceRepo = new WorkSpaceRepo();
        private OfficeRepo officeRepo = new OfficeRepo();
        public WorkspaceTestWindow()
        {
            InitializeComponent();
            myWorkspaceIdBox.IsEnabled = true;
            nameBox.IsEnabled = false;
            descriptionBox.IsEnabled = false;
            priceHBox.IsEnabled = false;
            priceDBox.IsEnabled = false;
            priceWBox.IsEnabled = false;
            priceMBox.IsEnabled = false;


            this.DataContext = new Workspace();
            dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
            cmbOffices.ItemsSource = officeRepo.GetOffices();
        }

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
                    priceHBox.IsEnabled = true;
                    priceDBox.IsEnabled = true;
                    priceWBox.IsEnabled = true;
                    priceMBox.IsEnabled = true;

                    nameBox.Text = workspace.WSName;
                    descriptionBox.Text = workspace.WSDescription;
                    priceHBox.Text = workspace.PriceByHour.ToString();
                    priceDBox.Text = workspace.PriceByDay.ToString();
                    priceWBox.Text = workspace.PriceByWeek.ToString();
                    priceMBox.Text = workspace.PriceByMonth.ToString();

                }
                else
                {
                    myWorkspaceIdBox.IsEnabled = true;
                }

            }
            else
            {
                MessageBox.Show("Anna kelvollinen toimitila id");
            }

        }

        private void UpdateWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var updateWorkspace = this.DataContext as Workspace;
            updateWorkspace.WSName = nameBox.Text;
            updateWorkspace.WSDescription = descriptionBox.Text;
            updateWorkspace.PriceByHour = Convert.ToDouble(priceHBox.Text);
            updateWorkspace.PriceByDay = Convert.ToDouble(priceDBox.Text);
            updateWorkspace.PriceByWeek = Convert.ToDouble(priceWBox.Text);
            updateWorkspace.PriceByMonth = Convert.ToDouble(priceMBox.Text);

            if (updateWorkspace.WSName == string.Empty || updateWorkspace.WSDescription == string.Empty)
            {
                MessageBox.Show("ÄÄÄÄÄÄÄÄH!");
                return;
            }
            workspaceRepo.UpdateWorkspace(updateWorkspace);
            MessageBox.Show("Tietojen päivitys onnistui!");
            dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();

        }

        private void AddNewWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var office = cmbOffices.SelectedItem as Office;
            if (txtName.Text != string.Empty && txtDescription.Text != string.Empty && cmbOffices.SelectedItem != null)
            {
                var newWorkspace = new Workspace
                {

                    WSName = txtName.Text,
                    WSDescription = txtDescription.Text,
                    PriceByHour = Convert.ToDouble(txtPriceH.Text),
                    PriceByDay = Convert.ToDouble(txtPriceD.Text),
                    PriceByWeek = Convert.ToDouble(txtPriceW.Text),
                    PriceByMonth = Convert.ToDouble(txtPriceM.Text),
                    OfficeID = office.OfficeID


                };
                workspaceRepo.AddNewWorkspace(newWorkspace, office);
                MessageBox.Show("Toimipisteen lisääminen onnistui!");
                dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
            }
            else
            {
                MessageBox.Show("ÄÄÄÄÄÄÄÄH!");
                return;
            }
        }

        private void DeleteWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var deleteWorkspace = this.DataContext as Workspace;
            int workspaceID;
            if (int.TryParse(deleteWorkspaceIdBox.Text, out workspaceID))
            {

                workspaceRepo.DeleteWorkspace(deleteWorkspace);
                dgWorkspaces.ItemsSource = workspaceRepo.GetWorkspaces();
                return;

            }
            else
            {
                MessageBox.Show("ÄÄÄÄÄ!");
            }

        }
    }
}
