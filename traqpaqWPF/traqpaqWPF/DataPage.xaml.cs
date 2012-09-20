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
    
    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        MainWindow main;
        GoogleEarthWebBrowser geBrowser;

        //TraqpaqDevice.RecordTableReader.RecordTable recordTable;
        Record recordTable;

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

            // if internet connection, use web browser to load google earth
            // otherwise, just plot the points
            //TODO figure this out, for now assume internet and fail gracefully
            geBrowser = new GoogleEarthWebBrowser();
            geBrowser.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            mainGrid.Children.Add(geBrowser);
            Grid.SetColumn(geBrowser, 2);
        }

        /// <summary>
        /// This function must be called before using the page
        /// If the new record is the same as the current then nothing changes
        /// </summary>
        /// <param name="record"></param>
        public void setRecord(Record record)
        {
            if (recordTable == null)
            {
                this.recordTable = record;
                _LapCollection.Clear();
                // Add the laps to the Collection
                foreach (LapInfo lap in recordTable.Laps)
                {
                    _LapCollection.Add(lap);
                }
            }
            else if (!recordTable.Equals(record))
            {
                this.recordTable = record;
                _LapCollection.Clear();
                // Add the laps to the Collection
                foreach (LapInfo lap in recordTable.Laps)
                {
                    _LapCollection.Add(lap);
                }
            }
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
            geBrowser.addPoints(lap.Latitudes, lap.Longitudes, lap.LapColor, lap.LapNo);
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