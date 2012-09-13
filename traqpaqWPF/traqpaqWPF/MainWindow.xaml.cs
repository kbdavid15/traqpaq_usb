using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using LibUsbDotNet.Info;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.DeviceNotify.Info;

namespace traqpaqWPF
{
    public enum PageName { WELCOME, RECORDS, UPLOAD, DATA };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Page[] pages;
        public TraqpaqDevice traqpaq;
        /// <summary>
        /// Used to detect when a device is connected, 
        /// if it is not connected already when the program is started.
        /// </summary>
        public IDeviceNotifier deviceNotifier;
        /// <summary>
        /// Use this list to save the previous pages that the user has visited.
        /// </summary>
        List<Page> backPageCache = new List<Page>();
        /// <summary>
        /// If the user hits the back button, save the page to this list
        /// </summary>
        List<Page> forwardPageCache = new List<Page>();

        public MainWindow()
        {
            InitializeComponent();
            
            // try to connect to device. Show status in status bar
            try
            {
                traqpaq = new TraqpaqDevice();
                // update status bar
                statusBarItemTraqpaq.Content = "Device connected: " + traqpaq.myOTPreader.SerialNumber;
            }
            catch (TraqPaqNotConnectedException)
            {
                // Device not found
                traqpaq = null;
                // update status bar
                statusBarItemTraqpaq.Content = "Device not found";
                // Set up event handler to wait for a usb device to connect to
                deviceNotifier = DeviceNotifier.OpenDeviceNotifier();
                deviceNotifier.OnDeviceNotify += new EventHandler<DeviceNotifyEventArgs>(deviceNotifier_OnDeviceNotify);
            }

            // Create the pages and save to array
            pages = new Page[] { new WelcomePage(this), new RecordTablePage(), new UploadPage(), new DataPage(this) };

            // Go to the welcome page
            navigatePage(PageName.WELCOME);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deviceNotifier_OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
        {
            // Detected a device, try to see if it is the traqpaq
            if (e.EventType == EventType.DeviceArrival)  // check for device arrival
            {
                if (traqpaq == null)
                {//TODO could also check for specifics with the e.Device..... properties
                    // try to connect again
                    try
                    {
                        traqpaq = new TraqpaqDevice();
                    }
                    catch (TraqPaqNotConnectedException) { }    // Silently fail
                }
            }
            else
            {
                //TODO use this to disconnect the device. The event handler would need to be created regardless
                //of wether or not the device is connected at first though.
            }
        }

        public void navigatePage(PageName p)
        {
            frame1.Navigate(pages[(int)p]);
        }        
    }
}
