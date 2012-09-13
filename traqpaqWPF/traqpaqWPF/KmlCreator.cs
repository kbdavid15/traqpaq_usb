using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System.Windows.Media;

namespace traqpaqWPF
{
    public static class KmlCreator
    {
        public static string getKMLstring(Color        lapColor,
                                          List<double> latitudes,
                                          List<double> longitudes, 
                                          List<double> altitudes = null)
        {
            // make sure the parameters are of the same length
            if (((altitudes != null) && !(latitudes.Count == longitudes.Count && altitudes.Count == longitudes.Count)) ||
                !(latitudes.Count == longitudes.Count))
            {
                throw new Exception("The length of the parameters must match");
            }

            // Create the KML stuff
            Kml kml = new Kml();
            kml.AddNamespacePrefix("gx", "http://www.google.com/kml/ext/2.2");
            Document doc = new Document();
            Style style = new Style();
            Placemark pMark = new Placemark();
            LineString ls = new LineString();
            ls.Coordinates = new CoordinateCollection();

            // Define the style
            style.Line = new LineStyle();
            style.Id = "redline";
            style.Line.Color = new Color32(lapColor.A, lapColor.B, lapColor.G, lapColor.R);
            style.Line.ColorMode = ColorMode.Random;
            style.Line.Width = 2;
            
            // add style to placemark
            pMark.StyleUrl = new Uri("#redline", UriKind.Relative);

            // loop through the lines in the file
            if (altitudes != null)
            {
                ls.AltitudeMode = AltitudeMode.Absolute;
                for (int i = 0; i < latitudes.Count; i++)
                {
                    ls.Coordinates.Add(new Vector(latitudes[i], longitudes[i], altitudes[i]));
                }
            }
            else // no altitude provided
            {
                for (int i = 0; i < latitudes.Count; i++)
                {
                    ls.Coordinates.Add(new Vector(latitudes[i], longitudes[i]));
                }
            }

            // Set properties on the line string
            ls.Extrude = false;
            ls.Tessellate = true;

            // Add the line string to the placemark
            pMark.Geometry = ls;

            // Generate a LookAt object to center the view on the placemark
            doc.Viewpoint = pMark.CalculateLookAt();

            // Add the placemark and style to the document
            doc.AddFeature(pMark);
            doc.AddStyle(style);

            // Add the document to the kml object
            kml.Feature = doc;

            // Create the KML file
            KmlFile kmlFile = KmlFile.Create(kml, false);

            //TODO for debugging purposes, save the kml file to test folder.
            //kmlFile.Save("test.kml");

            // Return the KML file as a string
            return kmlFile.SaveString();
        }
    }
}
