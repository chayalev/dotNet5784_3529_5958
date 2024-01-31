﻿using BlApi;
using DalApi;
using BO;
using System.Runtime.Intrinsics.Arm;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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
        private static BO.Engineer CreateEngineer(int id)
        {
            BO.Engineer? _engineer = new Engineer();
            int _result;
            double _resultD;
            Console.WriteLine("Press the values: ");
            //to get the engineer before update
            if (id != 0)
                _engineer = s_bl.Engineer?.Read(id);
            else
                Console.Write("id, ");
            Console.WriteLine("name, level, email, cost, task - id and alias");

            //Get the valeus of engineer
            Engineer newEngineer = new Engineer()
            {
                Id = id==0 ? (int.TryParse(Console.ReadLine(), out _result) ? _result : _engineer!.Id) : id,
                Name = StringParse(_engineer?.Name),
                Level = (EngineerExperience?)(int.TryParse(Console.ReadLine(), out _result) ? _result : (int?)_engineer?.Level),
                Email = StringParse(_engineer?.Email),
                Cost = double.TryParse(Console.ReadLine(), out _resultD) ? _resultD : _engineer?.Cost,
                Task = null,
            };
            //return the values to create the entity
            return newEngineer;
        }
        private static List<TaskInList> insertDependencies()
        {

            List<TaskInList> dependencies = new List<TaskInList>();
            int _result;
            Console.WriteLine("insert the list of dependencies till -1");
            int dep = int.TryParse(Console.ReadLine(), out _result) ? _result : -1;
            while (dep != -1)
            {
                dependencies.Add(new TaskInList { Id = dep });
                dep = int.TryParse(Console.ReadLine(), out _result) ? _result : -1;
            }
            return dependencies;
        }
        /// <summary>
        /// function to create the task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static BO.Task CreateTask(int id)
        {
            BO.Task? _task = new BO.Task();
            int _result;
            //if the user want to update the details
            if (id != 0)
                _task = s_bl.Task?.Read(id);
            Console.WriteLine("Press the values: ");
            Console.WriteLine("Description,Alias,Deliverables,Remarks,CopmlexityLevel,EngineerId,");
            //Get the valeus of task
            BO.Task newTask = new BO.Task()
            {
                Id = id,
                Description = StringParse(_task?.Description),
                Alias = StringParse(_task?.Alias),
                Deliverables = StringParse(_task?.Deliverables),
                Remarks = StringParse(_task?.Remarks),
                ComplexityLevel = (BO.EngineerExperience?)(int.TryParse(Console.ReadLine(), out _result) ? _result : (int?)_task?.ComplexityLevel),
                Engineer = _task?.Engineer,
                Dependencies = insertDependencies(),
            };
            //return the values to create the entity
            return newTask;
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
                                    s_bl?.Engineer.Create(CreateEngineer(0));
                                }
                                //create Milestone
                                else if (entity == "Milestone")
                                    s_bl?.Milestone.Create();
                                //create Task
                                else
                                    s_bl?.Task.Create(CreateTask(0));
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
                                        Console.WriteLine(s_bl?.Engineer.Read(_id) ?? throw new BlDoesNotExistException($"Engineer with ID={_id} does not exists"));
                                    //read Milestone
                                    else if (entity == "Milestone")
                                        Console.WriteLine(s_bl?.Milestone.Read(_id) ?? throw new BlDoesNotExistException($"Dependency with ID={_id} does not exists"));
                                    //read Task
                                    else
                                        Console.WriteLine(s_bl?.Task.Read(_id) ?? throw new BlDoesNotExistException($"Task with ID={_id} does not exists"));
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
                                    List<Engineer> allEng = s_bl?.Engineer.ReadAll().ToList() ?? throw new BlDoesNotExistException("Engineers do not exists");
                                    foreach (var _eng in allEng)
                                        Console.WriteLine(_eng);
                                }

                                //read Task
                                else
                                {
                                    List<BO.Task> allTask = s_bl?.Task.ReadAll().ToList() ?? throw new BlDoesNotExistException("Tasks do not exists");
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
                                        Console.WriteLine(s_bl?.Engineer.Read(_id) ?? throw new BlDoesNotExistException($"Engineer with ID= {_id} does not exists"));
                                        s_bl.Engineer.Update(CreateEngineer(_id));
                                    }
                                    ////update Milestone
                                    //else if (entity == "Milestone")
                                    //{
                                    //    Console.WriteLine(s_bl?.Milestone.Read(_id) ?? throw new BlDoesNotExistException($"Dependency with ID= {_id} does not exists"));
                                    //    s_bl.Milestone.Update(CreateMilestone(_id));
                                    //}
                                    //update Task
                                    else
                                    {
                                        Console.WriteLine(s_bl?.Task.Read(_id) ?? throw new BlDoesNotExistException($"Task with ID= {_id} does not exists"));
                                        s_bl.Task.Update(CreateTask(_id));
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
                                        s_bl?.Engineer.Delete(_id);

                                    //delete Task
                                    else
                                        s_bl?.Task.Delete(_id);
                                }
                            }
                            catch (Exception ex) { Console.WriteLine(ex.Message); }
                            break;
                        default:
                            throw new Exception("wrong input");
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
                Console.Write("Would you like to create Initial data? (Y/N)");
                string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
                if (ans == "Y")
                    DalTest.Initialization.Do();
                else if (ans != "N")
                    throw new BlWrongInput("wrong input only Y / N accepted");
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
                            throw new Exception("wrong input");
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
