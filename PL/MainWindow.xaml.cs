﻿using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();
        }

        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeDB();
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            s_bl.ResetDB();
        }

    }
}
