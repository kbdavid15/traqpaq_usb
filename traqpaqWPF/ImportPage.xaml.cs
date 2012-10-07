﻿using System;
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
    /// Interaction logic for ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        MainWindow main;
        TraqpaqDevice traqpaq;

        public ImportPage(MainWindow main)
        {
            this.main = main;
            this.traqpaq = main.traqpaq;
            InitializeComponent();
        }
    }
}