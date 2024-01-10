using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// Main logical entity-Milestone
/// </summary>
/// <param name="Id">ID</param>
/// <param name="Description">Description</param>
/// <param name="Alias">Alias</param>
/// <param name="CreatedAtDate">Production date</param>
/// <param name="Status">Status of task</param>
/// <param name="StartDate">Actual start date</param>
/// <param name="ForecastDate">Estimated completion date</param>
/// <param name="DeadlineDate">Final date for completion</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="CompletionPercentage">progress percentage</param>
/// <param name="Remarks">Remarks</param>
/// <param name="Dependencies">Dependency list (task-in-list type)</param>
public class Milestone
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime? CreatedAtDate { get; set; }
    public BO.Status Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public double CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public List<BO.TaskInList>? Dependencies { get; set; }
}
