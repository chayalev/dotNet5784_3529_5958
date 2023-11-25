
namespace DalTest;

using Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Security.Cryptography;
using System.Xml.Linq;

public static class Initialization
{
    private static IDependency? s_dalDependency;
    private static IEngineer? s_dalEngineer;
    private static ITask? s_dalTask;

    //Random number
    private static readonly Random s_rand = new();

    private static void createDependency()
    {
        int? _dependentTask = null, _dependsOnTask = null;
        for (int i = 0; i < 40; i++)
        {
            _dependentTask = s_rand.Next(1, s_dalTask!.ReadAll().Count);
            do
                _dependsOnTask = s_rand.Next(1, s_dalTask!.ReadAll().Count);
            while (_dependentTask != _dependsOnTask);

            if (i == 15)
            {
                for (int j = 15; j < 18; j++)
                {
                    Dependency newDependency1 = new(0, 12, j);
                    s_dalDependency!.Create(newDependency1);
                }
                for (int j = 15; j < 18; j++)
                {
                    Dependency newDependency1 = new(0, 13, j);
                    s_dalDependency!.Create(newDependency1);
                }
            }
        }
        Dependency newDependency = new(0, _dependentTask, _dependsOnTask);
        s_dalDependency!.Create(newDependency);
    }
    private static void createEngineer()
    {
        string[] engineerNames =
    {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
    };


        foreach (var _name in engineerNames)
        {
            int _id;
            //random id and check that nobody use it before
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dalEngineer!.Read(_id) != null);

            string _email = _name.Split(' ')[0] + "@gmail.com";

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);

            double _cost = s_rand.NextDouble() * (500.0 - 100.0) + 100;



            Engineer newEngineer = new(_id, _name, _level, _email, _cost);

            s_dalEngineer!.Create(newEngineer);
        }

    }
    private static void createTask()
    {
        string[] taskAliases =
    {
        "D22", "D5", "D21", "D2", "D20", "D19",
        "D4", "E3", "E5", "E7", "T8", "T11", "T13",
        "A10", "A1", "A15", "A2", "Y11", "Y9", "Y8",
        "S16", "S17", "S4", "S22"
    };
        string[] deliverables =
    {
        "wert", "sdfgh", "sdfg", "fgh", "xcdfv", "cxv",
        "sxdcfg", "dcfg", "dfgh", "xcvb", "fghj", "asdf", "erty",
        "dfgh", "dfgh", "sdfg", "sdfg", "dsfg", "rty", "fdgh",
        "fgh", "dfg", "tghyuj", "fghj"
    };
        string[] remarks =
  {
        "wert", "sdfgh", "sdfg", "fgh", "xcdfv", "cxv",
        "sxdcfg", "dcfg", "dfgh", "xcvb", "fghj", "asdf", "erty",
        "dfgh", "dfgh", "sdfg", "sdfg", "dsfg", "rty", "fdgh",
        "fgh", "dfg", "tghyuj", "fghj"
    };
        foreach (var _alias in taskAliases)
        {
            string? _description = _alias + "DESCRIPTION";

            bool? _stone = false;

            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _makeTask = start.AddDays(s_rand.Next(range));

            string? _deliverable = deliverables[s_rand.Next(deliverables.Length)];

            string? _remarks = remarks[s_rand.Next(remarks.Length)];

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);
            //DateTime _createTask = new DateTime(2000,1,1);
            //TimeSpan _range = (s_rand.Next(10, 100));
            //DateTime _startPlanning = new DateTime(s_rand.Next(2024, 2030), s_rand.Next(1, 13), s_rand.Next(1, 31));

            Task newTask = new(0, Description: _description, Alias: _alias, IsMilestone: _stone, CreatedAtDate: _makeTask,
                Deliverables: _deliverable, Remarks: _remarks, CopmlexityLevel: _level);

            s_dalTask!.Create(newTask);
        }
    }

    public static void Do(IDependency? s_dalDependency, IEngineer? s_dalEngineer, ITask? s_dalTask)
    {
        IDependency? dalDependency = null;
        IEngineer? dalEngineer = null;
        ITask? dalTask = null;

        createDependency();
        createEngineer();
        createTask();

        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
    }
}
