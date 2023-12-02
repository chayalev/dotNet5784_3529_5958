using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {

        //private static IDependency? s_dalDependency = new DependencyImplementation();
        //private static IEngineer? s_dalEngineer = new EngineerImplementation();
        //private static ITask? s_dalTask = new TaskImplementation();

        private static readonly IDal s_dal = new DalList(); //stage 2
        //function to print the options of any entity
        private static void PrintMenu(string entity)
        {
            Console.WriteLine(
              "If you want to create new " + entity + " press 2\n" +
              "if you want to get " + entity + " press 3\n" +
              "if you want to get all the " + entity + " press 4\n" +
              "if you want to update " + entity + " press 5\n" +
              "if you want to delete " + entity + " press 6\n" +
              "To exit press 1\n"
              );
        }
        //function to help to parse strings
        private static string? StringParse(string? _lastSt)
        {
            string st =Console.ReadLine()??"";
            if (string.IsNullOrEmpty(st))
                return _lastSt;
            return st;

        }
        //function to create the engineer
        private static Engineer CreateEngineer(int id)
        {
            Engineer? _engineer=new Engineer();
            int _result;
            double _resultD;
            Console.WriteLine("Press the values: ");
            Console.WriteLine("id,name,level,email,cost");
            //to get the engineer before update
            if (id != 0)
                _engineer = s_dalEngineer?.Read(id);
            //Get the valeus of engineer
            Engineer newEngineer = new Engineer()
            {
                Id = int.TryParse(Console.ReadLine(), out  _result)?_result: _engineer!.Id,
                Name = StringParse(_engineer?.Name),
                Level = (EngineerExperience?)(int.TryParse(Console.ReadLine(), out  _result) ? _result : (int?)_engineer?.Level),
                Email = StringParse(_engineer?.Email),
                Cost = double.TryParse(Console.ReadLine(), out  _resultD) ? _resultD : _engineer?.Cost
            };
            //return the values to create the entity
            return newEngineer;
        }

        //function to create the task
        private static DO.Task CreateTask(int id)
        {
            DO.Task? _task = new DO.Task();
            int _result;
            bool _resultB;
            DateTime _resultD;
            //if the user want to update the details
            if (id != 0)
                _task = s_dalTask?.Read(id);
            Console.WriteLine("Press the values: ");
            Console.WriteLine("Description,Alias,IsMilestone,CreatedAtDate,RequiredEffortTime,StartDate,ScheduledDate,DeadlineDate,CompleteDate,Deliverables,Remarks,CopmlexityLevel,EngineerId");
            //Get the valeus of task
            DO.Task newTask = new DO.Task()
            {
                Id=id,
                Description = StringParse(_task?.Description),
                Alias = StringParse(_task?.Alias),
                IsMilestone = bool.TryParse(Console.ReadLine(), out _resultB) ? _resultB : _task?.IsMilestone,
                CreatedAtDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                RequiredEffortTime=TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _resultT) ? _resultT : _task?.RequiredEffortTime,
                StartDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                ScheduledDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                DeadlineDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                CompleteDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                Deliverables = StringParse(_task?.Deliverables),
                Remarks = StringParse(_task?.Remarks),
                CopmlexityLevel = (EngineerExperience?)(int.TryParse(Console.ReadLine(), out int _result1) ? _result1 : (int?)_task?.CopmlexityLevel),
                EngineerId = int.TryParse(Console.ReadLine(), out _result) ? _result : _task?.EngineerId,
            };
            //return the values to create the entity
            return newTask;
        }

        //function to create the dependency
        private static Dependency CreateDependency(int id)
        {
            int _result;
            int? _dependsOnTask, _dependentTask;
            Dependency? _dependency = new Dependency();
            //to get the dependency before update
            if (id!= 0)
                _dependency = s_dalDependency?.Read(id);
            do
            {
                Console.WriteLine("Press the values: ");
                Console.WriteLine("DependentTask,DependsOnTask");
                //Get the valeus of dependency
                _dependentTask = int.TryParse(Console.ReadLine(), out _result) ? _result : _dependency?.DependentTask;
                _dependsOnTask = int.TryParse(Console.ReadLine(), out _result) ? _result : _dependency?.DependsOnTask;
            }
            //check that the values are fine
            while (_dependentTask == _dependsOnTask || (s_dalDependency?.ReadAll().Any(dep => dep.DependentTask == _dependsOnTask && dep.DependsOnTask == _dependentTask) ?? false));
            //return the values to create the entity
            Dependency newDep = new Dependency(id, _dependentTask, _dependsOnTask);
            return newDep;
        }

        //function to make the same actions of the entities
        //and send the different ones to the appropriate functions
        private static void SubMenu(string entity)
        {
            int chosenCrud = 0, _id, _result;
            //print the menu
            PrintMenu(entity);
            if (int.TryParse(Console.ReadLine(), out _result))
                chosenCrud = _result;
            try
            {
                while (chosenCrud != 1)
                {
                    switch (chosenCrud)
                    {
                        //create new entity
                        case 2:
                            try
                            {
                                //create Engineer
                                if (entity == "Engineer")
                                {
                                    s_dal?.Engineer.Create(CreateEngineer(0));
                                }
                                //create Dependency
                                else if (entity == "Dependency")
                                    s_dal?.Dependency.Create(CreateDependency(0));
                                //create Task
                                else
                                   s_dal?.Task.Create(CreateTask(0));
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        //read entity by id
                        case 3:
                            try
                            {
                                //get the id for the read
                                Console.WriteLine("press the id of the " + entity);
                                if (int.TryParse(Console.ReadLine(), out _result))
                                {
                                    _id = _result;
                                    //read Engineer
                                    if (entity == "Engineer")
                                        Console.WriteLine(s_dal?.Engineer.Read(_id) ?? throw new Exception("There is no engineer with id: " + _id));
                                    //read Dependency
                                    else if (entity == "Dependency")
                                        Console.WriteLine(s_dal?.Dependency.Read(_id) ?? throw new Exception("There is no dependency with id: " + _id));
                                    //read Task
                                    else
                                        Console.WriteLine(s_dal?.Task.Read(_id) ?? throw new Exception("There is no task with id: " + _id));
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        //read all the list
                        case 4:
                            try
                            {
                                //Printing all values ​​present in the list
                                //read Engineer
                                if (entity == "Engineer")
                                {
                                    List<Engineer>? allEng = s_dal?.Engineer.ReadAll() ?? throw new Exception("The list of engineers is empty ");
                                    foreach (var _eng in allEng)
                                        Console.WriteLine(_eng);
                                }

                                //read Dependency
                                else if (entity == "Dependency")
                                {
                                    List<Dependency>? allDep = s_dal?.Dependency.ReadAll() ?? throw new Exception("The list of dependencies is empty ");
                                    foreach (var _dep in allDep)
                                        Console.WriteLine(_dep);
                                }
                                //read Task
                                else
                                {
                                    List<DO.Task>? allTask = s_dal?.Task.ReadAll() ?? throw new Exception("The list of tasks is empty ");
                                    foreach (var _task in allTask)
                                        Console.WriteLine(_task);
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        //update
                        case 5:
                            try
                            {
                                //get the id to update the entity
                                Console.WriteLine("press the id of the " + entity);
                                if (int.TryParse(Console.ReadLine(), out _result))
                                {
                                    _id = _result;
                                    //update Engineer
                                    if (entity == "Engineer")
                                    {
                                        Console.WriteLine(s_dal?.Engineer?.Read(_id) ?? throw new Exception("There is no engineer with id: " + _id));
                                        s_dal.Engineer.Update(CreateEngineer(_id));
                                    }
                                    //update Dependency
                                    else if (entity == "Dependency")
                                    {
                                        Console.WriteLine(s_dal?.Dependency.Read(_id) ?? throw new Exception("There is no dependency with id: " + _id));
                                        s_dal.Dependency.Update(CreateDependency(_id));
                                    }
                                    //update Task
                                    else
                                    { 
                                        Console.WriteLine(s_dal?.Task.Read(_id) ?? throw new Exception("There is no task with id: " + _id));
                                        s_dal.Task.Update(CreateTask(_id));
                                    }
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        //delete
                        case 6:
                            try
                            {
                                //get the id to delete the entity
                                Console.WriteLine("press the id of the " + entity);
                                if (int.TryParse(Console.ReadLine(), out _result))
                                {
                                    _id = _result;
                                    //delete Engineer
                                    if (entity == "Engineer")
                                        s_dal?.Engineer.Delete(_id);
                                    //delete Dependency
                                    else if (entity == "Dependency")
                                        s_dal?.Dependency.Delete(_id);
                                    //delete Task
                                    else
                                        s_dal?.Task.Delete(_id);
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        default:
                            throw new Exception();
                    }
                    //Reprint of all the options given on the entity
                    PrintMenu(entity);
                    if (int.TryParse(Console.ReadLine(), out _result))
                        chosenCrud = _result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            int entity = 0, result;
            try
            {
                //Initialization the lists of entity
                Initialization.Do(s_dal);
                Console.WriteLine("Please press a number:\n 1-Engineer,2-Task,3-Dependency. \n 0 to exit");
                if (int.TryParse(Console.ReadLine(), out result))
                    entity = result;
                //The first menu to choose the entity
                while (entity != 0)
                {
                    switch (entity)
                    {
                        case 1:
                            //send to the menu of the Engineer
                            SubMenu("Engineer");
                            break;
                        case 2:
                            //send to the menu of the Task
                            SubMenu("Task");
                            break;
                        case 3:
                            //send to the menu of the Dependency
                            SubMenu("Dependency");
                            break;
                        default:
                            throw new Exception();

                    }
                    //Reprint all the entities options
                    Console.WriteLine("Please press a number:\n 1-Engineer,2-Task,3-Dependency. \n 0 to exit");
                    if (int.TryParse(Console.ReadLine(), out result))
                        entity = result;
                }

            }
            //catch the errors of the main
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}