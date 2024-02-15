
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Runtime.Intrinsics.Arm;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// The GetDependencies function to get the dependencies for a particular task
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    private List<BO.TaskInList> GetDependencies(int taskId)
    {
        ///Goes through the list of dependencies and takes all the tasks that the task depends on
        List<BO.TaskInList> dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == taskId)
        .Select(dep =>
        {
            ///Creating a task-type member in the list according to the values found in the task
            var currentTask = _dal.Task.Read(task => task.Id == dep?.DependsOnTask);
            int newid = dep?.DependsOnTask ?? throw new DalDoesNotExistException("");
            ///Calculates the status
            var status = _dal.startDate == null ? 0 : (Status)1;
            ///Returns a new member of type task in the list
            return new BO.TaskInList
            {
                Id = newid,
                Alias = currentTask?.Alias,
                Description = currentTask?.Description,
                Status = status
            };
        }).ToList();
        return dependencies;

    }

    /// <summary>
    /// create a new task
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="BlCantChangeAfterScheduled"></exception>
    /// <exception cref="wrongInput"></exception>
    public int Create(BO.Task item)
    {
        ///It is not possible to add a task after creating a project
        if (Factory.Get().IsCreate)
            throw new BlCantChangeAfterScheduledException("you cant add a task after a create a schedule");
        ///Checking that there is a description for the task
        if (item.Alias == null)
            throw new wrongInput("empty alias");
        ///Creating the task's dependencies
        item?.Dependencies?.ForEach(dep =>
        {
            var taskInList = _dal.Task.Read(dep.Id);
            if (taskInList != null)
            {
                dep.Id = taskInList.Id;
                dep.Description = taskInList.Description;
                dep.Alias = taskInList.Alias;
            }
        });
        ///Creating the new task
        DO.Task newTask = new DO.Task
        (0, item!.Description, item.RequiredEffortTime, item.Alias, false, item.CreateAtDate, item.StartDate, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        int idTask = _dal.Task.Create(newTask);
        ///Creating appropriate dependencies for the task
        item?.Dependencies?.ForEach(taskDep =>
                        {
                            var newDep = new DO.Dependency { DependentTask = idTask, DependsOnTask = taskDep.Id };
                            _dal.Dependency.Create(newDep);
                        });

        return idTask;
    }

    /// <summary>
    /// delete a task
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BlDeletionImpossible"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        ///Checking that the task does not precede other tasks
        if (_dal.Dependency.ReadAll(dep => dep.DependsOnTask == id).Any())
            throw new BlDeletionImpossibleException($"Task with ID={id} is in depended");
        ///Tasks cannot be deleted after the project schedule has been created
        if (_dal.Task.ReadAll(task => task.IsMilestone == true).Any())
            throw new BlDeletionImpossibleException($"Cannot delete a task after a Creating a schedule");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} doesnt exists", ex);
        }
    }

    /// <summary>
    /// get a task by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    /// <exception cref="DalDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {
        ///Bringing the task from the dal
        DO.Task doTask = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"task with ID={id} does Not exist");
        try
        {
            ///Calculates the status
            var status = _dal.startDate == null ? 0 : (Status)1;

            var milestoneInTask = _dal.Dependency.ReadAll()
             .Where(dep => dep?.DependentTask == id)
             .Select(dep => new MilestoneInTask
             {
                 Id = dep?.DependsOnTask ?? throw new DalDoesNotExistException(""),
                 Alias = _dal.Task.Read(dep.DependsOnTask.Value)?.Alias
             })
             .FirstOrDefault();
            ///Conversion from a database to the logic layer
            return new BO.Task()
            {
                Id = id,
                Alias = doTask.Alias,
                Description = doTask.Description,
                Dependencies = GetDependencies(id),
                StatusTask = status,
                CreateAtDate = doTask.CreatedAtDate,
                LinkMilestone = status == Status.Unscheduled ? null : milestoneInTask,
                StartDate = doTask.StartDate,
                BaselineStartDate = doTask.ScheduledDate,
                ForecastDate = null,
                DeadlineDate = doTask.DeadlineDate,
                ComleteDate = doTask.CompleteDate,
                Deliverables = doTask.Deliverables,
                Remarks = doTask.Remarks,
                Engineer = doTask.EngineerId != null
                ? new EngineerInTask { Id = (int)doTask.EngineerId, Name = _dal.Engineer.Read(id)?.Name }
                : null,
                ComplexityLevel = (BO.EngineerExperience?)doTask.CopmlexityLevel,
            };
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} doesnt exists", ex);
        }

    }

    /// <summary>
    /// get all the tasks or filtered task
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        ///Creating a new BO task list
        var allTasks = _dal.Task.ReadAll().Select(doTask => Read(doTask!.Id) ?? throw new BlDoesNotExistException("the tasks dont exist"));
        if (filter == null)
            return allTasks;
        else
            return allTasks.Where(filter);
    }
    
    /// <summary>
    /// update a task
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="BlWrongDate"></exception>
    /// <exception cref="wrongInput"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Update(BO.Task item)
    {
        DO.Task taskUpdate;
        ///Before creating a hazel
        if (!Factory.Get().IsCreate)
        {
            taskUpdate = new DO.Task
            (item.Id, item.Description, item.RequiredEffortTime, item.Alias, false, null, null, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
            ///If this task has dependent tasks
            if (item.Dependencies != null)
            {
                var depToDelete = _dal.Dependency.ReadAll(dep => dep?.DependentTask == item.Id);
                ///Deleting all dependencies that the task shares
                depToDelete.ToList().ForEach(dep =>
                    {
                        _dal.Dependency.Delete(dep!.Id);
                    });
                ///Changing the dependencies of the task
                item?.Dependencies?.ForEach(taskDep =>
                {
                    var newDep = new DO.Dependency { DependentTask = item.Id, DependsOnTask = taskDep.Id };
                    _dal.Dependency.Create(newDep);
                });
            }
        }
        ///After creating a project
        else
        {
            ///change the start date of a task - integrity
            if (item.StartDate != null)
            {
                ///If the start date is less than the end date of the tasks that depend on them
                if (!_dal.Dependency.ReadAll(dep => dep.DependentTask == item.Id).All(dep => _dal.Task.Read((int)dep?.DependsOnTask!)?.DeadlineDate <= item.StartDate))
                    throw new BlWrongDateException($"A start date is less than the end date of the tasks that task {item.Id} depends on");
            }
            ///If the start date is less than the start date of the entire project
            if (!_dal.Dependency.ReadAll().Any(dep => dep?.DependentTask == item.Id) && item.StartDate >= Factory.Get().StartDate)
            {
                throw new BlWrongDateException($" The task: {item.Id}- start date is earlier than the project start date");
            }
            ///If the start date is greater than the end date
            if (item.StartDate > item.DeadlineDate)
                throw new BlWrongDateException($"The task:{item.Id}  start date is later than the end date");
            ///When you want to update an engineer who already has another engineer for this task
            if (_dal.Task.Read(item.Id)?.EngineerId != null && _dal.Task.Read(item.Id)?.EngineerId != item.Engineer?.Id)
                throw new wrongInput($"The task:{item.Id} was taken by another engineer");
            ///End date calculator
            var deadLineDate = item.StartDate + item.RequiredEffortTime;
            ///duration calculator
            var requiredEffortTime = item.DeadlineDate - item.StartDate;
            taskUpdate = new DO.Task
           (item.Id, item.Description, requiredEffortTime, item.Alias, false, null, null, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        }
        ///Test Data
        try
        {
            _dal.Task.Update(taskUpdate);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item!.Id} already exists", ex);
        }
    }
}

