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


        private void EnterManager_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }
        private void btnAddYear_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddYear();
            CurrentTime = CurrentTime.AddYears(1);
        }
        private void btnAddMonth_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddMonth();
            CurrentTime = CurrentTime.AddMonths(1);
        }
        private void btnAddDay_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddDay();
            CurrentTime = CurrentTime.AddDays(1);
        }
        private void btnAddHour_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.AddHour();
            CurrentTime = CurrentTime.AddHours(1);
        }

        private void EnterEngineer_Click(object sender, RoutedEventArgs e)
        {
            {
                int id;
                string input = Microsoft.VisualBasic.Interaction.InputBox("please enter your ID:", "Enginner Enter");


                if (int.TryParse(input, out id))
                {
                    try
                    {
                        App.s_bl.Engineer.Read(id);
                        new EngeneerTaskWindow(id).ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }
                }
            }
        }



        private void btnRestClock_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.ResetClock();
            CurrentTime = App.s_bl.Clock;
        }
    }
}
