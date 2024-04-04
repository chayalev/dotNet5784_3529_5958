using DalApi;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Dal;

//stage 3
sealed internal class DalXml : IDal
{
    static string s_data_config_xml = "data-config";
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? StartDate
    {
        get => XMLTools.GetDate(s_data_config_xml, "StartDate");
        set => XMLTools.ChangeDate(s_data_config_xml, "StartDate", value);
    }
    public DateTime? EndDate
    {
        get => XMLTools.GetDate(s_data_config_xml, "EndDate");
        set => XMLTools.ChangeDate(s_data_config_xml, "EndDate", value);
    }
    //public static DateTime MyClock
    //{
    //    get => XMLTools.GetClock(s_data_config_xml, "MyClock");
    //    set => XMLTools.ChangeClock(s_data_config_xml, "MyClock", value);
    //}

    public void Reset()
    {
        XMLTools.ChangeDate(s_data_config_xml, "EndDate", null);
        XMLTools.ChangeDate(s_data_config_xml, "StartDate", null);
        XMLTools.ResetId(s_data_config_xml, "NextDependencyId");
        XMLTools.ResetId(s_data_config_xml, "NextTaskId");
        Dependency.Reset();
        Engineer.Reset();
        Task.Reset();   
    }
}

