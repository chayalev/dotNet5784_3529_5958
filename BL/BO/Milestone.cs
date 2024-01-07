using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Milestone
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public datetime CreatedAtDate { get; set; }
    public BO.Status Status { get; set; }
    public datetime StarttDate { get; set; }
    public datetime ForecastDate { get; set; }
    public datetime DeadlineDate { get; set; }
    public datetime CompleteDate { get; set; }
    public double CompletionPercentage { get; set; }
    public string Remarks { get; set; }
    public List<BO.TaskInList> Dependencies { get; set; }
}
