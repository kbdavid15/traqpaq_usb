using Facebook;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using System.Net;
using System.Collections.Specialized;
using System.Windows.Media.Animation;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        //const string _facebookAppId = "345961062172188";
        //string _permissions = ""; //"user_about_me,read_stream,publish_stream"; // Set your permissions here
        //FacebookClient _fb = new FacebookClient();

        WebClient webClient = new WebClient();

        /// <summary>
        /// key is username, value is password
        /// </summary>
        Dictionary<string, string> loginDictionary = new Dictionary<string, string>();  //TODO this is obviously not the correct way to store login credentials. Need to use a db

        MySqlConnection sqlConnection;
        string server = "192.168.1.3";
        string database = "redline";
        string uid = "redline";
        string password = "9mTKBfN2NdQjpPfq";
        string connectionString;

        public LoginWindow()
        {
            InitializeComponent();

            // load the login web page (facebook, google, twitter)
            loginBrowser.Navigate(new Uri("http://www.traqpaq.com/facebook/loginpage.html"));


            // connect to the database
            //connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            //sqlConnection = new MySqlConnection(connectionString);
            //try
            //{
            //    sqlConnection.Open();
            //}
            //catch (MySqlException e)
            //{
            //    throw;
            //}

            // set up the web client
            //webClient.BaseAddress = "http://redline-testing.com/";

            // Add dummy values to the login dictionary
            //loginDictionary["kbdavid15"] = "password1";
            //loginDictionary["ryan"] = "123";

            // set focus to the username box
            //textBoxUsername.Focus();

        }

        /// <summary>
        /// Attempt to login with the username/password combination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            // toggle visibility if necessary
            if (loginCanvas.Visibility == System.Windows.Visibility.Hidden)
            {
                loginCanvas.Visibility = System.Windows.Visibility.Visible;
                signupCanvas.Visibility = System.Windows.Visibility.Hidden;
                return;
            }

            // get user input
            string uname = textBoxUsername.Text;
            string pass = passwordBox.Password;

            // this is necessary because the server is using a self-signed certificate
            // In production, we will pay for a cert issued by a CA and will not require this line.
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            // POST the credentials to PHP
            byte[] response = webClient.UploadValues(new Uri("https://redline-testing.com/login.php"), new NameValueCollection()
            {
                { "user", uname },
                { "pass", pass }
            });
            string s = webClient.Encoding.GetString(response);

            MessageBox.Show(s); 
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// If the user has not already created an account, this button will
        /// open the default web browser and take them to the sign up site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {
            loginCanvas.Visibility = System.Windows.Visibility.Hidden;
            //signupCanvas.Visibility = System.Windows.Visibility.Visible;
            Storyboard sb = signupCanvas.FindResource("signUpStoryboard") as Storyboard;
            signupCanvas.BeginStoryboard(sb);
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
