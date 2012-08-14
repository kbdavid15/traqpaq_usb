﻿using System;
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
            //outLabel.Text += " " + traqpaq.get_sw_version();
            outLabel.Text += "" + traqpaq.reqApplicationVersion().ToString();
            outLabel.Text += "\nHardware Version: " + traqpaq.reqHardwareVersion().ToString();
            outLabel.Text += "\nSerial Number: " + traqpaq.reqSerialNumber().ToString();
            outLabel.Text += "\nTester ID: " + traqpaq.reqTesterID().ToString();
            traqpaq.battery.reqBatteryVoltage();
            outLabel.Text += "\nBattery Voltage: " + traqpaq.battery.Voltage;
            traqpaq.battery.reqBatteryTemp();
            outLabel.Text += "\nBattery Temp: " + traqpaq.battery.Temperature;
        }

        private void readRecordTableButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
