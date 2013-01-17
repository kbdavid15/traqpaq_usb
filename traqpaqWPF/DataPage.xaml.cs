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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.ViewportConstraints;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Common.Auxiliary;
using Microsoft.Research.DynamicDataDisplay.Common;
using Microsoft.Research.DynamicDataDisplay.Charts.NewLine;
using Microsoft.Research.DynamicDataDisplay.Markers2;
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

            // this disables scrolling along the main vertical axis and was necessary in order to fix gh issue #6
            plotter.Children.RemoveAllOfType(typeof(AxisNavigation));
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
                // add record to the recentTracks property for quick viewing on the home page
                Properties.Settings.Default.RecentTracks.pushRecord(r); //TODO figure out why this isn't working

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

            // set the isChecked property
            lap.isChecked = true;

            // plot the points on Google Maps
            geBrowser.addPoints(lap.Latitudes, lap.Longitudes, lap.LapColor, lap.Track, lap.LapNo);

            // plot the altitude and speed on the chart
            addLapToPlotter(lap);

            //TODO add event handler for mouse over event that displays the exact value for graph
        }
        
        /// <summary>
        /// Remove the lap from the map
        /// </summary>
        /// <param name="sender">The CheckBox that was unchecked</param>
        void LapCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            LapInfo lap = (LapInfo)cb.Tag;
            // update isChecked prop
            lap.isChecked = false;

            // remove from map
            geBrowser.removeLap(lap.Track, lap.LapNo);

            // remove from plotter
            removeLapFromPlotter(lap);
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
        /// Update the map with the new color, as well as the plotter
        /// </summary>
        void ColorPicker_SelectedColorChanged(object sender, EventArgs e)
        {
            xe.ColorPicker cp = sender as xe.ColorPicker;
            LapInfo lap = (LapInfo)cp.Tag;
            geBrowser.changeColor(lap.Track, lap.LapNo, cp.SelectedColor);

            // update the plotter color
            foreach (LineChart lchart in getLineCharts(lap))
            {
                lchart.Stroke = new SolidColorBrush(lap.LapColor);
            }
        }

        /// <summary>
        /// Make the speed plots visible if hidden, or create them if they have not yet been created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSpeed_Checked(object sender, RoutedEventArgs e)
        {
            if (recordTable != null)
            {
                LineChart hiddenChart;
                foreach (Record r in recordTable)
                {
                    foreach (LapInfo lap in r.Laps)
                    {
                        if (lap.isChecked == true)
                        {
                            hiddenChart = getLineCharts(lap, ChartOptions.SPEED);
                            if (hiddenChart == null)
                            {
                                addLapToPlotter(lap);// chart must be added
                            }
                            else
                            {
                                hiddenChart.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Make the altitude plots visible if hidden, or create them if they have not yet been created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAltitude_Checked(object sender, RoutedEventArgs e)
        {
            if (recordTable != null)
            {
                LineChart hiddenChart;
                foreach (Record r in recordTable)
                {
                    foreach (LapInfo lap in r.Laps)
                    {
                        if (lap.isChecked == true)
                        {
                            hiddenChart = getLineCharts(lap, ChartOptions.ALTITUDE);
                            if (hiddenChart == null)
                            {
                                addLapToPlotter(lap);// chart must be added
                            }
                            else
                            {
                                hiddenChart.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Hide the altitude plot(s) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxAltitude_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in getLineCharts(ChartOptions.ALTITUDE))
            {
                item.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Hide the speed plot(s)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSpeed_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in getLineCharts(ChartOptions.SPEED))
            {
                item.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// Adds the specified lap to the chart plotter control.
        /// Color will be determined by the currently selected LapColor
        /// Altitude is dashed and speed is a solid line
        /// plotter plots Altitude while innerPlotter plots Speed/Velocity
        /// </summary>
        /// <param name="lap"></param>
        void addLapToPlotter(LapInfo lap)
        {
            double[] xPoints = new double[lap.Altitude.Count];
            for (int i = 0; i < lap.Altitude.Count; i++)
            {
                xPoints[i] = i;
            }
            Brush lapColor = new SolidColorBrush(lap.LapColor);
            // look at checkboxes to see if user wants altitude, speed
            if (checkBoxAltitude.IsChecked == true)
            {
                var altitudeDS = DataSource.Create(xPoints, lap.Altitude);
                LineChart chart = new LineChart();
                chart.DataSource = altitudeDS;
                chart.Stroke = lapColor;
                chart.StrokeThickness = 2;
                chart.Description = "Altitude";
                chart.StrokeDashArray = new DoubleCollection(new double[] { 1, 1 });
                chart.Tag = lap;    // chart tag holds the corresponding lap object. used for chart removal
                innerAltitudePlotter.AddChild(chart);
            }
            if (checkBoxSpeed.IsChecked == true)
            {
                var speedDS = DataSource.Create(xPoints, lap.Velocity);
                LineChart chart = new LineChart();
                chart.DataSource = speedDS;
                chart.Stroke = lapColor;
                chart.StrokeThickness = 2;
                chart.Description = "Velocity";
                chart.Tag = lap;    // chart tag holds the corresponding lap object. used for chart removal
                innerSpeedPlotter.Children.Add(chart);
            }
        }

        /// <summary>
        /// Remove the specified lap from the plotter. Compare with the chart.Tag property
        /// </summary>
        /// <param name="lap"></param>
        void removeLapFromPlotter(LapInfo lap)
        {
            foreach (LineChart chart in getLineCharts(lap))
            {
                //chart.RemoveFromPlotter();
                chart.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        
        #region PlotterHelperFunctions
        // These functions will be used for common operations with the plotter
        //TODO consider rebuilding the library with these functions built in

        /// <summary>
        /// Gets all the charts on the plotters
        /// </summary>
        /// <returns></returns>
        List<LineChart> getLineCharts()
        {
            List<LineChart> charts = new List<LineChart>();
            foreach (IPlotterElement child in plotter.Children)
                if (typeof(LineChart) == child.GetType())
                    charts.Add((LineChart)child);
            return charts;
        }

        /// <summary>
        /// Gets a specific set of line charts that are either Altitude or Speed
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<LineChart> getLineCharts(ChartOptions option)
        {
            List<LineChart> charts = new List<LineChart>();
            switch (option)
            {
                case ChartOptions.ALTITUDE:
                    foreach (IPlotterElement child in innerAltitudePlotter.Children)
                        if (typeof(LineChart) == child.GetType())
                            charts.Add((LineChart)child);
                    break;
                case ChartOptions.SPEED:
                    foreach (IPlotterElement child in innerSpeedPlotter.Children)
                        if (typeof(LineChart) == child.GetType())
                            charts.Add((LineChart)child);
                    break;
            }
            return charts;
        }

        /// <summary>
        /// Gets the charts that are specific to a lap
        /// </summary>
        /// <param name="lap"></param>
        /// <returns></returns>
        List<LineChart> getLineCharts(LapInfo lap)
        {
            List<LineChart> charts = new List<LineChart>();
            foreach (IPlotterElement child in plotter.Children)
            {
                if (typeof(LineChart) == child.GetType())
                {
                    LineChart chart = child as LineChart;
                    if (lap.Equals((LapInfo)chart.Tag)) // find the chart that matches the lap object
                    {
                        charts.Add(chart);
                    }
                }
            }
            return charts;
        }

        /// <summary>
        /// This method can return at most one chart meeting both the specified criteria.
        /// </summary>
        /// <param name="lap"></param>
        /// <param name="option"></param>
        /// <returns>The LineChart of the chart matching criteria. If one is not found, return null</returns>
        LineChart getLineCharts(LapInfo lap, ChartOptions option)
        {
            switch (option)
            {
                case ChartOptions.ALTITUDE:
                    foreach (IPlotterElement child in innerAltitudePlotter.Children)
                    {
                        if (typeof(LineChart) == child.GetType())
                        {
                            LineChart chart = child as LineChart;
                            if (lap.Equals((LapInfo)chart.Tag)) // find the chart that matches the lap object
                            {
                                return chart;
                            }
                        }
                    }
                    break;
                case ChartOptions.SPEED:
                    foreach (IPlotterElement child in innerSpeedPlotter.Children)
                    {
                        if (typeof(LineChart) == child.GetType())
                        {
                            LineChart chart = child as LineChart;
                            if (lap.Equals((LapInfo)chart.Tag)) // find the chart that matches the lap object
                            {
                                return chart;
                            }
                        }
                    }
                    break;
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Resets the view of the plotter to see the entire chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonResetView_Click(object sender, RoutedEventArgs e)
        {
            plotter.FitToView();
        }
    }
}