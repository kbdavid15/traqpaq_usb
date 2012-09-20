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

namespace traqpaqWPF
{

    public class LapInfo
    {
        public string LapNo { get; set; }
        public string LapTime { get; set; }
        public Color LapColor { get; set; }
        public List<double> Latitudes { get; set; }
        public List<double> Longitudes { get; set; }
        public bool AreAllChecked = false;
    }

    public class Record
    {
        public string trackName { get; set; }
        public string DateStamp { get; set; }
        public List<LapInfo> Laps { get; set; }
        public Record(string trackname, string date, List<LapInfo> laps)
        {
            trackName = trackname;
            DateStamp = date;
            Laps = laps;
        }
    }

    /// <summary>
    /// Interaction logic for RecordTablePage.xaml
    /// </summary>
    public partial class RecordTablePage : Page
    {
        ObservableCollection<Record> _RecordTable = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordTable { get { return _RecordTable; } }
        MainWindow main;

        // create the dummy laps
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

        public RecordTablePage(MainWindow main)
        {
            InitializeComponent();
            this.main = main;

            List<LapInfo> laps = new List<LapInfo>();
            laps.Add(new LapInfo { LapNo = "1", LapTime = "2:30", LapColor = Colors.LawnGreen, Latitudes = latitudes1, Longitudes = longitudes1 });
            laps.Add(new LapInfo { LapNo = "2", LapTime = "2:24", LapColor = Colors.Red, Latitudes = latitudes2, Longitudes = longitudes2 });
            _RecordTable.Add(new Record("Hi", "Today", laps));
        }

        /// <summary>
        /// Double clicking an item in the listview shows the Data page and passes the item to it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listViewRecords_MouseDoubleClick(object sender, EventArgs e)
        {
            // construct the record data page
            DataPage data = (DataPage)main.pages[(int)PageName.DATA];
            data.setRecord((Record)listViewRecords.SelectedItem);
            // Navigate to the page
            main.navigatePage(PageName.DATA);
        }

        /// <summary>
        /// When the selected record changes, update the canvas preview and the stats pane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listViewRecords_SelectionChanged(object sender, EventArgs e)
        {
            Record session = (Record)listViewRecords.SelectedItem;
            foreach (LapInfo lap in session.Laps)
            {
                Polyline polyline = new Polyline();
                polyline.Stroke = Brushes.Black;
                // find the smallest dimension, width or height, and use that to calculate the points
                int size;
                if (canvasPreviewPane.RenderSize.Width > canvasPreviewPane.RenderSize.Height)
                {
                    size = (int)canvasPreviewPane.RenderSize.Height;
                }
                else
                {
                    size = (int)canvasPreviewPane.RenderSize.Width;
                }
                double[] xCoord = NormalizeData(lap.Latitudes, 10, size - 10);
                double[] yCoord = NormalizeData(lap.Longitudes, 10, size - 10);
                for (int i = 0; i < xCoord.Length; i++)
                {
                    polyline.Points.Add(new Point(xCoord[i], yCoord[i]));
                }

                // Add the polyline to the canvas
                canvasPreviewPane.Children.Add(polyline);
            }            
        }

        /// <summary>
        /// Normalize a set of data
        /// </summary>
        /// <param name="data">The enumerable data (double) to normalize</param>
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
    }    
}
