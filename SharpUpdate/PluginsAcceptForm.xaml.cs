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
        public SharpUpdateXml[] GeneralJobs;
        public PluginsAcceptForm(int ValidJobsNum,List<int>ValidJobs,SharpUpdateXml[] Jobs,Version Update,Version CuurentVersion)
        {
            InitializeComponent();
            GeneralJobs = Jobs;
            #region Check Jobs
            if (ValidJobsNum > 0)
            {
                UpdateButton.IsEnabled = true;
                DetailsButton.IsEnabled = true;
                UpdateOne.Text = "Version "+Update.ToString()+" is Available";
            }
            else
            {
                UpdateOne.Text = "No Updates Available";
            }
            #endregion
            VersionOne.Text = CuurentVersion.ToString();
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
            UpdateDetails UpDetails = new UpdateDetails(GeneralJobs);
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
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Action)
            {
                Action = false;
            }
            
        }
    }
}
