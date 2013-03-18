using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Web.Script.Serialization;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for GoogleMapsWebBrowser.xaml
    /// </summary>
    public partial class GoogleMapsWebBrowser : UserControl
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        
        public GoogleMapsWebBrowser()
        {
            InitializeComponent();
            //TODO clear cache is only for debugging, should be removed eventually
            //Q326201CS.DeleteCache.ClearCache();

            //webBrowser.DocumentText = traqpaqResources.ge;
            //webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/ge1.htm");
            webBrowser.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/gmaps.html");
        }

        public void loadKML(string kml, string lap)
        {
            // call js function to load kml
            webBrowser.InvokeScript("getKML", new string[] { kml, lap });
        }

        public void addPoints(IEnumerable<double> lats, IEnumerable<double> longs, Color color, string track, string lap)
        {
            string latitudes = jsSerializer.Serialize(lats);
            string longitudes = jsSerializer.Serialize(longs);
            string[] args = { latitudes, longitudes, "#" + color.ToString().Substring(3), (track + "-" + lap) };
            webBrowser.InvokeScript("addPoints", args);
        }

        public void removeLap(string track, string lap)
        {
            webBrowser.InvokeScript("removeLine", new string[] { (track + "-" + lap) });
        }

        public void changeColor(string track, string lap, Color color)
        {
            webBrowser.InvokeScript("changeColor", new string[] { (track + "-" + lap), "#" + color.ToString().Substring(3) });
        }

        public void clearLaps()
        {
            webBrowser.InvokeScript("clearPolyArray");
        }
    }
}
