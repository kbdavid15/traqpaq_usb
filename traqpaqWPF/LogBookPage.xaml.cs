﻿using System;
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

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for LogBookPage.xaml
    /// </summary>
    public partial class LogBookPage : Page
    {
        ObservableCollection<Record> _RecordTable = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordTable { get { return _RecordTable; } }
        MainWindow main;

        #region dummy
        /*
         * This is a dummy class for testing purposes that will be added to 
         * the record collection regardless of whether the device is connected or not
         */
        DummyRecord dummy = new DummyRecord();
        #endregion


        public LogBookPage(MainWindow main)
        {
            InitializeComponent();
            this.main = main;

            // add the dummy record
            _RecordTable.Add(dummy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void populateTracks()
        {
            foreach (TraqpaqDevice.RecordTableReader.RecordTable item in main.traqpaq.recordTableList)
            {
                _RecordTable.Add(new Record(main.traqpaq, item));
            }
        }

        /// <summary>
        /// Double clicking an item in the listview shows the Data page and passes the item to it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listViewRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != listViewRecords)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    goToDataPage(new Record[] { (Record)listViewRecords.SelectedItem });                    
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        /// <summary>
        /// When the selected record changes, update the canvas preview and the stats pane
        /// </summary>
        void listViewRecords_SelectionChanged(object sender, EventArgs e)
        {
            Record session = (Record)listViewRecords.SelectedItem;
            Polyline polyline = new Polyline();
            polyline.Stroke = Brushes.Black;

            //TODO fix error with multiple laps
            //LapInfo lap = session.Laps[1];
            foreach (LapInfo lap in session.Laps)
            {
                // find the smallest dimension, width or height, and use that to calculate the points
                int size;
                if (viewboxPreviewPane.RenderSize.Width > viewboxPreviewPane.RenderSize.Height)
                {
                    size = (int)viewboxPreviewPane.RenderSize.Height;
                }
                else
                {
                    size = (int)viewboxPreviewPane.RenderSize.Width;
                }

                // check if there is data in lat and long. If there is not, display message in preview pane
                if (lap.Longitudes.Count > 0 && lap.Latitudes.Count > 0)
                {
                    double[] xCoord = NormalizeData(lap.Latitudes, 10, size - 10);
                    double[] yCoord = NormalizeData(lap.Longitudes, 10, size - 10);
                    for (int i = 0; i < xCoord.Length; i+=3)    // skip every other to test issue #7
                    {
                        polyline.Points.Add(new Point(xCoord[i], yCoord[i]));
                    }
                }
                else
                {
                    // show error message in preview pane
                    Label l = new Label();
                    l.Content = "No data found";
                    viewboxPreviewPane.Child = l;
                }
            }
            // Add the polyline to the viewbox
            viewboxPreviewPane.Child = polyline;
        }

        /// <summary>
        /// Normalize a set of data. Used to show a preview of the track on the record table page.
        /// </summary>
        /// <param name="data">The enumerable data (doubles) to normalize</param>
        /// <param name="min">Min value of result</param>
        /// <param name="max">Max value of result</param>
        /// <returns>Array of doubles containing normalized data</returns>
        private static double[] NormalizeData(IEnumerable<double> data, int min, int max)
        {
            double dataMax = data.Max();
            double dataMin = data.Min();
            double range = dataMax - dataMin;

            return data
                .Select(d => (d - dataMin) / range)
                .Select(n => ((1 - n) * min + n * max))
                .ToArray();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            Record[] records = new Record[listViewRecords.SelectedItems.Count];
            for (int i = 0; i < listViewRecords.SelectedItems.Count; i++)
            {
                records[i] = (Record)listViewRecords.SelectedItems[i];
            }
            goToDataPage(records);
        }

        /// <summary>
        /// This function is called by the double click event and the datapage button click event
        /// </summary>
        /// <param name="records">Array of Record objects</param>
        private void goToDataPage(Record[] records)
        {
            // construct the record data page
            if (main.dataPage == null)
            {
                DataPage data = new DataPage(main);
                main.dataPage = data;
            }

            main.dataPage.setRecord(records);

            // Navigate to the page
            main.frameLogBook.Navigate(main.dataPage);

            // make the back button visible
            main.buttonBack.Visibility = System.Windows.Visibility.Visible;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class LapInfo
    {
        public string LapNo { get; set; }
        public string LapTime { get; set; }
        public Color LapColor { get; set; }
        public List<double> Latitudes = new List<double>();
        public List<double> Longitudes = new List<double>();
        public List<double> Altitude = new List<double>();
        public List<double> Velocity = new List<double>();
        public string Track { get; set; }
        public bool? isChecked { get; set; }

        /// <summary>
        /// Empty constructor for dummy record
        /// </summary>
        public LapInfo() { }

        //public LapInfo(List<double> latitudes, List<double> longitudes, Color color, string lapNo, string laptime, string track) 
        //{
        //    Latitudes = latitudes;
        //    Longitudes = longitudes;
        //    LapColor = color;
        //    LapNo = lapNo;
        //    LapTime = laptime;
        //    Track = track;
        //}
    }

    public class Record
    {
        public string trackName { get; set; }
        public string DateStamp { get; set; }
        public List<LapInfo> Laps = new List<LapInfo>();

        /// <summary>
        /// Empty constructor for inheritancein dummy record
        /// </summary>
        public Record() { }

        public Record(TraqpaqDevice traqpaq, TraqpaqDevice.RecordTableReader.RecordTable recordTable)
        {
            // get the track name
            trackName = traqpaq.trackList[recordTable.TrackID].trackName;
            //TODO need to convert to readable date format
            DateStamp = recordTable.DateStamp.ToLongDateString();
            // get the data at the record
            //TODO separate laps for now assume 1 lap
            TraqpaqDevice.RecordDataReader dataReader = new TraqpaqDevice.RecordDataReader(traqpaq, recordTable);
            dataReader.readRecordData();

            List<double> longitudes = new List<double>();
            List<double> latitutes = new List<double>();
            List<double> altitudes = new List<double>();
            List<double> velocities = new List<double>();

            foreach (var page in dataReader.recordDataPages)
            {
                foreach (var data in page.RecordData)
                {
                    longitudes.Add(data.Longitude);
                    latitutes.Add(data.Latitude);
                    altitudes.Add(data.Altitude);
                    velocities.Add(data.Speed);
                }
            }
            LapInfo lap = new LapInfo();
            lap.Latitudes = latitutes;
            lap.Longitudes = longitudes;
            lap.LapColor = Colors.Red;
            lap.LapNo = "1";
            lap.LapTime = "1:20";
            lap.Track = trackName;
            Laps.Add(lap);
        }
    }
}