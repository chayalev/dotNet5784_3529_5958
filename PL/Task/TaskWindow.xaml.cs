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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow(int id = 0)
        {
            InitializeComponent();
            if (id != 0)
                Task = App.s_bl.Task.Read(id)!;
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
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                //var dependencies = listBox as IEnumerable<TaskInEngineer>;
                //var dependencies = listBox.Items.Cast<TaskInEngineer>();
                var dependencies = listBox.SelectedItems.OfType<TaskInEngineer>();

                foreach (var selectedItem in dependencies)
                {
                    var task = App.s_bl.Task.Read(selectedItem.Id)!;
                    var dependecy = new TaskInList
                    {
                        Alias = task.Alias,
                        Description = task.Description,
                        Id = task.Id,
                        Status = task.StatusTask
                    };
                    if (Task.Dependencies == null)
                    {
                        Task.Dependencies = new List<TaskInList>();
                    }

                    Task.Dependencies.Add(dependecy);

                }
                if (Task.Id == 0)
                    App.s_bl.Task.Create(Task);
                else
                    App.s_bl.Task.Update(Task);
            }
        }

        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Task.Dependencies != null || Task.Id == 0)
                {
                    if ((string)(sender as Button)!.Content == "Update")
                        App.s_bl.Task.Update(Task);
                    else
                        App.s_bl.Task.Create(Task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();

        }
    }
}
