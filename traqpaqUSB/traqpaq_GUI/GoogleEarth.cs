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
    public partial class GoogleEarth : Form
    {
        public GoogleEarth()
        {
            InitializeComponent();
            //webBrowser.DocumentText = traqpaqResources.testGE;
            string path = Directory.GetCurrentDirectory() + @"../../..\test\test.html";
            StreamWriter f = new StreamWriter(path); // "../" means up one level from current directory
            string result = loadGE();
            // write to file            
            f.Write(result);
            f.Close();
            f.Dispose();
            // Open in Notepad
            System.Diagnostics.Process.Start("notepad.exe", path);

            webBrowser.DocumentText = result;
        }

        public void plotData(IEnumerable<double> longitudes, IEnumerable<double> latitudes)
        {
            
        }

        /// <summary>
        /// Simulates the output kml from file.
        /// Opens the output.csv file and creates a kml string, which is used in the generated jscript
        /// </summary>
        /// <returns>KML string to be used by parseKML()</returns>
        private IEnumerable<string> getKML()
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
                    coordCollect.Add(new Vector(Convert.ToDouble(s[1]), Convert.ToDouble(s[2])));
                }
                catch { }
            }

            // Add the coordinates to the line string
            ls.Coordinates = coordCollect;

            // Add the line string to the placemark
            pMark.Geometry = ls;

            // Add the placemark to the document
            doc.AddFeature(pMark);

            // Add the document to the kml object
            kml.Feature = doc;

            // Create the KML file
            KmlFile kmlFile = KmlFile.Create(kml, false);

            // Save the KML file
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "KML files (*.kml)|*.kml|All files (*.*)|*.*";
            if (sd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                kmlFile.Save(sd.FileName);
                string[] allLines = File.ReadAllLines(sd.FileName);
                return allLines;
            }
            return null;
        }

        public string loadGE()
        {
            // Create an html file that loads Google Earth
            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);

                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.Write("Sample");
                writer.RenderEndTag();  // End title tag
                
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "https://www.google.com/jsapi");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.RenderEndTag();  // closes first script tag
                
                // main GE code. start new script tag
                writer.AddAttribute("type", "text/javascript");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.WriteLine("var ge");
                writer.WriteLine("google.load(\"earth\", \"1\");");

                writer.WriteLine("function init() {");
                writer.WriteLine("\tgoogle.earth.createInstance('map3d', initCB, failureCB);");
                writer.WriteLine("}");
                writer.WriteLine();

                // failure callback function
                writer.WriteLine("function failureCB(errorCode) { }");
                writer.WriteLine();

                // initialize callback function
                writer.WriteLine("function initCB(instance) {");
                writer.WriteLine("\tge = instance;");
                writer.WriteLine("\tge.getWindow().setVisibility(true);");
                // add a navigation control
                writer.WriteLine("\tge.getNavigationControl().setVisibility(ge.VISIBILITY_AUTO);");
                // add some layers
                writer.WriteLine("\tge.getLayerRoot().enableLayerById(ge.LAYER_BORDERS, true);");
                writer.WriteLine("\tge.getLayerRoot().enableLayerById(ge.LAYER_ROADS, true);");
                // finished function (called after google earth loads)
                writer.WriteLine("\tfunction finished(object) {");
                writer.WriteLine("\t\tif (!object) {");
                // wrap alerts in API callbacks and event handlers
                // in a setTimeout to prevent deadlock in some browsers
                writer.WriteLine("\t\t\tsetTimeout(function() {");
                writer.WriteLine("\t\t\t\talert('Bad or null KML.');");
                writer.WriteLine("\t\t\t}, 0);");
                writer.WriteLine("\t\t\treturn;");
                writer.WriteLine("\t\t}");    // closes the if statement
                writer.WriteLine();
                writer.WriteLine("\t\tge.getFeatures().appendChild(object);");
                writer.WriteLine("\t\tvar la = ge.createLookAt('');");
                writer.WriteLine("\t\tla.set(37.77976, -122.418307, 25, ge.ALTITUDE_RELATIVE_TO_GROUND,180, 60, 500);");
                writer.WriteLine("\t\tge.getView().setAbstractView(la);");
                writer.WriteLine("\t}");  // closes the finished function
                writer.WriteLine();
                // fetch the KML
                //writer.WriteLine("\tvar url = 'http://sketchup.google.com/' + '3dwarehouse/download?mid=28b27372e2016cca82bddec656c63017&rtyp=k2';");
                //writer.WriteLine("\tgoogle.earth.fetchKml(ge, url, finished);");
                //Parse KML
                List<string> kmlFile = getKML().ToList();
                writer.WriteLine("\tvar kmlString = ''");
                // add each line to the file
                foreach (string s in kmlFile)
                {
                    string writeIt = "\t\t+ '" + s + "'";
                    if (kmlFile.LastIndexOf(s) == kmlFile.Count - 1)
                    {
                        writeIt += ";";
                    }
                    writer.WriteLine(writeIt);
                }
                writer.WriteLine("\tvar kmlObject = ge.parseKml(kmlString);");
                writer.WriteLine("\tfinished(kmlObject);");
                writer.WriteLine("}");  // closes the initCB function
                writer.WriteLine();

                writer.RenderEndTag();  // closes script tag
                writer.RenderEndTag();  // closes head tag

                // HTML body
                writer.AddAttribute("onload"    , "init()");
                writer.RenderBeginTag(HtmlTextWriterTag.Body);                
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "map3d");
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 400px; width: 600px;");   // not specifying the size allows it to fit to window
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();  // close div tag
                writer.RenderEndTag();  // close body tag
                writer.RenderEndTag();  // closes html tag
            }

            return stringWriter.ToString();
        }
    }
}
