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
            webBrowser.Url = new Uri(@"C:\Users\Kyle\Documents\GitHub\traqpaq_usb_driver\traqpaqUSB\traqpaq_GUI\test\testGE.html");
            loadGE();
        }

        public void plotData(IEnumerable<double> longitudes, IEnumerable<double> latitudes)
        {
            
        }

        public void loadGE()
        {
            // create a new html file that loads google earth
            string name = "testGE.html";

            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                //writer.AddAttribute(HtmlTextWriterAttribute.Title, "Sample");
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "https://www.google.com/jsapi");
                writer.RenderEndTag(); // closes the <script> tag

                // main GE code. start new script tag
                writer.RenderBeginTag(HtmlTextWriterTag.Script);
                writer.WriteLine("google.load(\"earth\", \"1\");");
                writer.WriteLine("function initCB(instance) {");
                writer.WriteLine("ge = instance;");
                writer.WriteLine("ge.getWindow().setVisibility(true);\n}");

                writer.RenderEndTag();  // closes head tag
                writer.RenderEndTag();  // closes html tag
                
            }
            MessageBox.Show(stringWriter.ToString());
        }
    }
}
