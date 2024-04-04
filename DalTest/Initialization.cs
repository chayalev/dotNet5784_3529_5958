
//using static System.Net.Mime.MediaTypeNames;
//using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Reflection;


//using Dal;
//using DalApi;
//using DO;
//using System.Runtime.Intrinsics.Arm;

//namespace DalTest;

//public static class Initialization
//{
//    /// <summary>
//    /// Activation of the relevant data base
//    /// </summary>
//    //private static IDal? s_dal = new DalList();//stage 2
//    private static IDal? s_dal = DalApi.Factory.Get;

//    /// <summary>
//    /// Random number
//    /// </summary>
//    private static readonly Random s_rand = new();

//    private static void createDependency()
//    {
//        int? _dependentTask = null, _dependsOnTask = null;
//        //counter to the random task
//        int countTasks = s_dal?.Task.ReadAll().Count() ?? 1;
//        for (int i = 0; i < 40; i++)
//        {
//            //Generates a random number from the id of all lists
//            _dependentTask = s_dal?.Task.ReadAll().ToArray()[s_rand.Next(1, countTasks)]?.Id;
//            do
//                _dependsOnTask = s_dal?.Task.ReadAll().ToArray()[s_rand.Next(1, countTasks)]?.Id;
//            while (_dependentTask == _dependsOnTask || (s_dal?.Dependency.ReadAll().FirstOrDefault(dep => dep!.DependentTask == _dependsOnTask && dep.DependsOnTask == _dependentTask) != null));
//            //create 3 dependencies with same depended Task
//            if (i == 15)
//            {
//                for (int j = 15; j < 18; j++)
//                {
//                    Dependency newDependency1 = new(0, 12, j);
//                    s_dal?.Dependency.Create(newDependency1);
//                }
//                for (int j = 15; j < 18; j++)
//                {
//                    Dependency newDependency1 = new(0, 13, j);
//                    s_dal?.Dependency.Create(newDependency1);
//                }
//            }
//            //create the new task
//            Dependency newDependency = new(0, _dependentTask, _dependsOnTask);
//            s_dal?.Dependency.Create(newDependency);
//        }
//    }
//    private static void createEngineer()
//    {
//        //array for all the names
//        string[] engineerNames =
//    {
//        "Dani Levi", "Eli Amar", "Yair Cohen",
//        "Ariela Levin", "Dina Klein", "Shira Israelof"
//    };

//        foreach (var _name in engineerNames)
//        {
//            int _id;
//            //random id and check that nobody use it before
//            do
//                _id = s_rand.Next(200000000, 400000000);
//            while (s_dal?.Engineer.Read(_id) != null);

//            string _email = _name.Split(' ')[0] + "@gmail.com";

//            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);

//            double _cost = s_rand.NextDouble() * (500.0 - 100.0) + 100;

//            //create the engineer
//            Engineer newEngineer = new(_id, _name, _level, _email, _cost);

//            s_dal?.Engineer.Create(newEngineer);
//        }

//    }
//    private static void createTask()
//    {
//        string[] taskAliases =
//    {
//        "D22", "D5", "D21", "D2", "D20", "D19",
//        "D4", "E3", "E5", "E7", "T8", "T11", "T13",
//        "A10", "A1", "A15", "A2", "Y11", "Y9", "Y8",
//        "S16", "S17", "S4", "S22"
//    };
//        string[] deliverables =
//    {
//        "Algorithmic data compression", "Secure authentication design", "Scalable cloud solution", "Real-time computer vision", "Fraud detection model", "Efficient database management",
//        "High-performance web app", "Natural language processing", "Blockchain application", "Mobile task management", "Distributed computing", "Augmented reality education", "IoT cybersecurity",
//        "Responsive e-commerce", "Personalized content recommendation", "Speech recognition", "Social media analytics", "Software-defined networking", "Software-defined networking", "Virtual reality gaming",
//            "fgh", "dfg", "tghyuj", "fghj"
//    };

//        foreach (var _alias in taskAliases)
//        {
//            string? _description = _alias + "DESCRIPTION";

//            bool? _stone = false;

