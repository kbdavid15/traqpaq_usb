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

namespace traqpaq_GUI
{
    public partial class GoogleEarth : Form
    {
        public GoogleEarth()
        {
            InitializeComponent();
            //webBrowser.DocumentText = traqpaqResources.testGE;
            string result = loadGE();
            // write to file
            StreamWriter f = new StreamWriter(@"../..\test\test.html"); // "../" means up one level from current directory
            f.Write(result);
            f.Close();
            f.Dispose();
            // Open in Notepad
            System.Diagnostics.Process.Start("notepad.exe", @"../..\test\test.html");

            webBrowser.DocumentText = loadGE();
        }

        public void plotData(IEnumerable<double> longitudes, IEnumerable<double> latitudes)
        {
            
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
                writer.WriteLine("\tvar url = 'http://sketchup.google.com/' + '3dwarehouse/download?mid=28b27372e2016cca82bddec656c63017&rtyp=k2';");
                writer.WriteLine("\tgoogle.earth.fetchKml(ge, url, finished);");
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
