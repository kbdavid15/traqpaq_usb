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
            StreamWriter f = new StreamWriter(@"C:\Users\Kyle\traqpaq_usb_driver\traqpaqUSB\traqpaq_GUI\test\test.html");
            f.Write(result);
            f.Close();
            f.Dispose();
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
                //writer.AddAttribute(HtmlTextWriterAttribute.Title, "Sample");
                //writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "https://www.google.com/jsapi");
                //writer.RenderEndTag(); // closes the <script> tag

                // main GE code. start new script tag
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.WriteLine("google.load(\"earth\", \"1\");");
                writer.WriteLine("function initCB(instance) {");
                writer.WriteLine("ge = instance;");
                writer.WriteLine("ge.getWindow().setVisibility(true);\n}");
                writer.WriteLine("\nfunction failureCB(errorCode) {\n}\n");
                writer.WriteLine("google.setOnLoadCallback(init);");
                writer.RenderEndTag();  // closes script tag

                writer.RenderEndTag();  // closes head tag

                writer.RenderBeginTag(HtmlTextWriterTag.Body);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "map3d");
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "height: 400px; width: 600px;");
                writer.RenderEndTag();  // close div tag
                writer.RenderEndTag();  // close body tag
                writer.RenderEndTag();  // closes html tag

            }

            return stringWriter.ToString();
        }
    }
}
