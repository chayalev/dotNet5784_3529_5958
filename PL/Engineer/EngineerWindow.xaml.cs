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

        }

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;


        public void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if ((string)(sender as Button)!.Content == "Update")
                s_bl.Engineer.Update(Engineer);
            else
                s_bl.Engineer.Create(Engineer);
            Close();

        }
    }
}
