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
            //webBrowser.Url = new Uri(traqpaqResources.testGE);
            //webBrowser.DocumentText = traqpaqResources.testGE;
            string result = loadGE();
            // write to file
            StreamWriter f = new StreamWriter(@"../..\test\test.html");
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

                // initialize callback function
                writer.WriteLine("function initCB(instance) {");
                writer.WriteLine("\tge = instance;");
                writer.WriteLine("\tge.getWindow().setVisibility(true);");
                // add a navigation control
                writer.WriteLine("ge.getNavigationControl().setVisibility(ge.VISIBILITY_AUTO);");
                // add some layers
                writer.WriteLine("ge.getLayerRoot().enableLayerById(ge.LAYER_BORDERS, true);");
                writer.WriteLine("ge.getLayerRoot().enableLayerById(ge.LAYER_ROADS, true);");
                writer.WriteLine("}");

                // failure callback function
                writer.WriteLine("function failureCB(errorCode) {\n}\n");
                writer.WriteLine("google.setOnLoadCallback(init);");
                writer.RenderEndTag();  // closes script tag

                writer.RenderEndTag();  // closes head tag

                writer.RenderBeginTag(HtmlTextWriterTag.Body);                
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "map3d");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 400px; width: 600px;");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderEndTag();  // close div tag
                writer.RenderEndTag();  // close body tag
                writer.RenderEndTag();  // closes html tag

            }

            return stringWriter.ToString();
        }
    }
}
