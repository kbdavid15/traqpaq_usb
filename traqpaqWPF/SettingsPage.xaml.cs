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
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.ViewportConstraints;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Common.Auxiliary;
using Microsoft.Research.DynamicDataDisplay.Common;
using Microsoft.Research.DynamicDataDisplay.Charts.NewLine;
using Microsoft.Research.DynamicDataDisplay.Markers2;

namespace traqpaqWPF
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainWindow mainWindow;

        public SettingsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
    }
}
