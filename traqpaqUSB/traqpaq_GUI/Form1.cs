using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace traqpaq_GUI
{
    public partial class Form1 : Form
    {
        public TraqpaqDevice traqpaq;

        public Form1()
        {
            InitializeComponent();
            // once the form is loaded, connect to the USB device
            this.traqpaq = new TraqpaqDevice();
            populateListView();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Sends command to the device requesting the software version
        /// Updates outLabel with the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void versionButton_Click(object sender, EventArgs e)
        {
            outLabel.Text += traqpaq.myOTPreader.ApplicationVersion;
            //TODO consider scrapping this subclass and make it part of the traqpaqDevice class

            traqpaq.battery.reqBatteryVoltage();
            outLabel.Text += "\nBattery Voltage: " + traqpaq.battery.Voltage;
            traqpaq.battery.reqBatteryTemp();
            outLabel.Text += "\nBattery Temp: " + traqpaq.battery.Temperature;
        }

        private void populateListView()
        {
            ListViewItem lViewItem;
            string[] lViewConstruct;
            foreach (TraqpaqDevice.RecordTableReader.RecordTable table in traqpaq.recordTableList)
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

        private void readButton_Click(object sender, EventArgs e)
        {
            // get the selected list view object
            TraqpaqDevice.RecordTableReader.RecordTable table;
            if (listView1.SelectedItems.Count == 1)
                table = (TraqpaqDevice.RecordTableReader.RecordTable)listView1.SelectedItems[0].Tag;
            else return;
            TraqpaqDevice.RecordDataReader dataReader = new TraqpaqDevice.RecordDataReader(traqpaq, table);
            dataReader.readRecordData();
        }
    }
}
