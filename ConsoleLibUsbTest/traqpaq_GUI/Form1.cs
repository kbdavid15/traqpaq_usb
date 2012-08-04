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
        traqpaqDevice traqpaq;

        public Form1()
        {
            InitializeComponent();
            // once the form is loaded, connect to the USB device
            traqpaq = new traqpaqDevice();
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

        }
    }
}
