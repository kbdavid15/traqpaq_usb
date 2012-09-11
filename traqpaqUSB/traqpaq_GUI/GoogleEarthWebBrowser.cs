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
            //this.DocumentText = traqpaqResources.ge;
            this.Navigate("http://www.redline-electronics.com/traqpaq/GoogleEarth/ge.html");
            this.Dock = DockStyle.Fill;
            this.ScrollBarsEnabled = false;
        }

        public void loadKML(string kml)
        {
            // call js function to load kml
            this.Document.InvokeScript("getKML", new string[] { kml });
        }
    }
}