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
        private MainWindow parent;
        private UIElement[] ajaxImages, ajaxSignUpImages;
        private CookieContainer cookieContainer = new CookieContainer();

        public LoginWindow(MainWindow parent)
        {
            InitializeComponent();

            this.parent = parent;

            ajaxImages = new UIElement[] { ajaxLoadingImage, greenCheckImage, redXImage };
            ajaxSignUpImages = new UIElement[] { ajaxLoadingImageSignUp, greenCheckImageSignUp, redXImageSignUp };

            // load the login web page (facebook, google, twitter)
            loginBrowser.Navigate(new Uri("http://www.traqpaq.com/facebook/loginpage.html"));

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
            // get user input
            string uname = textBoxUsername.Text;
            string pass = passwordBox.Password;

            // this is necessary because the server is using a self-signed certificate
            // In production, we will pay for a cert issued by a CA and will not require this line.
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            // POST the credentials to PHP
            CookieWebClient webClient = new CookieWebClient();
            //WebClient webClient = new WebClient();
            byte[] response = webClient.UploadValues(new Uri("https://redline-testing.com/login.php"), new NameValueCollection()
            {
                { "user", uname },
                { "pass", pass }
            });
            
            switch (PHP.get_PHP_return(webClient.Encoding.GetString(response)))
            {
                case PHPreturn.LOGIN_SUCCESSFUL:
                    // close login window
                    this.Close();
                    break;
                case PHPreturn.INCORRECT_PASSWORD:
                    MessageBox.Show("Incorrect password");
                    break;
                case PHPreturn.USERNAME_DNE:
                    MessageBox.Show("That username was not found in our database");
                    break;
                default:
                    break;
            }

            // test the cookies
            byte[] x = webClient.DownloadData(new Uri("https://redline-testing.com/upload.php"));

            MessageBox.Show( webClient.Encoding.GetString(x));


            // release the web client
            webClient.Dispose();
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
            buttonSignUp.Visibility = Visibility.Collapsed;
            buttonSubmit.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Verify the user submitted values and send to server for further verification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            // username and password are required, and passwords must match
            //TODO implement Ajax and have the server check if a username is available before hitting submit
            string firstname = null, lastname = null, email, password;
            if (textBoxFirstName.Text != "")
                firstname = textBoxFirstName.Text;
            if (textBoxLastName.Text != "")
                lastname = textBoxLastName.Text;
            if (textBoxSignUpUsername.Text != "")
            {
                email = textBoxSignUpUsername.Text;
                // check against regex to determine if it is an email
                //RegexUtil util = new RegexUtil();   //TODO convert this into a static class/method
                if (!RegexUtil.IsValidEmail(email))
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
                //TODO check if password is secure enough
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

            WebClient webClient = new WebClient();

            // now, send the information to the server to create a database entry for this user
            byte[] response = await webClient.UploadValuesTaskAsync(new Uri("https://redline-testing.com/signup.php"), new NameValueCollection()
            {
                { "firstname", firstname },
                { "lastname", lastname },
                { "email", email },
                { "password", password }
            });
            string s = webClient.Encoding.GetString(response);
            MessageBox.Show(s);

            // release the web client
            webClient.Dispose();
        }

        private async void textBoxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxUsername.Text == "")
                foreach (var image in ajaxImages)
                    image.Visibility = Visibility.Collapsed;
            else
            {
                // show the loading image
                ajaxLoadingImage.Visibility = Visibility.Visible;

                // this is necessary because the server is using a self-signed certificate
                // In production, we will pay for a cert issued by a CA and will not require this line.
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                WebClient webClient = new WebClient();

                // now, send the information to the server to create a database entry for this user
                byte[] response_bytes = await webClient.UploadValuesTaskAsync(new Uri("https://redline-testing.com/ajax/username.php"), new NameValueCollection() { { "email", textBoxUsername.Text } });
                string response = webClient.Encoding.GetString(response_bytes);

                // parse response
                PHPreturn PHP_response = PHP.get_PHP_return(response);
                switch (PHP_response)
                {
                    case PHPreturn.USERNAME_EXISTS:
                        setImageVisibility(greenCheckImage);
                        break;
                    case PHPreturn.USERNAME_DNE:
                        setImageVisibility(redXImage);
                        break;
                    default:
                        ajaxLoadingImage.Visibility = Visibility.Collapsed;
                        break;
                }

                // release the web client
                webClient.Dispose();
            }
        }

        /// <summary>
        /// Sets the image to visible and the other images to collapsed.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="isSignUp">Optional paramater to use the sign up images vs the login images. Defaults to false</param>
        private void setImageVisibility(UIElement image, bool isSignUp = false)
        {
            if (isSignUp)
                foreach (var elem in ajaxSignUpImages)
                    elem.Visibility = elem.Equals(image) ? Visibility.Visible : Visibility.Collapsed;
            else
                foreach (var elem in ajaxImages)
                    elem.Visibility = elem.Equals(image) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Similar to the other username LostFocus callback, but inverted. The red x shows when the username is taken,
        /// and the green check shows when the username is available
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void textBoxSignUpUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSignUpUsername.Text == "")
            {
                foreach (var image in ajaxImages)
                    image.Visibility = Visibility.Collapsed;
            }
            else
            {
                // show the loading image
                ajaxLoadingImageSignUp.Visibility = Visibility.Visible;

                // this is necessary because the server is using a self-signed certificate
                // In production, we will pay for a cert issued by a CA and will not require this line.
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                WebClient webClient = new WebClient();

                // now, send the information to the server to create a database entry for this user
                byte[] response_bytes = await webClient.UploadValuesTaskAsync(new Uri("https://redline-testing.com/ajax/username.php"), new NameValueCollection() { { "email", textBoxUsername.Text } });
                string response = webClient.Encoding.GetString(response_bytes);

                // parse response
                PHPreturn PHP_response = PHP.get_PHP_return(response);
                switch (PHP_response)
                {
                    case PHPreturn.USERNAME_EXISTS:
                        setImageVisibility(redXImage);
                        break;
                    case PHPreturn.USERNAME_DNE:
                        setImageVisibility(greenCheckImage);
                        break;
                    default:
                        ajaxLoadingImage.Visibility = Visibility.Collapsed;
                        break;
                }

                // release the web client
                webClient.Dispose();
            }

        }
    }
}
