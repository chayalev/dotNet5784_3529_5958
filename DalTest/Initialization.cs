
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace DalTest;

using Dal;
using DalApi;
using DO;
using System.Runtime.Intrinsics.Arm;

public static class Initialization
{
    //private static IDependency? s_dalDependency = new DependencyImplementation();
    //private static IEngineer? s_dalEngineer = new EngineerImplementation();
    //private static ITask? s_dalTask = new TaskImplementation();
    private static IDal? s_dal = new DalList();

    //Random number
    private static readonly Random s_rand = new();

    private static void createDependency()
    {
        int? _dependentTask = null, _dependsOnTask = null;
        //counter to the random task
        int countTasks = s_dal?.Task.ReadAll().Count() ?? 1;
        for (int i = 0; i < 40; i++)
        {
            //Generates a random number from the id of all lists
            _dependentTask = s_dal?.Task.ReadAll().ToArray()[s_rand.Next(1, countTasks)]?.Id;
            do
                _dependsOnTask = s_dal?.Task.ReadAll().ToArray()[s_rand.Next(1, countTasks)]?.Id;
            while (_dependentTask == _dependsOnTask || (s_dal?.Dependency.ReadAll().Any(dep => dep!.DependentTask == _dependsOnTask && dep.DependsOnTask == _dependentTask) ?? false));
            //create 3 dependencies with same depended Task
            if (i == 15)
            {
                for (int j = 15; j < 18; j++)
                {
                    Dependency newDependency1 = new(0, 12, j);
                    s_dal?.Dependency.Create(newDependency1);
                }
                for (int j = 15; j < 18; j++)
                {
                    Dependency newDependency1 = new(0, 13, j);
                    s_dal?.Dependency.Create(newDependency1);
                }
            }
            //create the new task
            Dependency newDependency = new(0, _dependentTask, _dependsOnTask);
            s_dal?.Dependency.Create(newDependency);
        }
    }
    private static void createEngineer()
    {
        //array for all the names
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
            while (s_dal?.Engineer.Read(_id) != null);

            string _email = _name.Split(' ')[0] + "@gmail.com";

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);

            double _cost = s_rand.NextDouble() * (500.0 - 100.0) + 100;

            //create the engineer
            Engineer newEngineer = new(_id, _name, _level, _email, _cost);

            s_dal?.Engineer.Create(newEngineer);
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
        "Algorithmic data compression", "Secure authentication design", "Scalable cloud solution", "Real-time computer vision", "Fraud detection model", "Efficient database management",
        "High-performance web app", "Natural language processing", "Blockchain application", "Mobile task management", "Distributed computing", "Augmented reality education", "IoT cybersecurity",
        "Responsive e-commerce", "Personalized content recommendation", "Speech recognition", "Social media analytics", "Software-defined networking", "Software-defined networking", "Virtual reality gaming",
            "fgh", "dfg", "tghyuj", "fghj"
    };
       
        foreach (var _alias in taskAliases)
        {
            string? _description = _alias + "DESCRIPTION";

            bool? _stone = false;

            //start date 
            DateTime start = new DateTime(2000, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime _startTask = start.AddDays(s_rand.Next(range));

            //last date
            DateTime _completeDate = _startTask.AddDays(s_rand.Next(range));
          
            string? _deliverable = deliverables[_alias.IndexOf(_alias)];

            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);
           
            //create the task
            Task newTask = new(0, Description: _description, Alias: _alias, IsMilestone: _stone, StartDate: _startTask,
                Deliverables: _deliverable, CopmlexityLevel: _level);

           s_dal?.Task.Create(newTask);
        }
    }

    public static void Do(IDal dal)
    {

        //s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2

        //IDependency? dalDependency = null;
        //IEngineer? dalEngineer = null;
        //ITask? dalTask = null;

        ////creates the entity lists
        createEngineer();
        createTask();
        createDependency();
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); //stage 2


        //dalDependency= s_dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        //dalEngineer = s_dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        //dalTask = s_dalTask ?? throw new NullReferenceException("DAL can not be null!");
    }
}
