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
using System.Web.Script;

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
            // clear cache
            Q326201CS.DeleteCache.ClearCache();

            //webBrowser.DocumentText = traqpaqResources.ge;
            webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/ge1.htm");
            //webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/gmaps.htm");
        }

        public void loadKML(string kml, string lap)
        {
            // call js function to load kml
            webBrowser.InvokeScript("getKML", new string[] { kml, lap });
        }

        public void addPoints(IEnumerable<double> lats, IEnumerable<double> longs, Color color, string lap)
        {
            string latitudes = jsSerializer.Serialize(lats);
            string longitudes = jsSerializer.Serialize(longs);
            string[] args = { latitudes, longitudes, "#" + color.ToString().Substring(3), lap };
            webBrowser.InvokeScript("addPoints", args);
        }

        public void removeLap(string lap)
        {
            webBrowser.InvokeScript("removeLine", new string[] { lap });
        }

        public void changeColor(string lap, Color color)
        {
            webBrowser.InvokeScript("changeColor", new string[] { lap, "#" + color.ToString().Substring(3) });
        }
    }
}
