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
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

namespace traqpaqWPF
{
    public enum PageName { WELCOME, IMPORT, SETTINGS, DATA, RECORDS };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Page> pages;
        public TraqpaqDevice traqpaq;
        /// <summary>
        /// Used to detect when a device is connected, 
        /// if it is not connected already when the program is started.
        /// </summary>
        public IDeviceNotifier deviceNotifier;
        /// <summary>
        /// 
        /// </summary>
        public BackgroundWorker bw = new BackgroundWorker();

        // Declare the record page, so that there are not multiple copies of the same page
        public LogBookPage logBookPage;
        public DataPage dataPage;

        public delegate void Populate();

        public MainWindow()
        {
            InitializeComponent();

            // Set up event handler to wait for a usb device to connect to
            deviceNotifier = DeviceNotifier.OpenDeviceNotifier();
            deviceNotifier.OnDeviceNotify += new EventHandler<DeviceNotifyEventArgs>(deviceNotifier_OnDeviceNotify);
            
            // try to connect to device (if device was connected previously). Show status in status bar
            try
            {
                traqpaq = new TraqpaqDevice();
                // update status bar
                statusBarItemTraqpaq.Content = "Device connected: " + traqpaq.reqSerialNumber();
            }
            catch (TraqPaqNotConnectedException)
            {
                // Device not found
                traqpaq = null;
                // update status bar
                statusBarItemTraqpaq.Content = "Device not found";

            }

            // Create the pages and save to array
            pages = new List<Page>() { new HomePage(this), new UploadPage(this), new SettingsPage(this) };

            // assign the pages to the respective tabs
            frameUpload.Navigate(pages[(int)PageName.IMPORT]);
            frameSettings.Navigate(pages[(int)PageName.SETTINGS]);

            // Go to the welcome page
            frameHome.Navigate(pages[(int)PageName.WELCOME]);
            
            // create the log book page
            logBookPage = new LogBookPage(this);
            frameLogBook.Navigate(logBookPage);
            //logBookPage.populateTracks();

            // configure the background worker
            bw.DoWork += bw_DoWork;
            bw.WorkerReportsProgress = false;
            // attempt with backgroundworker if connected
            if (traqpaq != null)
            {
                bw.RunWorkerAsync(); 
            }

            //// create the log book page as a background task
            //Thread threadCreateRecordPage = new System.Threading.Thread(
            //    new ThreadStart(
            //        delegate()
            //        {
            //            System.Windows.Threading.DispatcherOperation dispatcherOp = frameLogBook.Dispatcher.BeginInvoke(
            //                System.Windows.Threading.DispatcherPriority.Normal,
            //                new Action(delegate()
            //                {
            //                    logBookPage.populateTracks();                                
            //                }));
            //        }));
            //threadCreateRecordPage.SetApartmentState(ApartmentState.STA);
            //threadCreateRecordPage.Start();

            //Action createRecordPage = delegate()
            //{
            //    logBookPage.populateTracks();
            //};
            //Task taskLogPage = Task.Factory.StartNew(createRecordPage);

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Complete");
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Populate handler = logBookPage.populateTracks;
            logBookPage.Dispatcher.BeginInvoke(handler);
            //logBookPage.populateTracks();
        }

        /// <summary>
        /// Called whenever a usb device is plugged in or unplugged
        /// </summary>
        void deviceNotifier_OnDeviceNotify(object sender, DeviceNotifyEventArgs e)
        {
            // Detected a device, try to see if it is the traqpaq
            if (e.Device.IdProduct == Constants.PID && e.Device.IdVendor == Constants.VID)
            {
                if (e.EventType == EventType.DeviceArrival)  // check for device arrival
                {
                    if (traqpaq == null)
                    {
                        // try to connect again
                        try
                        {
                            traqpaq = new TraqpaqDevice();
                            statusBarItemTraqpaq.Content = "Device connected: " + traqpaq.reqSerialNumber();
                            // populate tracks
                            //TODO fix populate tracks
                            
                        }
                        catch (TraqPaqNotConnectedException) { }    // Silently fail

                        //logBookPage.populateTracks();
                        //BackgroundWorker bw = new BackgroundWorker();
                        //bw.DoWork += bw_DoWork;
                        bw.RunWorkerAsync(); 
                    }
                }
                else    // device removal
                {
                    traqpaq.MyUSBDevice.Close();
                    traqpaq = null;
                    // update status bar
                    statusBarItemTraqpaq.Content = "Traqpaq disconnected";
                }
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (frameLogBook.CanGoBack)
            {
                frameLogBook.GoBack();
                if (!frameLogBook.CanGoBack)
                {   // if can't go back anymore, hide the back button
                    buttonBack.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Show the login window and let the user attempt to login
        /// </summary>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            if (login.ShowDialog() == true)
            {
                Label username = new Label();
                username.Content = (string)login.Tag;
                stackPanelLogin.Children.Add(username);
                // change the login button to be the logout button
                buttonLogin.Visibility = System.Windows.Visibility.Hidden;
                buttonLogout.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>
        /// Button is only visible if the user is logged in.
        /// If clicked, it logs the user out and shows the login button
        /// </summary>
        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            stackPanelLogin.Children.Clear();
            // revert to the login button
            buttonLogin.Visibility = System.Windows.Visibility.Visible;
            buttonLogout.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}