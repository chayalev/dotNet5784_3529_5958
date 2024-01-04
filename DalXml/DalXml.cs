using DalApi;
using System.Linq.Expressions;

namespace Dal;

//stage 3
sealed public class DalXml : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startDate { get ; set ; }
    public DateTime? EndDate { get; set; }

    public void Reset()
    {
        Dependency.Reset();
        Engineer.Reset();
        Task.Reset();   
    }
}

