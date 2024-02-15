using DalApi;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startDate
    {
        get => XMLTools.GetDate("data-config", "StartDate");
        set => XMLTools.ChangeDate("data-config", "StartDate", value);
    }
    public DateTime? EndDate
    {
        get => XMLTools.GetDate("data-config", "EndDate");
        set => XMLTools.ChangeDate("data-config", "EndDate", value);
    }

    public void Reset()
    {
        Dependency.Reset();
        Engineer.Reset();
        Task.Reset();   
    }
}

