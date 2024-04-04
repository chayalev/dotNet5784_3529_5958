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
        /// <summary>
        /// The chart table
        /// </summary>
        public DataTable dataTable
        {
            get { return (DataTable)GetValue(dataTableProperty); }
            set { SetValue(dataTableProperty, value); }
        }

        public static readonly DependencyProperty dataTableProperty =
            DependencyProperty.Register("dataTable", typeof(DataTable), typeof(GantWindow), new PropertyMetadata(null));


        public GantWindow()
        {
            buildDataTable(); //table initialization
            InitializeComponent();
        }

        private void buildDataTable()
        {
            dataTable = new DataTable();

            ///Creating columns of task details
            dataTable.Columns.Add("Task Id", typeof(int));
            dataTable.Columns.Add("Task Name", typeof(string));
            dataTable.Columns.Add("Engineer Name", typeof(string));
            dataTable.Columns.Add("Depend On Tasks", typeof(string));
            int col = 4;

            ///Creating columns of project days
            for (DateTime day = App.s_bl.StartDate ?? App.s_bl.Clock; day <= App.s_bl.EndDate; day = day.AddDays(1))
            {
                string strDay = $"{day.Day}-{day.Month}-{day.Year}";
                dataTable.Columns.Add(strDay, typeof(string));
                col++;
            }

            ///Task sorting
            IEnumerable<BO.Task> orderedTasks = App.s_bl.Task.ReadAll().OrderBy(x => x.StartDate);
            
            ///Filling the table
            foreach (BO.Task task in orderedTasks)
            {
                DataRow row = dataTable.NewRow();
                row[0] = task.Id;
                row[1] = task.Alias;
                if (task.Engineer != null)
                    row[2] = task.Engineer.Name;
                else
                    row[2] = "";
                row["Depend On Tasks"] = string.Join(", ", task.Dependencies!.Select(d => $"{d.Id}"));

     
                for (DateTime day = App.s_bl.StartDate ?? App.s_bl.Clock; day <= App.s_bl.EndDate; day = day.AddDays(1))
                {
                    string strDay = $"{day.Day}-{day.Month}-{day.Year}";

                    if (day < task.StartDate || day > task.DeadlineDate)
                        row[strDay] = BO.Status.Unscheduled;
                    else
                        row[strDay] = task.StatusTask;
                }
                dataTable.Rows.Add(row);
            }
        }
    }
}
