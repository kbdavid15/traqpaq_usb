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
        WebClient webClient = new WebClient();

        public LoginWindow()
        {
            InitializeComponent();

            // load the login web page (facebook, google, twitter)
            loginBrowser.Navigate(new Uri("http://www.traqpaq.com/facebook/loginpage.html"));

            // set up the web client
            //webClient.BaseAddress = "http://redline-testing.com/";

            // set focus to the username box
            textBoxUsername.Focus();
        }

        /// <summary>
        /// Attempt to login with the username/password combination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            // toggle visibility if necessary
            //if (loginCanvas.Visibility == System.Windows.Visibility.Hidden)
            //{
            //    loginCanvas.Visibility = System.Windows.Visibility.Visible;
            //    signupCanvas.Visibility = System.Windows.Visibility.Hidden;
            //    return;
            //}

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
            //loginCanvas.Visibility = System.Windows.Visibility.Hidden;
            //signupCanvas.Visibility = System.Windows.Visibility.Visible;
            Storyboard sb = inputCanvas.FindResource("signUpStoryboard") as Storyboard;
            inputCanvas.BeginStoryboard(sb);

            // hide signup button, show submit button
            buttonSignUp.Visibility = System.Windows.Visibility.Collapsed;
            buttonSubmit.Visibility = System.Windows.Visibility.Visible;
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

        /// <summary>
        /// Verify the user submitted values and send to server for further verification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            // username and password are required, and passwords must match
            //TODO implement Ajax and have the server check if a username is available before hitting submit
            string firstname = null, lastname = null, email, password;
            if (textBoxFirstName.Text != "")
                firstname = textBoxFirstName.Text;
            if (textBoxLasttName.Text != "")
                lastname = textBoxLasttName.Text;
            if (textBoxSignUpUsername.Text != "")
            {
                email = textBoxSignUpUsername.Text;
                // check against regex to determine if it is an email
                RegexUtil util = new RegexUtil();   //TODO convert this into a static class/method
                if (!util.IsValidEmail(email))
                {
                    MessageBox.Show("Please enter a valid email address");
                    return;
                }
            }
            else
            {
                MessageBox.Show("You must provide an email address");
                return;
            }
            if (passwordBoxSignUp.Password != "" && passwordBoxConfirmSignUp.Password != "" && passwordBoxSignUp.Password == passwordBoxConfirmSignUp.Password)
            {
                // check if password is secure enough
                password = passwordBoxSignUp.Password;
            }
            else
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            // this is necessary because the server is using a self-signed certificate
            // In production, we will pay for a cert issued by a CA and will not require this line.
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            // now, send the information to the server to create a database entry for this user
            byte[] response = webClient.UploadValues(new Uri("https://redline-testing.com/signup.php"), new NameValueCollection()
            {
                { "firstname", firstname },
                { "lastname", lastname },
                { "email", email },
                { "password", password }
            });
            string s = webClient.Encoding.GetString(response);
            MessageBox.Show(s);
        }

        /// <summary>
        /// Use this function to verify the username before the submit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxSignUpUsername_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
