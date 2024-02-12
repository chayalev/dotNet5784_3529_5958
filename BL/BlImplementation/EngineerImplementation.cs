

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer eng)
    {
        DO.Engineer doEngineer = new DO.Engineer
          (eng.Id, eng.Name, (DO.EngineerExperience?)eng.Level, eng.Email, eng.Cost);
        try
        {
            int idStud = _dal.Engineer.Create(doEngineer);
            return idStud;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={eng.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Engineer? boEng = this.Read(id);
        if (boEng?.Task != null)
            throw new BO.BlWrongInput("Engineer in the middle of task");
        if (_dal.Engineer.Read(id) == null)
            throw new BO.BlDoesNotExistException($"Engineer ID = {id} does not exist");
        else
            _dal.Engineer.Delete(id);

    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        BO.TaskInEngineer? taskInEngineer = null;
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        var task = _dal.Task.Read(task => task.EngineerId == id);
        if (task != null)
            taskInEngineer = new TaskInEngineer { Id = task!.Id, Alias = task.Alias };
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

    public IEnumerable<BO.Engineer> ReadAll()
    {
        //return _dal.Engineer.ReadAll().Select(eng => Read(eng!.Id)!);
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience?)doEngineer.Level,
                    Cost = doEngineer.Cost,
                });

    }

    public void Update(BO.Engineer eng)
    {
        var dependencies=_dal.Dependency.ReadAll(dep => dep.DependentTask == eng.Task?.Id);
        bool isAllDone = dependencies.All(dep => Factory.Get().Task.Read((int)dep!.DependsOnTask!)?.StatusTask == Status.Done);
        if (isAllDone)
        {

            var deleteTaskInEng = _dal.Task.Read(task => task.EngineerId == eng.Id);
            if (deleteTaskInEng != null)
            {
                _dal.Task.Update(deleteTaskInEng with { EngineerId = null });

            }
            if (eng.Task != null)
            {
                var newTask = _dal.Task.Read(task => task.Id == eng.Task?.Id);
                if (newTask != null && newTask.EngineerId == null && (int?)newTask.CopmlexityLevel <= (int?)eng.Level)
                    _dal.Task.Update(newTask with { EngineerId = eng.Id });
            }
        }
        DO.Engineer doEngineer = new DO.Engineer
          (eng.Id, eng.Name, (DO.EngineerExperience?)eng.Level, eng.Email, eng.Cost);
        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={eng.Id} already exists", ex);
        }
    }
}

