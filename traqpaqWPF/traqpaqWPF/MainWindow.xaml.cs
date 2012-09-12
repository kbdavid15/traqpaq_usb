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
            }
            catch (TraqPaqNotConnectedException e)
            {
                // Device not found
                traqpaq = null;
                IDeviceNotifier deviceNotifier = DeviceNotifier.OpenDeviceNotifier();
                deviceNotifier.OnDeviceNotify += new EventHandler<DeviceNotifyEventArgs>(deviceNotifier_OnDeviceNotify);

            }

            // Create the pages and save to array
            pages = new Page[] { new WelcomePage(this), new RecordTablePage(), new UploadPage(), new DataPage() };

            // Go to the welcome page
            navigatePage(PageName.DATA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deviceNotifier_OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
        {
            //TODO test to see if this handler fires when the device is plugged in
            throw new NotImplementedException();
        }

        public void navigatePage(PageName p)
        {
            frame1.Navigate(pages[(int)p]);
        }
    }
}
