using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Main logical entity-Task
/// </summary>
/// <param name="Id"> unique ID (created automatically)</param>
/// <param name="Description">Description of the task</param>
/// <param name="Alias">Alias of the task</param>
/// <param name="CreateAtDate">Production date</param>
/// <param name="Status">Status of task</param>
/// <param name="Dependencies">Dependency list (task-in-list type)</param>
/// <param name="LinkMilestone">related milestone</param>
/// <param name="BaselineStartDate">Estimated start date</param>
/// <param name="StartDate">Actual start date</param>
/// <param name="ForecastDate">Estimated completion date</param>
/// <param name="DeadlineDate">Final date for completion</param>
/// <param name="ComleteDate">Actual end date</param>
/// <param name="Deliverables">product</param>
/// <param name="Remarks">Remarks</param>
/// <param name="Engineer">If available, the ID and name of the engineer assigned to the task</param>
/// <param name="ComplexityLevel">Difficulty</param>
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreateAtDate { get; set; }
    public BO.Status Status { get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }
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
