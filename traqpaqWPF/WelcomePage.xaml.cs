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

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        MainWindow main;

        public WelcomePage(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void buttonLogBook_Click(object sender, RoutedEventArgs e)
        {
            main.navigatePage(PageName.RECORDS);
        }

        /// <summary>
        /// Import a new session from the device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Change user settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
