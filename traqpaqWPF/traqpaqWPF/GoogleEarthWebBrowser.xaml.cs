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

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for GoogleEarthWebBrowser.xaml
    /// </summary>
    public partial class GoogleEarthWebBrowser : UserControl
    {
        public GoogleEarthWebBrowser()
        {
            InitializeComponent();
            //webBrowser.DocumentText = traqpaqResources.ge;
            webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/ge.html");
        }

        public void loadKML(string kml)
        {
            // call js function to load kml
            webBrowser.InvokeScript("getKML", new string[] { kml });
        }
    }
}
