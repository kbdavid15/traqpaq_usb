using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Windows.Interop;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace traqpaq_GUI
{
    class GoogleEarthWebBrowser : WebBrowser
    {
        public GoogleEarthWebBrowser()
        {
            this.DocumentText = traqpaqResources.ge_1;
            this.Dock = DockStyle.Fill;
            this.ScrollBarsEnabled = false;
        }

        public void Start()
        {
            // call js function to load kml
            this.Document.InvokeScript("getKML", new string[] { getKMLstring() });
            //TODO long term: host html on redline-electronics.com and then use this.Document.InvokeScript() to call a function that loads the KML file
        }

        public void loadKML(string kml)
        {
            // call js function to load kml
            this.Document.InvokeScript("getKML", new string[] { kml });
        }

        private string getKMLstring()
        {
            string[] lines;
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Comma Separated Value (*.csv)|*.csv";

            if (fd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                lines = File.ReadAllLines(fd.FileName);
            else return null;

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
                    coordCollect.Add(new Vector(Convert.ToDouble(s[1]), Convert.ToDouble(s[2]), Convert.ToDouble(s[4])));
                }
                catch { }
            }

            // Add the coordinates to the line string
            ls.Coordinates = coordCollect;
            ls.AltitudeMode = AltitudeMode.Absolute;
            ls.Extrude = true;
            ls.Tessellate = true;

            // Add the line string to the placemark
            pMark.Geometry = ls;

            // Generate a LookAt object to center the view on the placemark
            LookAt lookAt = pMark.CalculateLookAt();
            doc.Viewpoint = lookAt;

            // Add the placemark to the document
            doc.AddFeature(pMark);

            // Add the document to the kml object
            kml.Feature = doc;

            // Create the KML file
            KmlFile kmlFile = KmlFile.Create(kml, false);

            // Return the KML file as a string
            return kmlFile.SaveString();
        }
    }
}