using PL.Engineer;
using PL.Task;
using System;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static readonly DependencyProperty StartDateProperty =
          DependencyProperty.Register("StartDate", typeof(DateTime), typeof(MainWindow));

        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }
        public MainWindow()
        {
            InitializeComponent();
            StartDate = App.s_bl.Clock;
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
                if (StartDate != null)
                {
                    App.s_bl.CreateProject(StartDate);
                    MessageBox.Show("the project was began!", "success!!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("you must insert a start date");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //private void btnCreateSManual_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        // App.s_bl.s();
        //        MessageBox.Show("the project was began!", "success!!", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}


        private void btnGant_Click(object sender, RoutedEventArgs e)
        {
            if (App.s_bl.IsCreate)
                new GantWindow().ShowDialog();
            else
                MessageBox.Show("Impossible to see the gantt chart before creating the project!");
        }

    }
}
