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
        public string LapColor { get; set; }
    }

    /// <summary>
    /// Interaction logic for DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        //TraqpaqDevice.RecordTableReader.RecordTable recordTable;

        ObservableCollection<LapInfo> _LapCollection = new ObservableCollection<LapInfo>();
        public ObservableCollection<LapInfo> LapCollection
        {
            get { return _LapCollection; }
        }
        
        /// <summary>
        /// This page should not be created until a run is selected
        /// </summary>
        /// <param name="recordTable"></param>
        public DataPage()
        {
            InitializeComponent();
            //this.recordTable = recordTable;

            // if internet connection, use web browser to load google earth
            // otherwise, just plot the points
            //TODO figure this out, for now assume internet and fail gracefully
            //GoogleEarthWebBrowser geBrowser = new GoogleEarthWebBrowser();
            //mainGrid.Children.Add(geBrowser);
            //Grid.SetColumn(geBrowser, 1);

            _LapCollection.Add(new LapInfo { LapNo = "1", LapTime = "2:30", LapColor = "red" });

            
        }
    }
}
