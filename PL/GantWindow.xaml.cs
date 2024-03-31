using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public DataTable dataTable
        {
            get { return (DataTable)GetValue(dataTableProperty); }
            set { SetValue(dataTableProperty, value); }
        }

        public static readonly DependencyProperty dataTableProperty = 
            DependencyProperty.Register("dataTable", typeof(DataTable), typeof(GantWindow), new PropertyMetadata(null));


        public GantWindow()
        {
            buildDataTable();

            InitializeComponent();
        }

        private void buildDataTable()
        {
            dataTable = new DataTable();

            dataTable.Columns.Add("Task Id", typeof(int));
            dataTable.Columns.Add("Task Name", typeof(string));
            dataTable.Columns.Add("Engineer Id", typeof(int));
            dataTable.Columns.Add("Engineer Name", typeof(string));

            int col = 4;

            for(DateTime day = s_bl.StartDate ?? s_bl.Clock; day <= s_bl.EndDate; day = day.AddDays(1))
            {
                string strDay = $"{day.Day}-{day.Month}-{day.Year}";
                dataTable.Columns.Add(strDay, typeof(string));
                col++;
            }

            IEnumerable<BO.Task> orderedTasks = s_bl.Task.ReadAll().OrderBy(x => x.StartDate);
            foreach(BO.Task task in orderedTasks)
            {
               
                DataRow row = dataTable.NewRow();
                row[0] = task.Id;
                row[1] = task.Alias;
                row[2] = 0;
                row[3] = "name";

                for (DateTime day = s_bl.StartDate ?? s_bl.Clock; day <=s_bl.EndDate; day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}-{day.Month}-{day.Year}";

                    if (day < s_bl.StartDate || day > s_bl.EndDate)
                        row[strDay] = BO.Status.Unscheduled;
                    else
                        row[strDay] = task.StatusTask;
                }
                dataTable.Rows.Add(row);

            }
            

        }
    }
}
