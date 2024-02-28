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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerWindow(int id = 0)
        {

            InitializeComponent();
            if (id != 0)
                Engineer = s_bl.Engineer.Read(id)!;
            else
                Engineer = new BO.Engineer();
            TaskToChoose = s_bl.Task.AllTaskInEngineer();

        }

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        public IEnumerable<BO.TaskInEngineer> TaskToChoose
        {
            get { return (IEnumerable<BO.TaskInEngineer>)GetValue(TaskToChooseProperty); }
            set { SetValue(TaskToChooseProperty, value); }
        }

        public static readonly DependencyProperty TaskToChooseProperty =
            DependencyProperty.Register("TaskToChoose", typeof(IEnumerable<BO.TaskInEngineer>), typeof(EngineerWindow), new PropertyMetadata(null));

        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)(sender as Button)!.Content == "Update")
                    s_bl.Engineer.Update(Engineer);
                else
                    s_bl.Engineer.Create(Engineer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                Closed += EngineerWindow_Closed!;
                Close();
            }

           

        }
        private void EngineerWindow_Closed(object sender, EventArgs e)
        {
            // An instance of the main window EngineerListWindow
            var mainWindow = Application.Current.Windows
                                            .OfType<EngineerListWindow>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                // Updating the list of engineers in the main window by calling the BL
                // function that returns the list of engineers
                mainWindow.EngineerList = s_bl.Engineer.ReadAll()!;
            }
        }
    }
}
