using PL.Engineer;
using PL.Task;
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
        //static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty StatusMessageProperty =
          DependencyProperty.Register("StatusMessage", typeof(string), typeof(MainWindow));

        public string StatusMessage
        {
            get { return (string)GetValue(StatusMessageProperty); }
            set { SetValue(StatusMessageProperty, value); }
        }
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
            App.s_bl.InitializeDB();
            MessageBox.Show($"The details were initialized successfully");
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.ResetDB();
            MessageBox.Show($"The details were reseted successfully");
        }

        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }
       
        private void btnCreateSAuto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.s_bl.CreateProject();
                MessageBox.Show("the project was began!", "success!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCreateSManual_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // App.s_bl.s();
                MessageBox.Show("the project was began!", "success!!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnGant_Click(object sender, RoutedEventArgs e)
        {
            new GantWindow().ShowDialog();
        }

    }
}
