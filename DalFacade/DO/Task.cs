

namespace DO;
/// <summary>
/// Task Entity represents a task with all its props
/// </summary>
/// <param name="Id"> unique ID (created automatically)</param>
/// <param name="Description">Description of the task</param>
/// <param name="RequiredEffortTime">The amount of time required to perform the task</param>
/// <param name="Alias">Alias of the task</param>
/// <param name="IsMilestone">Milestone</param>
/// <param name="CreatedAtDate">Task creation date</param>
/// <param name="StartDate">Planned date for work to begin</param>
/// <param name="ScheduledDate">Date of commencement of work on the assignment</param>
/// <param name="DeadlineDate">Possible final end date</param>
/// <param name="CompleteDate">Actual end date</param>
/// <param name="Deliverables">Deliverables</param>
/// <param name="Remarks">Remarks</param>
/// <param name="EngineerId">The engineer ID assigned to the task</param>
/// <param name="CopmlexityLevel">The difficulty level of the task</param>


public record Task
(
  int Id,
  string? Description=null,
  TimeSpan? RequiredEffortTime=null,
  string? Alias = null,
  bool? IsMilestone=false,
  DateTime? CreatedAtDate=null,
  DateTime? StartDate=null,
  DateTime? ScheduledDate=null,
  DateTime? DeadlineDate=null ,
  DateTime? CompleteDate = null,
  string? Deliverables = null,
  string? Remarks = null,
  int? EngineerId = null,
  EngineerExperience? CopmlexityLevel = null
)
{
    public Task() : this(0) { }
  //  public Task(int Id, string? Description, TimeSpan? RequiredEffortTime, string Alias, bool IsMilestone, DateTime? CreatedAtDate, DateTime? StartDate, DateTime? ScheduledDate, DateTime? DeadlineDate, DateTime? CompleteDatestring, string? Deliverables, string? Remarks, int? EngineerId, EngineerExperience? CopmlexityLevel):this(Id,Description,)
    //{
    //    this.Id = Id;
    //    this.Description = Description;
    //    this.RequiredEffortTime = RequiredEffortTime;
    //    this.Alias = Alias;
    //    this.IsMilestone = IsMilestone;
    //    this.CreatedAtDate = CreatedAtDate;
    //    this.StartDate = StartDate;
    //    this.ScheduledDate = ScheduledDate;
    //    this.DeadlineDate = DeadlineDate;
    //    this.Deliverables = Deliverables;
    //    this.Remarks = Remarks;
    //    this.EngineerId = EngineerId;
    //    this.CopmlexityLevel = CopmlexityLevel;
   // }
}

