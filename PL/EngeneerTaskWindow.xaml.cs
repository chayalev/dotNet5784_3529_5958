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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngeneerTaskWindow.xaml
    /// </summary>
    public partial class EngeneerTaskWindow : Window
    {
        private static BO.Engineer engineer;
        public EngeneerTaskWindow(int id)
        {
            InitializeComponent();
            engineer = App.s_bl.Engineer.Read(id)!;
            var task = App.s_bl.Engineer.Read(id)?.Task;
            if (task != null)
            {
                Task = App.s_bl.Task.Read(task.Id)!;
            }

        }
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(EngeneerTaskWindow), new PropertyMetadata(null));


        //When the engineer finish his Task
        private void FinishTask_Click(object sender, RoutedEventArgs e)
        {
            //Updates the status to done, the engineer, and the completion date of the task according to the values 
            Task.StatusTask = BO.Status.Done;
            Task.ComleteDate = DateTime.Now;
            Task.Engineer = null;
            try
            {
                App.s_bl.Task.Update(Task);
                MessageBox.Show($"The task: {Task.Alias} completed", "complete!");
                Close();
                //Allows the engineer to select a new task
                new EngineerChooseTaskWindow(engineer.Id).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
