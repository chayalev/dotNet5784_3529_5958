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
       

        public EngineerWindow(int id = 0)
        {
           
            InitializeComponent();
            if (id != 0)
                Engineer = App.s_bl.Engineer.Read(id)!;
            else
                Engineer = new BO.Engineer();
        }

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public static BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        /// <summary>
        /// Add and update the enginner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Update
                if ((string)(sender as Button)!.Content == "Update")
                {
                    App.s_bl.Engineer.Update(Engineer);
                    MessageBox.Show($"The engineer: {Engineer.Name} update successfully", "Update successfully");
                }
                //Add
                else
                {
                    App.s_bl.Engineer.Create(Engineer);
                    MessageBox.Show($"The engineer: {Engineer.Name} was added successfully", "Add successfully");


                }
                Closed += EngineerWindow_Closed!;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
               



        }
        /// <summary>
        /// When adding or updating an engineer - refreshes the list of engineers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                mainWindow.EngineerList = App.s_bl.Engineer.ReadAll()!;
            }
        }
    }
}
