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
    public partial class GoogleEarth : Form
    {
        public GoogleEarth()
        {
            InitializeComponent();
            webBrowser.Url = new Uri(@"http://www.github.com");
        }

        public void plotData(IEnumerable<double> longitudes, IEnumerable<double> latitudes)
        {

        }
    }
}
