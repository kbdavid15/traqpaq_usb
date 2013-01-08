using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using traqpaqWPF.Properties;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        MainWindow main;

        ObservableCollection<Record> _RecordTable = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordTable { get { return _RecordTable; } }
        
        public HomePage(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            
            // if there are recent Records in the settings array, then add them to the _RecentTracks ovservable collection
            // if the array is not initialized, then set it to contain 10 elements
            if (Settings.Default.RecentTracks == null)
            {
                Settings.Default.RecentTracks = new RecentTracks();
            }

            // set up event handler for the properties collection
            Properties.Settings.Default.RecentTracks.RecordList.CollectionChanged += recordList_CollectionChanged;

            updateRecentTracks();
        }

        void recordList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            updateRecentTracks();
        }

        /// <summary>
        /// Called when the property collection is modified
        /// </summary>
        public void updateRecentTracks()
        {
            foreach (Record record in Settings.Default.RecentTracks.RecordList)
            {
                if (record != null)
                {
                    _RecordTable.Add(record);
                }
            }
        }

        private void buttonShowRecentRecords_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Contents of properties.settings:\n" + Properties.Settings.Default.RecentTracks.ToString() + "\nLength: " + Properties.Settings.Default.RecentTracks.RecordList.Count);
            MessageBox.Show("Contents of _RecentTracks:\nLength: " + _RecordTable.Count + "\nItems: " + _RecordTable.ToString());
        }
    }
}
