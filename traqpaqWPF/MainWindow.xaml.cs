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
using System.Net;
using System.Collections.Specialized;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window    //TODO implement IDisposable
    {
        public TraqpaqDevice traqpaq;
        /// <summary>
        /// Used to detect when a device is connected, 
        /// if it is not connected already when the program is started.
        /// </summary>
        public IDeviceNotifier deviceNotifier;
        /// <summary>
        /// Background Worker used to add records to the Log Book Page without freezing the GUI
        /// </summary>
        //public BackgroundWorker bw = new BackgroundWorker();

        // Declare the log page and data page here so that there are not multiple copies of the same page
        public LogBookPage logBookPage;
        public DataPage dataPage;
        public HomePage homePage;
        public SettingsPage settingsPage;
        public UploadPage uploadPage;

        //public delegate void Populate();

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

            // Create the pages
            homePage = new HomePage(this);
            settingsPage = new SettingsPage(this);
            uploadPage = new UploadPage(this);

            // assign the pages to the respective tabs
            frameUpload.Navigate(uploadPage);
            frameSettings.Navigate(settingsPage);

            // Go to the welcome page
            frameHome.Navigate(homePage);

            // create the log book page
            logBookPage = new LogBookPage(this);
            frameLogBook.Navigate(logBookPage);
            if (traqpaq != null)
            {
                logBookPage.populateTracks();
            }

            // configure the background worker
            //bw.DoWork += bw_DoWork;
            //bw.WorkerReportsProgress = false;
            // attempt with backgroundworker if connected
            //if (traqpaq != null)
            //{
            //    logBookPage.populateTracks();
            //    //bw.RunWorkerAsync();
            //}


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

        //void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    MessageBox.Show("Complete");
        //}

        //void bw_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Populate handler = logBookPage.populateTracks;
        //    logBookPage.Dispatcher.BeginInvoke(handler);
        //}

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
                            logBookPage.populateTracks();                            
                        }
                        catch (TraqPaqNotConnectedException) { return; }    // Silently fail and exit method

                        //BackgroundWorker bw = new BackgroundWorker();
                        //bw.DoWork += bw_DoWork;
                        //bw.RunWorkerAsync(); 
                    }
                }
                else    // device removal
                {
                    traqpaq.disconnectDevice();
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
            LoginWindow login = new LoginWindow(this);
            // add event handler for login successful
            login.Closed += login_Closed;

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

        void login_Closed(object sender, EventArgs e)
        {
            // if login successful
            syncUserRecords();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void syncUserRecords()
        {
            // this is necessary because the server is using a self-signed certificate
            // In production, we will pay for a cert issued by a CA and will not require this line.
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            WebClient webClient = new WebClient();

            // get listing of data from server
            byte[] response = await webClient.UploadValuesTaskAsync(new Uri("https://redline-testing.com/upload.php"), new NameValueCollection()
            {
                { "sync", "server" }    // value of "server" tells the server to return a listing of all records stored for user
            });
            string s = webClient.Encoding.GetString(response);
            MessageBox.Show(s);

            // release the web client
            webClient.Dispose();
        }

        //TODO remove this once SSL cert is purchased
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}