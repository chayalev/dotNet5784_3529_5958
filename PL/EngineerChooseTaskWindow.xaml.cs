using BO;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngineerChooseTaskWindow.xaml
    /// </summary>
    public partial class EngineerChooseTaskWindow : Window
    {
        private static BO.Engineer engineer;
        public DateTime? RealStartDate { get; set; } // מאפיין תאריך
        public EngineerChooseTaskWindow(int id)
        {
            engineer = App.s_bl!.Engineer.Read(id)!;
            TaskList = App.s_bl?.Task.AllTaskInListByEngineerLevel(engineer.Level)!;
            RealStartDate = null;
            InitializeComponent();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerChooseTaskWindow), new PropertyMetadata(null));

        /// <summary>
        /// When the engineer selects a task from the list of tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (sender as ListBox)!;
            //In case you did not enter a start date for the selected task
            if (RealStartDate == null )
            {
                MessageBox.Show($"Please insert the start date for this task", "Insert Start Date", MessageBoxButton.OK, MessageBoxImage.Information);
                listBox!.SelectedIndex = -1;     
            }
            else
            {
                try
                {
                    if (listBox != null)
                    {
                        var chosenTask = listBox.SelectedItem as BO.TaskInList;
                        var task = App.s_bl.Task.Read(chosenTask!.Id);
                        //Checks if the selected task is found and updates it accordingly
                        if (task != null)
                        {
                            task.BaselineStartDate = RealStartDate; 
                            task.Engineer = new EngineerInTask { Id = engineer.Id, Name = engineer.Name };
                            App.s_bl.Task.Update(task);
                        }
                        //Creates a task of type task in the engineer and updates the engineer to be responsible for that task
                        engineer.Task = new TaskInEngineer { Id = chosenTask.Id, Alias = chosenTask.Alias };
                        App.s_bl.Engineer.Update(engineer);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Close();
                //After updating, a window opens for him with the details of the selected task
                new EngeneerTaskWindow(engineer.Id).ShowDialog();
            }

        }
    }

}

