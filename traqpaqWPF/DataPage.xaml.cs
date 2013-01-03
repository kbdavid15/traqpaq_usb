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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

namespace traqpaqWPF
{
    
    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        MainWindow main;
        GoogleMapsWebBrowser geBrowser;

        //TraqpaqDevice.RecordTableReader.RecordTable recordTable;
        Record[] recordTable;

        ObservableCollection<LapInfo> _LapCollection = new ObservableCollection<LapInfo>();
        public ObservableCollection<LapInfo> LapCollection { get { return _LapCollection; } }

        /// <summary>
        /// This page should not be created until a run is selected
        /// </summary>
        /// <param name="main">The window that created this page</param>
        public DataPage(MainWindow main)
        {            
            InitializeComponent();
            this.main = main;            

            // if internet connection, use web browser to load google earth
            // otherwise, just plot the points
            //TODO figure this out, for now assume internet and fail gracefully
            geBrowser = new GoogleMapsWebBrowser();
            geBrowser.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            subGrid1.Children.Add(geBrowser);
            Grid.SetColumn(geBrowser, 2);
        }

        /// <summary>
        /// This function must be called before using the page
        /// If the new record is the same as the current then nothing changes
        /// </summary>
        /// <param name="record">Record object containing lap information</param>
        public void setRecord(Record[] records)
        {            
            if (recordTable != null)
            {
                if (!recordTable.Equals(records))
                {
                    // clear lap collection and javascript array
                    _LapCollection.Clear();
                    geBrowser.clearLaps();
                }
            }

            this.recordTable = records;
            // Add the laps to the Collection
            foreach (Record r in recordTable)
            {
                foreach (LapInfo lap in r.Laps)
                {
                    _LapCollection.Add(lap);
                }
            }
        }

        /// <summary>
        /// Runs when a Checkbox in the lapListView is Checked or Unchecked
        /// and plots the lap on the map. Also it will plot the altitude
        /// and speed in the chart below.
        /// </summary>
        /// <param name="sender">The CheckBox that was checked</param>
        void LapCheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            LapInfo lap = (LapInfo)cb.Tag;
            geBrowser.addPoints(lap.Latitudes, lap.Longitudes, lap.LapColor, lap.Track, lap.LapNo);

            // generate chart, make sure to plot all the laps that are checked
            int[] xPoints = new int[lap.Altitude.Count];
            for (int i = 0; i < lap.Altitude.Count; i++)
            {
                xPoints[i] = i;
            }
            var xDataSource = new EnumerableDataSource<int>(xPoints);
            var altitudeDataSource = new EnumerableDataSource<double>(lap.Altitude);
            var velocityDataSource = new EnumerableDataSource<double>(lap.Velocity);
            xDataSource.SetXMapping(x => x);
            altitudeDataSource.SetYMapping(y => y);
            velocityDataSource.SetYMapping(y => y);
            CompositeDataSource compositeSource1 = new CompositeDataSource(xDataSource, altitudeDataSource);
            CompositeDataSource compositeSource2 = new CompositeDataSource(xDataSource, velocityDataSource);
            chartPlotter.AddLineGraph(compositeSource1, Colors.Blue, 1, "Altitude");
            chartPlotter.AddLineGraph(compositeSource2, Colors.Red, 1, "Speed");
            chartPlotter.Viewport.FitToView();
        }

        /// <summary>
        /// Remove the lap from the map
        /// </summary>
        /// <param name="sender">The CheckBox that was unchecked</param>
        void LapCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            LapInfo lap = (LapInfo)cb.Tag;
            geBrowser.removeLap(lap.Track, lap.LapNo);
        }

        /// <summary>
        /// Select all the checkboxes
        /// </summary>
        void LapHeaderCheckBox_Checked(object sender, EventArgs e)
        {
            listViewLaps.SelectAll();
        }
        
        /// <summary>
        /// Unselect all the checkboxes
        /// </summary>
        void LapHeaderCheckBox_Unchecked(object sender, EventArgs e)
        {
            listViewLaps.SelectedItems.Clear();
        }

        /// <summary>
        /// Update the map with the new color
        /// </summary>
        void ColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            xe.ColorPicker cp = sender as xe.ColorPicker;
            LapInfo lap = (LapInfo)cp.Tag;
            geBrowser.changeColor(lap.Track, lap.LapNo, cp.SelectedColor);
        }
    }
}