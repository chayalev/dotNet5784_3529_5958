using Dal;
using DalApi;
using DO;

namespace DalTest
{
    internal class Program
    {

        private static IDependency? s_dalDependency = new DependencyImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static ITask? s_dalTask = new TaskImplementation();

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

        //function to create the engineer
        private static Engineer CreateEngineer(int id)
        {
            Console.WriteLine("Press the values: ");
            Console.WriteLine("id,name,level,email,cost");
            //Get the valeus of engineer
            Engineer newEngineer = new Engineer()
            {
                Id = int.Parse(Console.ReadLine()??""),
                Name = Console.ReadLine(),
                Level = (EngineerExperience)int.Parse(Console.ReadLine() ?? ""),
                Email = Console.ReadLine() ?? "",
                Cost = double.Parse(Console.ReadLine() ?? "0")
            };
            //return the values to create the entity
            return newEngineer;
        }

        //function to create the task
        private static DO.Task CreateTask(int id)
        {
            Console.WriteLine("Press the values: ");
            Console.WriteLine("Description,Alias,IsMilestone,CreatedAtDate,StartDate,ScheduledDate,DeadlineDate,CompleteDate,Deliverables,Remarks,CopmlexityLevel,EngineerId");
            //Get the valeus of task
            DO.Task newTask = new DO.Task()
            {
                Id=id,
                Description = Console.ReadLine() ?? "",
                Alias = Console.ReadLine() ?? "",
                IsMilestone = bool.Parse(Console.ReadLine() ?? "false"),
                CreatedAtDate = DateTime.Parse(Console.ReadLine() ?? ""),
                StartDate = DateTime.Parse(Console.ReadLine() ?? ""),
                ScheduledDate = DateTime.Parse(Console.ReadLine() ?? ""),
                DeadlineDate = DateTime.Parse(Console.ReadLine() ?? ""),
                CompleteDate = DateTime.Parse(Console.ReadLine() ?? ""),
                Deliverables = Console.ReadLine(),
                Remarks = Console.ReadLine(),
                CopmlexityLevel = (EngineerExperience)int.Parse(Console.ReadLine() ?? ""),
                EngineerId = int.Parse(Console.ReadLine() ?? "")
            };
            //return the values to create the entity
            return newTask;
        }

        //function to create the dependency
        private static Dependency CreateDependency(int id)
        {
            Console.WriteLine("Press the values: ");
            Console.WriteLine("DependentTask,DependsOnTask");
            //Get the valeus of dependency
            Dependency newDep = new Dependency()
            {
                Id = id,
                DependentTask = int.Parse(Console.ReadLine() ?? throw new Exception("No information entered")),
                DependsOnTask = int.Parse(Console.ReadLine() ?? throw new Exception("No information entered"))
            };
            //return the values to create the entity
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
                                    s_dalEngineer?.Create(CreateEngineer(0));
                                }
                                //create Dependency
                                else if (entity == "Dependency")
                                    s_dalDependency?.Create(CreateDependency(0));
                                //create Task
                                else
                                    s_dalTask?.Create(CreateTask(0));
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
                                        Console.WriteLine(s_dalEngineer?.Read(_id) ?? throw new Exception("There is no engineer with id: " + _id));
                                    //read Dependency
                                    else if (entity == "Dependency")
                                        Console.WriteLine(s_dalDependency?.Read(_id) ?? throw new Exception("There is no dependency with id: " + _id));
                                    //read Task
                                    else
                                        Console.WriteLine(s_dalTask?.Read(_id) ?? throw new Exception("There is no task with id: " + _id));
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
                                    List<Engineer>? allEng = s_dalEngineer?.ReadAll() ?? throw new Exception("The list of engineers is empty ");
                                    foreach (var _eng in allEng)
                                        Console.WriteLine(_eng);
                                }

                                //read Dependency
                                else if (entity == "Dependency")
                                {
                                    List<Dependency>? allDep = s_dalDependency?.ReadAll() ?? throw new Exception("The list of dependencies is empty ");
                                    foreach (var _dep in allDep)
                                        Console.WriteLine(_dep);
                                }
                                //read Task
                                else
                                {
                                    List<DO.Task>? allTask = s_dalTask?.ReadAll() ?? throw new Exception("The list of tasks is empty ");
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
                                        Console.WriteLine(s_dalEngineer?.Read(_id) ?? throw new Exception("There is no engineer with id: " + _id));
                                        s_dalEngineer.Update(CreateEngineer(_id));
                                    }
                                    //update Dependency
                                    else if (entity == "Dependency")
                                    {
                                        Console.WriteLine(s_dalDependency?.Read(_id) ?? throw new Exception("There is no dependency with id: " + _id));
                                        s_dalDependency.Update(CreateDependency(_id));
                                    }
                                    //update Task
                                    else
                                    { 
                                        Console.WriteLine(s_dalTask?.Read(_id) ?? throw new Exception("There is no task with id: " + _id));
                                        s_dalTask.Update(CreateTask(_id));
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
                                        s_dalEngineer?.Delete(_id);
                                    //delete Dependency
                                    else if (entity == "Dependency")
                                        s_dalDependency?.Delete(_id);
                                    //delete Task
                                    else
                                        s_dalTask?.Delete(_id);
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
                Initialization.Do(s_dalDependency, s_dalEngineer, s_dalTask);
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