﻿using BO;
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
            ///Initialization of a task
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

        /// <summary>
        /// Dependency selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;
            if (listBox != null)
            {
                var dependencies = listBox.SelectedItems.OfType<TaskInEngineer>();

                ///Add each selected dependency to the list of dependencies
                foreach (var selectedItem in dependencies)
                {
                    var task = App.s_bl.Task.Read(selectedItem.Id)!;
                    ///Convert to task in list
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
            }
        }

        /// <summary>
        /// Sending a task for update / addition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((string)(sender as Button)!.Content == "Update")
                    App.s_bl.Task.Update(Task);
                else
                    App.s_bl.Task.Create(Task);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ///Closing page after finishing
                Closed += TaskWindow_Closed!;
                Close();
            }

        }
        
        /// <summary>
        /// updating the tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            // An instance of the main window TaskListWindow
            var mainWindow = Application.Current.Windows
                                            .OfType<TaskListWindow>()
                                            .FirstOrDefault();
            if (mainWindow != null)
            {
                // Updating the list of engineers in the main window by calling the BL
                // function that returns the list of engineers
                mainWindow.TaskList = App.s_bl.Task.AllTaskInList()!;
            }
        }
    }
}
