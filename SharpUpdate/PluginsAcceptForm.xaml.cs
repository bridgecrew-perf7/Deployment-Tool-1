using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SharpUpdate
{
    /// <summary>
    /// Interaction logic for PluginsAcceptForm.xaml
    /// </summary>
    public partial class PluginsAcceptForm : Window
    {
        public bool Action =false;
        public int ProjectChoosen= 0;
        public SharpUpdateXml[] GeneralJobsSmart;
        public SharpUpdateXml[] GeneralJobsDeployer;
        public PluginsAcceptForm(List<List<int>> ValidJobsList, List<SharpUpdateXml[]> ListOfUpdates, List<Version> MaxVersionList, List<Version> CurrentVersionList,List<string> Programs)
        {

            InitializeComponent();

            #region Check Assigned Programs
            if (!(Programs.Contains("DeployerTool")))
            {
                TextTwo.Visibility = Visibility.Hidden;
                UpdateTwo.Visibility = Visibility.Hidden;
                VersionTwo.Visibility = Visibility.Hidden;
                DetailsButtonTwo.Visibility = Visibility.Hidden;
                UpdateButtonTwo.Visibility = Visibility.Hidden;

                TextOne.SetValue(Grid.RowProperty, 0);
                UpdateOne.SetValue(Grid.RowProperty, 0);
                VersionOne.SetValue(Grid.RowProperty, 0);
                DetailsButton.SetValue(Grid.RowProperty, 0);
                UpdateButton.SetValue(Grid.RowProperty, 0);

                ClosePanel.Margin = new Thickness(0, -40, 0, 0);
            }
            if (!(Programs.Contains("SmartDesignUpdate")))
            {
                TextOne.Visibility = Visibility.Hidden;
                UpdateOne.Visibility = Visibility.Hidden;
                VersionOne.Visibility = Visibility.Hidden;
                DetailsButton.Visibility = Visibility.Hidden;
                UpdateButton.Visibility = Visibility.Hidden;

                ClosePanel.Margin = new Thickness(0, -40, 0, 0);
            }
            #endregion

            GeneralJobsSmart = ListOfUpdates[0];
            GeneralJobsDeployer = ListOfUpdates[1];
            #region Check Jobs
            if (ValidJobsList[0].Count > 0)
            {
                UpdateButton.IsEnabled = true;
                DetailsButton.IsEnabled = true;
                UpdateOne.Text = "Version "+ MaxVersionList[0].ToString()+" is Available";
            }
            else
            {
                UpdateOne.Text = "No Updates Available";
            }
            if (ValidJobsList[1].Count > 0)
            {
                UpdateButtonTwo.IsEnabled = true;
                DetailsButtonTwo.IsEnabled = true;
                UpdateTwo.Text = "Version " + MaxVersionList[1].ToString() + " is Available";
            }
            else
            {
                UpdateOne.Text = "No Updates Available";
            }
            #endregion
            VersionOne.Text = CurrentVersionList[0].ToString();
            VersionTwo.Text = CurrentVersionList[1].ToString();
            SetPropertires();
        }

        void SetPropertires()
        {
            System.Drawing.Image img = Properties.Resources.Deployer;
            this.Icon = GetImageSource(img);
        }

        private BitmapSource GetImageSource(System.Drawing.Image img)
        {
            BitmapImage bmp = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;
                bmp.EndInit();
            }
            return bmp;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Action = false;
            Close();
        }

        private void Details(object sender, RoutedEventArgs e)
        {
            #region Details
            UpdateDetails UpDetails = new UpdateDetails(GeneralJobsSmart);
            UpDetails.Height = 395;
            UpDetails.Width = 600;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = UpDetails.Width;
            double windowHeight = UpDetails.Height;
            UpDetails.Left = (screenWidth / 2) - (windowWidth / 2);
            UpDetails.Top = (screenHeight / 2) - (windowHeight / 2);
            UpDetails.ShowDialog();
            #endregion
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            Action = true;
            ProjectChoosen = 0;
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Action)
            {
                Action = false;
            }
            
        }

        private void DetailsButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            #region Details
            UpdateDetails UpDetails = new UpdateDetails(GeneralJobsDeployer);
            UpDetails.Height = 395;
            UpDetails.Width = 600;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = UpDetails.Width;
            double windowHeight = UpDetails.Height;
            UpDetails.Left = (screenWidth / 2) - (windowWidth / 2);
            UpDetails.Top = (screenHeight / 2) - (windowHeight / 2);
            UpDetails.ShowDialog();
            #endregion
        }

        private void UpdateButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            Action = true;
            ProjectChoosen = 1;
            Close();
        }
    }
}
