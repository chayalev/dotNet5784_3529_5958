using BO;
using DO;
using PL.Engineer;
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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskInListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        public TaskListWindow()
        {
            TaskList = App.s_bl?.Task.AllTaskInList()!;
            InitializeComponent();
        }
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        /// <summary>
        /// open the page to update the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Task_doubleClick(object sender, MouseButtonEventArgs e)
        {
            //If a task is selected
            if (sender is ListView listView && listView.SelectedItems.Count > 0)
            {
                BO.TaskInList? selectedTask = listView.SelectedItem as BO.TaskInList;

                ///Messenger to the task editing page
                if (selectedTask != null)
                    new TaskWindow(selectedTask.Id).ShowDialog();
            }
        }

        /// <summary>
        /// Selecting tasks that match the level of the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Level == BO.EngineerExperience.None) ?
                            App.s_bl?.Task.AllTaskInList()! : App.s_bl?.Task.TaskInListByLevel(Level)!;
        }

        /// <summary>
        /// Opening the Add Task page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Task(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
        }

    }
}
