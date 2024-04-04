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
        //The project start date
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
        
        /// <summary>
        /// Entering the engineer page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().ShowDialog();
        }
        /// <summary>
        /// Initialize the details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.InitializeDB();
            MessageBox.Show($"The details were initialized successfully");
        }

        /// <summary>
        /// Deleting the details from the system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.ResetDB();
            MessageBox.Show($"The details were reseted successfully");
        }
        
        /// <summary>
        /// View all project tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();
        }

        /// <summary>
        /// Automatic project creation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Displaying a Gantt chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGant_Click(object sender, RoutedEventArgs e)
        {
            ///A Gantt chart can only be displayed after creating a project
            if (App.s_bl.IsCreate)
                new GantWindow().ShowDialog();
            else
                MessageBox.Show("Impossible to see the gantt chart before creating the project!");
        }

    }
}
