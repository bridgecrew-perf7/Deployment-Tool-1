using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for UpdateDetails.xaml
    /// </summary>
    public partial class UpdateDetails : Window
    {
        public UpdateDetails(SharpUpdateXml[] Jobs)
        {
            InitializeComponent();
            List<string> DescriptionsData = new List<string>();
            foreach(var J in Jobs)
            {
                DescriptionsData.Add(J.Description);
            }

            
            var Data = Descriptions.GetData(DescriptionsData);
            Grid1.ItemsSource = Data;
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
        private void Grid1_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var col = e.Column as DataGridTextColumn;
            
            col.Width = 570;
            var style = new Style(typeof(TextBlock));
            style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            style.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            style.Setters.Add(new Setter(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center));

            col.ElementStyle = style;

            var Headerstyle = new Style(typeof(DataGridColumnHeader));
            Headerstyle.Setters.Add(new Setter(DataGridColumnHeader.VerticalAlignmentProperty, VerticalAlignment.Center));
            Headerstyle.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            Headerstyle.Setters.Add(new Setter(FontWeightProperty, FontWeights.Bold));
            col.HeaderStyle = Headerstyle;

        }

        private void OK(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
    class Descriptions
    {

        public string Description { get; set; }


        public static ObservableCollection<Descriptions> GetData( List<string> Definition)
        {

            var Data = new ObservableCollection<Descriptions>();

            #region making rows with the data
            int count = 0;
            foreach (string Serial in Definition)
            {
                var SerialInApp = "0";

                Data.Add(new Descriptions() { Description = Serial });
                count = count + 1;
            }

            #endregion
            return Data;
        }

    }
}
