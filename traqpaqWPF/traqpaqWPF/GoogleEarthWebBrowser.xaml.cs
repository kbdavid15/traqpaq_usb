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
using System.Web.Script.Serialization;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for GoogleEarthWebBrowser.xaml
    /// </summary>
    public partial class GoogleEarthWebBrowser : UserControl
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        public GoogleEarthWebBrowser()
        {
            InitializeComponent();
            //webBrowser.DocumentText = traqpaqResources.ge;
            //webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/ge.html");
            webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/gmaps.htm");
        }

        public void loadKML(string kml)
        {
            // call js function to load kml
            webBrowser.InvokeScript("getKML", new string[] { kml });
        }

        public void addPoints(IEnumerable<double> lats, IEnumerable<double> longs, string color)
        {
            string latitudes = jsSerializer.Serialize(lats);
            string longitudes = jsSerializer.Serialize(longs);
            string[] args = { latitudes, longitudes, color };
            webBrowser.InvokeScript("addPoints", args);
        }

    }
}
