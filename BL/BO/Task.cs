using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? alias { get; set; }
    public DateTime? CreateAtDate { get; set; }
    public BO.Status Status { get; set; }
    public IEnumerable<BO.TaskInList>? Dependencies { get; set; }
    public DateTime? LinkMilestone { get; set; }
    public DateTime? BaselineStartDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? ComleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience ComplexityLevel { get; set; }
}
