
namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }
    public ITask Task { get; }
    public DateTime? startDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsCreate { get; set; }
    public void Reset();

}

