using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {



       //private static readonly IDal s_dal = new DalList(); //stage 2
        private static readonly IDal s_dal = new DalXml(); //stage 3

        /// <summary>
        /// function to print the options of any entity
        /// </summary>
        /// <param name="entity"></param>
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
        
        /// <summary>
        /// function to help to parse strings
        /// </summary>
        /// <param name="_lastSt"></param>
        /// <returns></returns>
        private static string? StringParse(string? _lastSt)
        {
            string st = Console.ReadLine() ?? "";
            if (string.IsNullOrEmpty(st))
                return _lastSt;
            return st;

        }
        
        /// <summary>
        /// function to create the engineer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Engineer CreateEngineer(int id)
        {
            Engineer? _engineer = new Engineer();
            int _result;
            double _resultD;
            Console.WriteLine("Press the values: ");
            Console.WriteLine("id,name,level,email,cost");
            //to get the engineer before update
            if (id != 0)
                _engineer = s_dal.Engineer?.Read(id);
            //Get the valeus of engineer
            Engineer newEngineer = new Engineer()
            {
                Id = int.TryParse(Console.ReadLine(), out _result) ? _result : _engineer!.Id,
                Name = StringParse(_engineer?.Name),
                Level = (EngineerExperience?)(int.TryParse(Console.ReadLine(), out _result) ? _result : (int?)_engineer?.Level),
                Email = StringParse(_engineer?.Email),
                Cost = double.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _engineer?.Cost
            };
            //return the values to create the entity
            return newEngineer;
        }

        /// <summary>
        /// function to create the task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static DO.Task CreateTask(int id)
        {
            DO.Task? _task = new DO.Task();
            int _result;
            bool _resultB;
            DateTime _resultD;
            //if the user want to update the details
            if (id != 0)
                _task = s_dal.Task?.Read(id);
            Console.WriteLine("Press the values: ");
            Console.WriteLine("Description,Alias,IsMilestone,CreatedAtDate,RequiredEffortTime,StartDate,ScheduledDate,DeadlineDate,CompleteDate,Deliverables,Remarks,CopmlexityLevel,EngineerId");
            //Get the valeus of task
            DO.Task newTask = new DO.Task()
            {
                Id = id,
                Description = StringParse(_task?.Description),
                Alias = StringParse(_task?.Alias),
                IsMilestone = bool.TryParse(Console.ReadLine(), out _resultB) ? _resultB : _task?.IsMilestone,
                CreatedAtDate = DateTime.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _task?.CreatedAtDate,
                RequiredEffortTime = TimeSpan.TryParse(Console.ReadLine(), out TimeSpan _resultT) ? _resultT : _task?.RequiredEffortTime,
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

        /// <summary>
        /// function to create the dependency
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static Dependency CreateDependency(int id)
        {
            int _result;
            int? _dependsOnTask, _dependentTask;
            Dependency? _dependency = new Dependency();
            //to get the dependency before update
            if (id != 0)
                _dependency = s_dal.Dependency?.Read(id);
            do
            {
                Console.WriteLine("Press the values: ");
                Console.WriteLine("DependentTask,DependsOnTask");
                //Get the valeus of dependency
                _dependentTask = int.TryParse(Console.ReadLine(), out _result) ? _result : _dependency?.DependentTask;
                _dependsOnTask = int.TryParse(Console.ReadLine(), out _result) ? _result : _dependency?.DependsOnTask;
            }
            //check that the values are match the instructions
            while (_dependentTask == _dependsOnTask || (s_dal.Dependency?.ReadAll().Any(dep => dep!.DependentTask == _dependsOnTask && dep!.DependsOnTask == _dependentTask) ?? false));
            //return the values to create the entity
            Dependency newDep = new Dependency(id, _dependentTask, _dependsOnTask);
            return newDep;
        }

        /// <summary>
        /// function to make actions of the entities and send them to the appropriate functions
        /// </summary>
        /// <param name="entity"></param>
        /// <exception cref="DalDoesNotExistException"></exception>
        /// <exception cref="wrongInput"></exception>
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
                                        Console.WriteLine(s_dal?.Engineer.Read(_id) ?? throw new DalDoesNotExistException($"Engineer with ID={_id} does not exists"));
                                    //read Dependency
                                    else if (entity == "Dependency")
                                        Console.WriteLine(s_dal?.Dependency.Read(_id) ?? throw new DalDoesNotExistException($"Dependency with ID={_id} does not exists"));
                                    //read Task
                                    else
                                        Console.WriteLine(s_dal?.Task.Read(_id) ?? throw new DalDoesNotExistException($"Task with ID={_id} does not exists"));
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
                                    List<Engineer?> allEng = s_dal?.Engineer.ReadAll().ToList() ?? throw new DalDoesNotExistException("Engineers do not exists");
                                    foreach (var _eng in allEng)
                                        Console.WriteLine(_eng);
                                }

                                //read Dependency
                                else if (entity == "Dependency")
                                {
                                    List<Dependency?> allDep = s_dal?.Dependency.ReadAll().ToList() ?? throw new DalDoesNotExistException("Dependencies do not exists");
                                    foreach (var _dep in allDep)
                                        Console.WriteLine(_dep);
                                }
                                //read Task
                                else
                                {
                                    List<DO.Task?> allTask = s_dal?.Task.ReadAll().ToList() ?? throw new DalDoesNotExistException("Tasks do not exists");
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
                                        Console.WriteLine(s_dal?.Engineer.Read(_id) ?? throw new DalDoesNotExistException($"Engineer with ID= {_id} does not exists"));
                                        s_dal.Engineer.Update(CreateEngineer(_id));
                                    }
                                    //update Dependency
                                    else if (entity == "Dependency")
                                    {
                                        Console.WriteLine(s_dal?.Dependency.Read(_id) ?? throw new DalDoesNotExistException($"Dependency with ID= {_id} does not exists"));
                                        s_dal.Dependency.Update(CreateDependency(_id));
                                    }
                                    //update Task
                                    else
                                    {
                                        Console.WriteLine(s_dal?.Task.Read(_id) ?? throw new DalDoesNotExistException($"Task with ID= {_id} does not exists"));
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
                            throw new wrongInput("wrong input");
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
                Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                if (ans == "Y") //stage 3
                {
                    s_dal.Reset();
                    Initialization.Do(s_dal);
                }

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
                            throw new wrongInput("wrong input");
                    }

                }
                //Reprint all the entities options
                Console.WriteLine("Please press a number:\n 1-Engineer,2-Task,3-Dependency. \n 0 to exit");
                if (int.TryParse(Console.ReadLine(), out result))
                    entity = result;
            }
            //catch the errors of the main
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
