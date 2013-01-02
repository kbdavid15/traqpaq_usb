using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace traqpaqWPF
{
    class DummyRecord : Record
    {
        public LapInfo lap = new LapInfo();


        /// <summary>
        /// open the file, extract the stored data, and create a Record object
        /// </summary>
        public DummyRecord()
        {
            string[] rows = traqpaqWPF.Properties.Resources.cardrivedata.Split('\n');
            foreach (string row in rows)
            {
                if (row != "")
                {
                    string[] cols = row.Split(',');
                    lap.Latitudes.Add(double.Parse(cols[2]));
                    lap.Longitudes.Add(double.Parse(cols[1]));
                    lap.Altitude.Add(double.Parse(cols[3]));
                    lap.Velocity.Add(double.Parse(cols[4]));
                }
            }

            this.trackName = "Car Drive";
            // add other information
            lap.LapColor = System.Windows.Media.Colors.Blue;
            lap.LapNo = "1";
            lap.LapTime = "5:32";
            lap.Track = trackName;

            this.DateStamp = new DateTime(2013, 1, 1).ToLongDateString();
            
            // add the lap to the record
            this.Laps.Add(lap);

        }
    }
}
