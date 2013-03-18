﻿using Facebook;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
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

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        const string _facebookAppId = "345961062172188";
        string _permissions = ""; //"user_about_me,read_stream,publish_stream"; // Set your permissions here
        FacebookClient _fb = new FacebookClient();

        /// <summary>
        /// key is username, value is password
        /// </summary>
        Dictionary<string, string> loginDictionary = new Dictionary<string, string>();  //TODO this is obviously not the correct way to store login credentials. Need to use a db

        public LoginWindow()
        {
            InitializeComponent();
            // Add dummy values to the login dictionary
            loginDictionary["kbdavid15"] = "password1";
            loginDictionary["ryan"] = "123";

            // set focus to the username box
            textBoxUsername.Focus();

            // load the login web page (facebook, google, twitter)
            loginBrowser.Navigate(new Uri("http://www.traqpaq.com/facebook/loginpage.html"));

        }

        /// <summary>
        /// Attempt to login with the username/password combination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            //TODO set up login credentials database
            //look into password management
            try
            {
                if (loginDictionary[textBoxUsername.Text] == passwordBox.Password)
                {
                    Tag = textBoxUsername.Text;
                    this.DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Access denied");
                }
            }
            catch (KeyNotFoundException)
            {
                MessageBox.Show("That username was not found in our records.");
            }            
        }

        /// <summary>
        /// If the user has not already created an account, this button will
        /// open the default web browser and take them to the sign up site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSignUp_Click(object sender, RoutedEventArgs e)
        {

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
