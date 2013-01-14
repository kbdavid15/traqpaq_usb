using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;

namespace traqpaqWPF
{
    class DummyRecord : Record
    {
        LapInfo lap1 = new LapInfo();
        LapInfo lap2 = new LapInfo();

        /// <summary>
        /// open the file, extract the stored data, and create a Record object
        /// </summary>
        public DummyRecord()
        {
            string[] rows = traqpaqWPF.Properties.Resources.cardrivedata.Split('\n');
            double lon, lat, alt, vel;
            Random rand = new Random();
            foreach (string row in rows)
            {
                if (row != "")
                {
                    string[] cols = row.Split(',');
                    lon = double.Parse(cols[1]);
                    lat = double.Parse(cols[2]);
                    alt = double.Parse(cols[3]);
                    vel = double.Parse(cols[4]);
                    lap1.Longitudes.Add(lon);
                    lap1.Latitudes.Add(lat);
                    lap1.Altitude.Add(alt);
                    lap1.Velocity.Add(vel);
                    lap2.Longitudes.Add(lon + rand.NextDouble());
                    lap2.Latitudes.Add(lat + rand.NextDouble());
                    lap2.Altitude.Add(alt + rand.NextDouble());
                    lap2.Velocity.Add(vel + rand.NextDouble());
                }
            }

            this.trackName = "Car Drive";
            // add other information
            lap1.LapColor = Colors.Blue;
            lap1.LapNo = "1";
            lap1.LapTime = "5:32";
            lap1.Track = trackName;
            // lap 2 information
            lap2.LapColor = Colors.Orange;
            lap2.LapNo = "2";
            lap1.LapTime = "5:14";
            lap2.Track = trackName;

            this.DateStamp = new DateTime(2013, 1, 1).ToLongDateString();
            
            // add the lap to the record
            this.Laps.Add(lap1);
            this.Laps.Add(lap2);

        }
    }
}
