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
    public class Record
    {
        public string trackName { get; set; }
        public string DateStamp { get; set; }
        public Record(string trackname, string date)
        {
            trackName = trackname;
            DateStamp = date;
        }
    }
    /// <summary>
    /// Interaction logic for RecordTablePage.xaml
    /// </summary>
    public partial class RecordTablePage : Page
    {
        ObservableCollection<Record> _RecordTable = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordTable { get { return _RecordTable; } }

        public RecordTablePage()
        {
            InitializeComponent();
            _RecordTable.Add(new Record("Hi", "Today"));
        }
    }

    
}
