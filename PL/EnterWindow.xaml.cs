using BO;
using PL.Engineer;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for EnterWindow.xaml
    /// </summary>
    public partial class EnterWindow : Window
    {
        //static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        /// <summary>
        /// Initialize login window
        /// </summary>
        public EnterWindow()
        {
            InitializeComponent();
            CurrentTime = App.s_bl.Clock;
        }

        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(EnterWindow), new PropertyMetadata(null));

        /// <summary>
        /// Engineer window entry button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterManager_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }
        /// <summary>
        /// Adding a year to the date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddYear_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddYear();
            CurrentTime = CurrentTime.AddYears(1);
        }
        /// <summary>
        /// Adding a month to the date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMonth_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddMonth();
            CurrentTime = CurrentTime.AddMonths(1);
        }
        /// <summary>
        /// Add a day to the date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddDay_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddDay();
            CurrentTime = CurrentTime.AddDays(1);
        }
        /// <summary>
        /// Adding an hour to the clock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddHour_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddHour();
            CurrentTime = CurrentTime.AddHours(1);
        }

        /// <summary>
        /// Login process to an engineer's page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterEngineer_Click(object sender, RoutedEventArgs e)
        {
            {
                ///Obtaining an engineer's identity number
                int id;
                string input = Microsoft.VisualBasic.Interaction.InputBox("please enter your ID:", "Enginner Enter");

                ///If a valid number is entered
                if (int.TryParse(input, out id))
                {
                    try
                    {
                        ///There is no engineer window before creating the schedule
                        if (!App.s_bl.IsCreate)
                            MessageBox.Show("Can not choose a task before project schedule", "Error", MessageBoxButton.OK);
                        else
                        {
                            var engineerTask = App.s_bl.Engineer.Read(id)?.Task;
                            ///Displaying the engineer's task if it exists
                            if (engineerTask is not null)
                                new EngeneerTaskWindow(id).ShowDialog();
                            ///Sending to a page where an engineer selects a task
                            else
                                new EngineerChooseTaskWindow(id).ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                }
            }
        }

        /// <summary>
        /// Reset clock to current date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestClock_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.ResetClock();
            CurrentTime = App.s_bl.Clock;
        }
    }
}
