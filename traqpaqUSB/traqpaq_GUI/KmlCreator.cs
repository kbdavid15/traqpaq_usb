using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;

namespace traqpaq_GUI
{
    public static class KmlCreator
    {
        public static string getKMLstring(List<double> latitudes,
                                   List<double> longitudes, 
                                   List<double> altitudes = null)
        {
            // make sure the parameters are of the same length
            if (altitudes != null)
            {
                if (!(latitudes.Count == longitudes.Count && altitudes.Count == longitudes.Count))
                {
                    throw new Exception("The length of the parameters must match");
                }
            }
            else
            {
                if (!(latitudes.Count == longitudes.Count))
                {
                    throw new Exception("The length of the parameters must match");
                }
            }

            // Create the KML stuff
            Kml kml = new Kml();
            kml.AddNamespacePrefix("gx", "http://www.google.com/kml/ext/2.2");
            Document doc = new Document();
            Placemark pMark = new Placemark();
            LineString ls = new LineString();
            CoordinateCollection coordCollect = new CoordinateCollection();

            // loop through the lines in the file
            if (altitudes != null)
            {
                for (int i = 0; i < latitudes.Count; i++)
                {
                    coordCollect.Add(new Vector(latitudes[i], longitudes[i], altitudes[i]));
                }
            }
            else // no altitude provided
            {
                for (int i = 0; i < latitudes.Count; i++)
                {
                    coordCollect.Add(new Vector(latitudes[i], longitudes[i]));
                }
            }

            // Add the coordinates to the line string
            ls.Coordinates = coordCollect;

            if (altitudes != null)
            {
                ls.AltitudeMode = AltitudeMode.Absolute;
            }
            ls.Extrude = false;
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
