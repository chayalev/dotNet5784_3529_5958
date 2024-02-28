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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                Task = s_bl.Task.Read(id)!;
            else
                Task = new BO.Task();
        }

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)(sender as Button)!.Content == "Update")
                    s_bl.Task.Update(Task);
                else
                    s_bl.Task.Create(Task);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();

        }
    }
}
