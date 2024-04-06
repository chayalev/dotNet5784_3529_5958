

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
{

    /// <summary>
    /// Check the id of Engineer
    /// </summary>
    /// <param name="id"></param>
    /// <returns>valid id</returns>
    private static int GetValidId(int id)
    {
        string idString = id.ToString("D9"); // Ensure a fixed length of 9 digits

        int sum = 0;
        for (int i = 0; i < idString.Length; i++)
        {
            int digit = int.Parse(idString[i].ToString()) * ((i % 2) + 1);
            sum += digit > 9 ? digit - 9 : digit;
        }

        if (sum % 10 == 0)
        {
            return id;
        }
        else
        {
            throw new BlWrongDateException($"Id {id} is not valid");
        }
    }

    /// <summary>
    /// Check the email of engineer
    /// </summary>
    /// <param name="email"></param>
    /// <returns>valid email</returns>
    /// <exception cref="BlWrongDateException"></exception>
    private static string GetValidEmail(string? email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);

        if (email != null && regex.IsMatch(email))
        {
            return email;
        }
        else
        {
            throw new BlWrongDateException($"Email {email} is not valid");
        }
    }
    /// <summary>
    /// creat a new engineer
    /// </summary>
    /// <param name="eng"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Engineer eng)
    {
        DO.Engineer doEngineer = new DO.Engineer
          (GetValidId(eng.Id), eng.Name, (DO.EngineerExperience?)eng.Level, GetValidEmail(eng.Email), eng.Cost);
        try
        {
            int idStud = Bl._dal.Engineer.Create(doEngineer);
            return idStud;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={eng.Id} already exists", ex);
        }
    }

    /// <summary>
    /// delete an engineer
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlWrongInput"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        BO.Engineer? boEng = this.Read(id);
        ///integrity - engineer not in the middle of a task
        if (boEng?.Task != null)
            throw new BO.BlWrongInputException("Engineer in the middle of task");
        ///integrity - there is an engineer with this ID number
        if (Bl._dal.Engineer.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Engineer ID = {id} does not exist");
        else
            Bl._dal.Engineer.Delete(id);
    }

    /// <summary>
    /// get an engineer by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = Bl._dal.Engineer.Read(id);
        BO.TaskInEngineer? taskInEngineer = null;
        ///integrity - there is an engineer with this ID number
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        ///Accepting the engineer's assignment
        var task = Bl._dal.Task.Read(task => task.EngineerId == id);
        if (task != null)
            taskInEngineer = new TaskInEngineer { Id = task!.Id, Alias = task.Alias };
        ///Creating and returning the engineer
        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience?)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = taskInEngineer
        };
    }

    /// <summary>
    /// get the all engineers
    /// </summary>
    /// <returns></returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter)
    {
        var allEngineers = Bl._dal.Engineer.ReadAll().Select(eng => Read(eng!.Id) ?? throw new BlDoesNotExistException("the tasks dont exist"));
        if (filter == null)
            return allEngineers;
        else
            return allEngineers.Where(filter);

    }

    /// <summary>
    /// update an engineer
    /// </summary>
    /// <param name="eng"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Update(BO.Engineer eng)
    {
        ///Checking if this engineer's task is dependent on another task
        var dependencies = Bl._dal.Dependency.ReadAll(dep => dep.DependentTask == eng.Task?.Id);
        ///Checking if all the tasks that this task depends on have already been done
        bool isAllDone = dependencies.All(dep => Factory.Get().Task.Read((int)dep!.DependsOnTask!)?.StatusTask == Status.Done);
        if (isAllDone)
        {
            ///Finding the task that belonged to this engineer
            var deleteTaskInEng = Bl._dal.Task.Read(task => task.EngineerId == eng.Id);
            if (deleteTaskInEng != null)
            {
                ///Update of the task that the engineer was deleted
                Bl._dal.Task.Update(deleteTaskInEng with { EngineerId = null });
            }
            if (eng.Task != null)
            {
                var newTask = Bl._dal.Task.Read(task => task.Id == eng.Task?.Id);
                var prevEng = Bl._dal.Engineer.Read(eng.Id);
                ///Checking that the new task is without a registered engineer and that it matches the level of the engineer
                if (newTask != null && newTask.EngineerId == null && (int?)newTask.CopmlexityLevel <= (int?)eng.Level && (int?)eng.Level >= (int?)prevEng?.Level)
                    Bl._dal.Task.Update(newTask with { EngineerId = eng.Id });
            }
        }
        else
            throw new BlWrongInputException($"you cant take task : {eng.Task!.Alias} because all the tasks that depended on it werent taken");
        ///Creating the engineer with the new data
        DO.Engineer doEngineer = new DO.Engineer
          (eng.Id, eng.Name, (DO.EngineerExperience?)eng.Level, eng.Email, eng.Cost);
        try
        {
            Bl._dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={eng.Id} already exists", ex);
        }
    }

    public IEnumerable<EngineerInTask> AllEngineerInTask()
    {
        return ReadAll(eng => eng.Task == null).Select(eng => new EngineerInTask
        {
            Id = eng.Id,
            Name = eng.Name,
        });
    }

}

