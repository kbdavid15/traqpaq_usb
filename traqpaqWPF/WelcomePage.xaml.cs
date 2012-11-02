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

        ///// <summary>
        ///// Show the log page (record page)
        ///// </summary>
        //private void buttonLogBook_Click(object sender, RoutedEventArgs e)
        //{
        //    //main.navigatePage(PageName.RECORDS);
        //    // create the log page
        //    if (main.recordPage == null)
        //    {
        //        RecordTablePage page = new RecordTablePage(main);
        //        main.recordPage = page;
        //    }
        //    main.navigatePage(main.recordPage);
        //}

        ///// <summary>
        ///// Import a new session from the device
        ///// </summary>
        //private void buttonImport_Click(object sender, RoutedEventArgs e)
        //{
        //    main.navigatePage(PageName.IMPORT);
        //}

        /// <summary>
        /// Change user settings
        /// </summary>
        private void buttonSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
        }
    }
}
