using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using xe = Xceed.Wpf.Toolkit;

namespace traqpaqWPF
{
    public class LapInfo
    {
        public string LapNo { get; set; }
        public string LapTime { get; set; }
        public Color LapColor { get; set; }
    }

    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        MainWindow main;
        GoogleEarthWebBrowser geBrowser;

        //TraqpaqDevice.RecordTableReader.RecordTable recordTable;

        ObservableCollection<LapInfo> _LapCollection = new ObservableCollection<LapInfo>();
        public ObservableCollection<LapInfo> LapCollection { get { return _LapCollection; } }
        
        /// <summary>
        /// This page should not be created until a run is selected
        /// </summary>
        /// <param name="recordTable"></param>
        public DataPage(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
            //this.recordTable = recordTable;

            // if internet connection, use web browser to load google earth
            // otherwise, just plot the points
            //TODO figure this out, for now assume internet and fail gracefully
            geBrowser = new GoogleEarthWebBrowser();
            mainGrid.Children.Add(geBrowser);
            Grid.SetColumn(geBrowser, 1);

            _LapCollection.Add(new LapInfo { LapNo = "1", LapTime = "2:30", LapColor = Colors.LawnGreen });
            _LapCollection.Add(new LapInfo { LapNo = "2", LapTime = "2:24", LapColor = Colors.LightSlateGray });

        }

        /// <summary>
        /// Runs when a Checkbox in the lapListView is Checked or Unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LapCheckBox_Un_Checked(object sender, EventArgs e)
        {
            // get the selected items and update the infobox, also generate KML files to overlay on GE
            foreach (LapInfo item in listViewLaps.Items)
            {
                // use this to determine average lap time, average speed, max speed, etc
                // show a message box with the selected color
                List<double> latitudes = new List<double> { 42.1838786, 42.17332503, 42.1607474, 42.15192871, 
                                                            42.14788076, 42.1538081, 42.16493994, 42.17462615,
                                                            42.18286665, 42.1838786, 42.1838786};
                List<double> longitudes = new List<double> { -83.32956822, -83.34756995, -83.35109965, -83.34209883,
                                                             -83.32321473, -83.31368443, -83.30027137, -83.30556597,
                                                             -83.3188025, -83.32939178, -83.32939178 };
                //string kml = KmlCreator.getKMLstring(item.LapColor, latitudes, longitudes);
                //geBrowser.loadKML(kml);
                geBrowser.addPoints(latitudes, longitudes, "#" + item.LapColor.ToString().Substring(3));
                //TODO test removal of lap
            }
        }

        /// <summary>
        /// Select all the checkboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LapHeaderCheckBox_Checked(object sender, EventArgs e)
        {
            listViewLaps.SelectAll();
        }

        /// <summary>
        /// Unselect all the checkboxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LapHeaderCheckBox_Unchecked(object sender, EventArgs e)
        {
            listViewLaps.SelectedItems.Clear();
        }

        /// <summary>
        /// Update the map with the new color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {

        }
    }
}
