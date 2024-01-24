namespace Dal;
using DalApi;
using System;

sealed internal class DalList : IDal
{
    //public static IDal Instance { get; } = new DalList();
    private static readonly Lazy<IDal> lazyInstance = new Lazy<IDal>(() => new DalList());
    private static readonly object lockObject = new object();

    public static IDal Instance
    {
        get
        {
            lock (lockObject)
            {
                return lazyInstance.Value;
            }
        }
    }

    private DalList() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;

    public void Reset()
    {
        Engineer.Reset();   
        Dependency.Reset();
        Task.Reset();
    }

 
}
