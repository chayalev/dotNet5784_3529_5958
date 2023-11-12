

namespace DO;
/// <summary>
/// Task Entity represents a task with all its props
/// </summary>
/// <param name="Id"> unique ID (created automatically)</param>
/// <param name="Description"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="Alias"></param>
/// <param name="IsMilestone"></param>
/// <param name="CreatedAtDate"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
/// <param name="EngineerId"></param>
/// <param name="CopmlexityLevel"></param>


public record Task
(
  int Id,
  string Description,
  TimeSpan? RequiredEffortTime=null,
  string? Alias = null,
  bool IsMilestone=false,
  DateTime? CreatedAtDate=null,
  DateTime? StartDate=null,
  DateTime? ScheduledDate=null,
  DateTime? DeadlineDate=null ,
  DateTime? CompleteDate = null,
  string? Deliverables = null,
  string? Remarks = null,
  int? EngineerId = null,
  int? CopmlexityLevel = null
)
{
    public Task() : this(0,"") { }
    
}