//            //start date 
//            DateTime start = new DateTime(2000, 1, 1);
//            int range = (DateTime.Today - start).Days;
//            DateTime _startTask = start.AddDays(s_rand.Next(range));

//            //last date
//            DateTime _completeDate = _startTask.AddDays(s_rand.Next(range));

//            string? _deliverable = deliverables[_alias.IndexOf(_alias)];

//            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 5);

//            //create the task
//            DO.Task newTask = new(0, Description: _description, Alias: _alias, IsMilestone: _stone, StartDate: _startTask,
//                Deliverables: _deliverable, CopmlexityLevel: _level);

//           s_dal?.Task.Create(newTask);
//        }
//    }

//    public static void Do()
//    {
//        //creates the entity lists

//        s_dal = DalApi.Factory.Get; //stage 4

//        createEngineer();
//        createTask();
//        createDependency();
//    }
//    public static void Reset()
//    {
//        s_dal?.Reset();
//    }
//}
namespace DalTest;


using Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Security.Cryptography;
using System.Xml.Linq;

/// <summary>
/// class initialization of all the entities
/// </summary>
public static class Initialization
{
    //////declaration  the objects of their entities
    ////private static IEngineer? e_dalEngineer;     //stage1
    ////private static IDependency? d_dalDependency; //stage1
    ////private static ITask? t_dalTask;             //stage1

    private static IDal? s_dal=DalApi.Factory.Get;

 
    private static readonly Random s_rand = new();
    /// <summary>
    /// create objects Engineer type
    /// </summary>
    private static void CreateEngineer()
    {
        //create array for the names of engineer
        string[] EngineerNames =
        {
            "Kewin Klein", "Dan Segal",
            "Barbara Sol", "Alice Weiss"
        };
        //initialize all the details of engineer by goes through the array
        foreach (var _name in EngineerNames)
        {
            int _id;
            do
                _id = s_rand.Next(200000000, 400000000);
            while (s_dal!.Engineer.Read(_id) != null);
            EngineerExperience _level = (EngineerExperience)s_rand.Next(0, 4);
            double? _cost = s_rand.Next(1000, 400000);
            string? _email = _name + "@gmail.com";
            //creating a new object
            Engineer newEng = new(_id, _name, _level, _email, _cost);
            //Add to data by calling create operation
            _id = s_dal!.Engineer.Create(newEng);
        }
    }
    private static void CreateTask()
    {
        int firstTask, secondTask, thirdTask, fourthTask,fifthTask;
        Task tsk = new(0, "Planning the appearance of the web page", TimeSpan.FromDays(5), "first Planing", false, null,null, null, null, null, null, null,null,(EngineerExperience)1) ;
        firstTask = s_dal!.Task.Create(tsk);
        tsk = new(0, "Building a database with appropriate values", TimeSpan.FromDays(3), "build database", false, null, null, null, null, null, null, null, null, (EngineerExperience)3);
        secondTask = s_dal!.Task.Create(tsk);
        tsk = new(0, "Writing server and client code", TimeSpan.FromDays(10), "code", false, null,null, null, null, null, null, null, null, (EngineerExperience)1);
        thirdTask = s_dal!.Task.Create(tsk);
        tsk = new(0, "User page display design and code usage", TimeSpan.FromDays(7), "design", false, null, null, null, null, null, null, null, null, (EngineerExperience)2);
        fourthTask = s_dal!.Task.Create(tsk);
        tsk = new(0, "organize the computer", TimeSpan.FromDays(3), "organized", false, null, null, null, null, null, null, null, null, (EngineerExperience)1);
        fifthTask = s_dal!.Task.Create(tsk);

        Dependency dependency = new Dependency(0, secondTask, firstTask);
        int depId = s_dal.Dependency.Create(dependency);
        dependency = new Dependency(0, thirdTask, secondTask);
        depId = s_dal.Dependency.Create(dependency);
        dependency = new Dependency(0, fourthTask, firstTask);
        depId = s_dal!.Dependency.Create(dependency);
    }
    public static void Reset()
    {
        s_dal?.Reset();
    }
    public static void Do()
    {
        s_dal = DalApi.Factory.Get;
        //Calling all the initializiation function of the different entitites
        CreateEngineer();
        CreateTask();
    }
}
