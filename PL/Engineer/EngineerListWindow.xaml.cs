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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = App.s_bl.Engineer.ReadAll();
        }
        /// <summary>
        /// open the page to update the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engineer_doubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItems.Count > 0)
            {
                BO.Engineer selectedEngineer = listView.SelectedItem as BO.Engineer;

                if (selectedEngineer != null)
                {
                    //TaskToChoose.LevelOfEng = s_bl.Engineer.Read(selectedEngineer.Id)!.Level;
                    new EngineerWindow(selectedEngineer.Id).ShowDialog();
                }
            }
        }

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        private void cbLevelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Level == BO.EngineerExperience.None) ?
                            App.s_bl?.Engineer.ReadAll()! : App.s_bl?.Engineer.ReadAll(eng => eng.Level == Level)!;

        }

        private void Add_Engineer(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        public void RefreshEngineerList()
        {
            EngineerList = (Level == BO.EngineerExperience.None) ?
                App.s_bl?.Engineer.ReadAll()! : App.s_bl?.Engineer.ReadAll(eng => eng.Level == Level)!;
        }

    }
}
