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

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        MainWindow main;

        ObservableCollection<Record> _RecentTracks = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecentTracks { get { return _RecentTracks; } }

        public HomePage(MainWindow main)
        {
            InitializeComponent();
            this.main = main;

            // if there are recent Records in the settings array, then add them to the _RecentTracks ovservable collection
            // if the array is not initialized, then set it to contain 10 elements
            if (Properties.Settings.Default.RecentTracks == null)
            {
                Properties.Settings.Default.RecentTracks = new Record[10];
            }

            foreach (Record record in Properties.Settings.Default.RecentTracks)
            {
                if (record != null)
                {
                    _RecentTracks.Add(record); 
                }
            }
        }
    }
}
