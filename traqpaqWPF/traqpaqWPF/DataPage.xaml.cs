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
        public List<double> latitudes;
        public List<double> longitudes;
        public bool AreAllChecked = false;
    }

    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        List<double> latitudes1 = new List<double> { 42.1838786, 42.17332503, 42.1607474, 42.15192871, 
                                                            42.14788076, 42.1538081, 42.16493994, 42.17462615,
                                                            42.18286665, 42.1838786, 42.1838786};
        List<double> longitudes1 = new List<double> { -83.32956822, -83.34756995, -83.35109965, -83.34209883,
                                                             -83.32321473, -83.31368443, -83.30027137, -83.30556597,
                                                             -83.3188025, -83.32939178, -83.32939178 };
        List<double> latitudes2 = new List<double> { 42.1848786, 42.17432503, 42.1617474, 42.15292871, 
                                                            42.14888076, 42.1548081, 42.16593994, 42.17562615,
                                                            42.18386665, 42.1848786, 42.1848786};
        List<double> longitudes2 = new List<double> { -83.32856822, -83.34656995, -83.35009965, -83.34109883,
                                                             -83.32221473, -83.31268443, -83.30027137, -83.30456597,
                                                             -83.3198025, -83.32839178, -83.32839178 };
        
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
            geBrowser.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            mainGrid.Children.Add(geBrowser);
            Grid.SetColumn(geBrowser, 2);

            _LapCollection.Add(new LapInfo { LapNo = "1", LapTime = "2:30", LapColor = Colors.LawnGreen, latitudes = latitudes1, longitudes = longitudes1 });
            _LapCollection.Add(new LapInfo { LapNo = "2", LapTime = "2:24", LapColor = Colors.Red, latitudes = latitudes2, longitudes = longitudes2 });
        }

        /// <summary>
        /// Runs when a Checkbox in the lapListView is Checked or Unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LapCheckBox_Checked(object sender, EventArgs e)
        {
            // get the selected items and update the infobox, also generate KML files to overlay on GE
            //foreach (LapInfo item in listViewLaps.SelectedItems)
            //{
            //    // use this to determine average lap time, average speed, max speed, etc                
            //    //string kml = KmlCreator.getKMLstring(item.LapColor, item.latitudes, item.longitudes);
            //    //geBrowser.loadKML(kml, item.LapNo);
            //    plotLap(item);                
            //}
            CheckBox cb = sender as CheckBox;
            LapInfo lap = (LapInfo)cb.Tag;
            geBrowser.addPoints(lap.latitudes, lap.longitudes, lap.LapColor, lap.LapNo);
        }

        /// <summary>
        /// Remove the lap from the map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LapCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            geBrowser.removeLap(((LapInfo)cb.Tag).LapNo);
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
        /// Only matters if the lap is already rendered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            xe.ColorPicker cp = sender as xe.ColorPicker;
            LapInfo lap = (LapInfo)cp.Tag;
            geBrowser.changeColor(lap.LapNo, cp.SelectedColor);
        }
    }
}