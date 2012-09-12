﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace traqpaq_GUI
{
    public partial class traqpaqForm : Form
    {
        public TraqpaqDevice traqpaq;
        public double[] Latitudes { get; set; }
        public double[] Longitudes { get; set; }
        public int CoordinateIndex = 0;
        GoogleEarthWebBrowser ge;
        
        public traqpaqForm()
        {
            InitializeComponent();
            // once the form is loaded, connect to the USB device
            try
            {
                this.traqpaq = new TraqpaqDevice();
                populateListView();
            }
            catch { /*MessageBox.Show("Device not found.", "Error");*/ }

            // Set up the Google Earth stuff
            ge = new GoogleEarthWebBrowser();
            tabPageGoogleEarth.Controls.Add(ge);
        }

        private void populateListView()
        {
            ListViewItem lViewItem;
            string[] lViewConstruct;
            foreach (var table in traqpaq.recordTableList)
            {
                lViewConstruct = new string[] { table.TrackID.ToString(),
                    traqpaq.trackList[table.TrackID].trackName, traqpaq.recordTableList.IndexOf(table).ToString(), 
                    table.DateStamp.ToString(), table.StartAddress.ToString(), table.EndAddress.ToString(),
                    traqpaq.trackList[table.TrackID].Latitude.ToString(), traqpaq.trackList[table.TrackID].Longitute.ToString() };
                lViewItem = new ListViewItem(lViewConstruct);
                lViewItem.Tag = table;
                listView1.Items.Add(lViewItem);
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string output = "";
            output += traqpaq.myOTPreader.ApplicationVersion;
            //TODO consider scrapping this subclass and make it part of the traqpaqDevice class
            traqpaq.battery.reqBatteryVoltage();
            output += "\nBattery Voltage: " + traqpaq.battery.Voltage;
            traqpaq.battery.reqBatteryTemp();
            output += "\nBattery Temp: " + traqpaq.battery.Temperature;
            MessageBox.Show(output);
        }

        private void buttonOutputCSV_Click(object sender, EventArgs e)
        {
            // get the selected list view object
            TraqpaqDevice.RecordTableReader.RecordTable table;
            if (listView1.SelectedItems.Count == 1)
                table = (TraqpaqDevice.RecordTableReader.RecordTable)listView1.SelectedItems[0].Tag;
            else return;
            TraqpaqDevice.RecordDataReader dataReader = new TraqpaqDevice.RecordDataReader(traqpaq, table);
            dataReader.readRecordData();

            // dump data to text file
            using (StreamWriter file = new StreamWriter(@"C:\Users\Kyle\Documents\GitHub\traqpaq_usb_driver\output.csv"))
            {
                file.WriteLine("Time (UTC),Latitude,Longitude,Lap Detected,Altitude (m),Speed (m/s),Course (deg),HDOP,Current Mode,Satellites");
                int lapdetect;
                foreach (var page in dataReader.recordDataPages)
                {
                    foreach (var data in page.RecordData)
                    {
                        if (data.lapDetected) lapdetect = 1;
                        else lapdetect = 0;
                        file.WriteLine(page.utc + "," + data.Latitude + "," + data.Longitude + "," + lapdetect + "," + data.Altitude + "," + data.Speed + "," + data.Heading + "," + page.hdop + "," + page.GPSmode + "," + page.Satellites);
                    }
                }
            }
        }

        /// <summary>
        /// Write the coordinates to a KML file that can be used by Google Earth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOutputKML_Click(object sender, EventArgs e)
        {
            // get the selected list view object
            TraqpaqDevice.RecordTableReader.RecordTable table;
            if (listView1.SelectedItems.Count == 1)
                table = (TraqpaqDevice.RecordTableReader.RecordTable)listView1.SelectedItems[0].Tag;
            else return;
            TraqpaqDevice.RecordDataReader dataReader = new TraqpaqDevice.RecordDataReader(traqpaq, table);
            dataReader.readRecordData();

            // create a new line string to store the coordinates            
            LineString ls = new LineString();
            CoordinateCollection coordinates = new CoordinateCollection();

            // add coordinates to the collection;                
            foreach (TraqpaqDevice.RecordDataReader.RecordDataPage page in dataReader.recordDataPages)
                foreach (TraqpaqDevice.RecordDataReader.RecordDataPage.tRecordData data in page.RecordData)
                    coordinates.Add(new Vector(data.Latitude, data.Longitude));

            // add the coordinates to the line string
            ls.Coordinates = coordinates;
            ls.Tessellate = true;

            // save the KML file
            KmlFile kml = KmlFile.Create(ls, false);
            if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                kml.Save(saveFileDialog1.FileName);
        }

        /// <summary>
        /// Shows a file dialog, opens a file, reads latitude and longitude, plots on chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlotText_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                List<double> latitudes = new List<double>();
                List<double> longitudes = new List<double>();
                string[] lines = File.ReadAllLines(openFileDialog1.FileName);
                foreach (string line in lines)
                {
                    string[] s = line.Split(',');
                    try
                    {
                        latitudes.Add(Convert.ToDouble(s[1]));
                        longitudes.Add(Convert.ToDouble(s[2]));
                    }
                    catch { }
                }

                this.Latitudes = latitudes.ToArray();
                this.Longitudes = longitudes.ToArray();

                // plot the points on the chart
                chart1.Series.Clear();
                chart1.Series.Add(new Series("Coordinates"));
                chart1.Series[0].ChartType = SeriesChartType.Point;
                // add the points to the chart when the timer ticks
                System.Timers.Timer timer = new System.Timers.Timer(5);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.SynchronizingObject = this;
                timer.Start();
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.CoordinateIndex < this.Latitudes.Length)
            {
                chart1.Series[0].Points.Add(new DataPoint(Latitudes[CoordinateIndex], Longitudes[CoordinateIndex]));
                CoordinateIndex+=10;
            }
            else
            {
                System.Timers.Timer timer = sender as System.Timers.Timer;
                timer.Stop();
                timer.Dispose();
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

        /// <summary>
        /// Opens the output.csv file, reads in the data points, and creates a KML file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOutputKMLfromFile_Click(object sender, EventArgs e)
        {
            string[] lines;
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Comma Separated Value (*.csv)|*.csv";

                if (fd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    lines = File.ReadAllLines(fd.FileName);
                else return; 
            }

            // Create the KML stuff
            Kml kml = new Kml();
            kml.AddNamespacePrefix("gx", "http://www.google.com/kml/ext/2.2");
            Document doc = new Document();
            Placemark pMark = new Placemark();
            LineString ls = new LineString();
            CoordinateCollection coordCollect = new CoordinateCollection();

            // loop through the lines in the file
            foreach (string line in lines)
            {
                // skip first line if header
                string[] s = line.Split(',');
                try
                {
                    coordCollect.Add(new Vector(Convert.ToDouble(s[1]), Convert.ToDouble(s[2])));
                }
                catch { }
            }

            // Add the coordinates to the line string
            ls.Coordinates = coordCollect;

            // Add the line string to the placemark
            pMark.Geometry = ls;

            // Add the placemark to the document
            doc.AddFeature(pMark);
                        
            // Add the document to the kml object
            kml.Feature = doc;

            // Create the KML file
            KmlFile kmlFile = KmlFile.Create(kml, false);
            
            // Save the KML file
            using (SaveFileDialog sd = new SaveFileDialog())
            {
                sd.Filter = "KML files (*.kml)|*.kml|All files (*.*)|*.*";
                if (sd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    kmlFile.Save(sd.FileName); 
            }
        }

        /// <summary>
        /// Read the record data and plot on the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlot_Click(object sender, EventArgs e)
        {
            // get the selected list view object
            TraqpaqDevice.RecordTableReader.RecordTable table;
            if (listView1.SelectedItems.Count == 1)
                table = (TraqpaqDevice.RecordTableReader.RecordTable)listView1.SelectedItems[0].Tag;
            else return;
            TraqpaqDevice.RecordDataReader dataReader = new TraqpaqDevice.RecordDataReader(traqpaq, table);
            dataReader.readRecordData();
            List<double> latitudes = new List<double>();
            List<double> longitudes = new List<double>();

            foreach (TraqpaqDevice.RecordDataReader.RecordDataPage page in dataReader.recordDataPages)
                foreach (TraqpaqDevice.RecordDataReader.RecordDataPage.tRecordData data in page.RecordData)
                {
                    latitudes.Add(data.Latitude);
                    longitudes.Add(data.Longitude);
                }
            // normalize the points to plot on the chart
            double[] normLatitudes = NormalizeData(latitudes, 0, 100);
            double[] normLongitudes = NormalizeData(longitudes, 0, 100);
            this.Latitudes = normLatitudes;
            this.Longitudes = normLongitudes;

            // plot the points on the chart
            chart1.Series.Clear();
            chart1.Series.Add(new Series("Coordinates"));
            chart1.Series[0].ChartType = SeriesChartType.Point;
            // add the points to the chart when the timer ticks
            System.Timers.Timer timer = new System.Timers.Timer(5);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.SynchronizingObject = this;
            timer.Start();
        }

        /// <summary>
        /// Load the google earth form, sending it the data to plot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGoogleEarth_Click(object sender, EventArgs e)
        {
            string[] lines;
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = "Comma Separated Value (*.csv)|*.csv";

                if (fd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    lines = File.ReadAllLines(fd.FileName);
                else return;
            }

            List<double> latitudes = new List<double>();    
            List<double> longitudes = new List<double>();
            List<double> altitudes = new List<double>();

            // loop through the lines in the file
            foreach (string line in lines)
            {
                // skip first line if header
                string[] s = line.Split(',');
                if (lines[0] != line)
                {
                    latitudes.Add(double.Parse(s[1]));
                    longitudes.Add(double.Parse(s[2]));
                    altitudes.Add(double.Parse(s[4]));
                }
            }
            ge.loadKML(KmlCreator.getKMLstring(latitudes,longitudes,altitudes));            

        }

        /// <summary>
        /// Plot the gps coordinates that are contained in the track files from Dad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlotGPS_Click(object sender, EventArgs e)
        {
            // show open file dialog to find the file (just csv for now)
            //TODO look into other gps file formats           
            using (OpenFileDialog oFd = new OpenFileDialog())
            {
                if (oFd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    // read file into array
                    string[] file;
                    try
                    {
                        file = File.ReadAllLines(oFd.FileName);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    List<double> latitudes = new List<double>();
                    List<double> longitudes = new List<double>();
                    for (int i = 0; i < file.Length; i++)
                    {
                        if (i > 42)
                        {
                            string[] s = file[i].Split(',');
                            latitudes.Add(double.Parse(s[2]));
                            longitudes.Add(double.Parse(s[3]));
                        }
                    }

                    // create kml string
                    string kml = KmlCreator.getKMLstring(latitudes, longitudes);

                    // show on Google Earth
                    ge.loadKML(kml);
                } 
            }
        }
    }
}
