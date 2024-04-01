
namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public ITask Task { get; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsCreate { get; set; }
    public void CreateProject();
    public void InitializeDB();
    public void ResetDB();
    #region Clock
    public DateTime Clock { get; }
    public void ResetClock();
    public void AddYear();
    public void AddMonth();
    public void AddDay();
    public void AddHour();
    #endregion
    public void StartDateForTask(BO.Task tsk, DateTime startForTsk);
}

