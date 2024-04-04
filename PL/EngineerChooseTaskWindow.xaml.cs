using BO;
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
    /// Interaction logic for EngineerChooseTaskWindow.xaml
    /// </summary>
    public partial class EngineerChooseTaskWindow : Window
    {
        private static BO.Engineer engineer;
        public EngineerChooseTaskWindow(int id)
        {
            engineer = App.s_bl!.Engineer.Read(id)!;
            TaskList = App.s_bl?.Task.AllTaskInListByEngineerLevel(engineer.Level)!;
            InitializeComponent();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                var chosenTask = listBox.SelectedItem as BO.TaskInList;
                var taskInEngineer = new TaskInEngineer { Id = chosenTask!.Id, Alias = chosenTask.Alias };
                engineer.Task = taskInEngineer;
                var task = App.s_bl.Task.Read(chosenTask!.Id);
                if (task != null)
                {
                    task.Engineer = new EngineerInTask { Id = engineer.Id, Name = engineer.Name };
                    App.s_bl.Task.Update(task);
                }
                App.s_bl.Engineer.Update(engineer);
            }
        }
    }
}
