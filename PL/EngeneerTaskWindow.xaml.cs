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
            var task  = App.s_bl.Engineer.Read(id)?.Task;
            if (task != null)
            {
                Task = App.s_bl.Task.Read(task.Id);
            }

        }

        public BO.Task? Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));


        private void TaskForEngineer_Click(object sender, RoutedEventArgs e)
        {
            App.s_bl.Task.AllTaskInEngineer(engineer.Level);
        }
    }
}
