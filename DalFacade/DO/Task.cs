

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
  string? Description=null,
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
  EngineerExperience? CopmlexityLevel = null
)
{
    public Task() : this(0) { }
    //public Task(int Id, string? Description, TimeSpan? RequiredEffortTime, string Alias, bool IsMilestone, DateTime? CreatedAtDate, DateTime? StartDate, DateTime? ScheduledDate, DateTime? DeadlineDate, DateTime? CompleteDatestring, string? Deliverables, string? Remarks, int? EngineerId, EngineerExperience? CopmlexityLevel)
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
    //    this.CopmlexityLevel= CopmlexityLevel;
    //}
}

