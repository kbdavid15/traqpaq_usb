using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace traqpaqWPF
{
    //TODO consider changing this class to a "session" where information about what the user was working on last is saved as well, so they can pick up where they left off
    public class RecentTracks
    {
        ObservableCollection<Record> _RecordList = new ObservableCollection<Record>();
        public ObservableCollection<Record> RecordList { get { return _RecordList; } }

        public RecentTracks() { }

        /// <summary>
        /// Adds the track to the top of the list. Remove duplicates from the list as well
        /// </summary>
        /// <param name="r"></param>
        public void pushRecord(Record r)
        {
            int idx = _RecordList.IndexOf(r);
            if (idx >= 0)    //tests if the record is already in the list
            {
                _RecordList.RemoveAt(idx);
            }
            if (!isListFull())
            {
                _RecordList.Insert(0, r);
            }

            // save the property
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Since the recent tracks list should only keep track of the last 10 items
        /// </summary>
        /// <returns></returns>
        public bool isListFull()
        {
            return _RecordList.Count > 10;
        }
    }
}
