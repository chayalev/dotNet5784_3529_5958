﻿
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.Arm;

internal class TaskImplementation : ITask
{
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;

    /// <summary>
    /// The GetDependencies function to get the dependencies for a particular task
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    /// <exception cref="DalDoesNotExistException"></exception>
    private List<BO.TaskInList> GetDependencies(int taskId)
    {
        ///Goes through the list of dependencies and takes all the tasks that the task depends on
        List<BO.TaskInList> dependencies = Bl._dal.Dependency.ReadAll(dep => dep.DependentTask == taskId)
        .Select(dep =>
        {
            ///Creating a task-type member in the list according to the values found in the task
            var currentTask = Bl._dal.Task.Read(task => task.Id == dep?.DependsOnTask);
            int newid = dep?.DependsOnTask ?? throw new DalDoesNotExistException("");
            ///Calculates the status
            var status = _bl.IsCreate ? 0 : (Status)1;
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
    private bool ImpossibleDependency(BO.Task task)
    {
        Queue<BO.TaskInList> taskQueue = new Queue<BO.TaskInList>(task.Dependencies!);
        while (taskQueue.Count() != 0)
        {
            var currentTask = taskQueue.Dequeue();
            if (currentTask.Id == task.Id)
                return false;
            Read(currentTask.Id)!.Dependencies?.ForEach(tsk => taskQueue.Enqueue(tsk));
        }
        return true;

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
        if (_bl.IsCreate)
            throw new BlCantChangeAfterScheduledException("you cant add a task after a create a schedule");
        ///Checking that there is a description for the task
        if (item.Alias == null)
            throw new wrongInput("empty alias");
        ///Creating the task's dependencies
        item?.Dependencies?.ForEach(dep =>
        {
            var taskInList = Bl._dal.Task.Read(dep.Id);
            if (taskInList != null)
            {
                dep.Id = taskInList.Id;
                dep.Description = taskInList.Description;
                dep.Alias = taskInList.Alias;
            }
        });
        ///Creating the new task
        DO.Task newTask = new DO.Task
        (0, item!.Description, item.RequiredEffortTime, item.Alias, false, _bl.Clock, item.StartDate, item.ForecastDate, item.StartDate + item.RequiredEffortTime, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        int idTask = Bl._dal.Task.Create(newTask);
        ///Creating appropriate dependencies for the task
        item?.Dependencies?.ForEach(taskDep =>
                        {
                            var newDep = new DO.Dependency { DependentTask = idTask, DependsOnTask = taskDep.Id };
                            Bl._dal.Dependency.Create(newDep);
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
        if (Bl._dal.Dependency.ReadAll(dep => dep.DependsOnTask == id).Any())
            throw new BlDeletionImpossibleException($"Task with ID={id} is in depended");
        ///Tasks cannot be deleted after the project schedule has been created
        if (Bl._dal.Task.ReadAll(task => task.IsMilestone == true).Any())
            throw new BlDeletionImpossibleException($"Cannot delete a task after a Creating a schedule");
        try
        {
            Bl._dal.Task.Delete(id);
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
        DO.Task doTask = Bl._dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"task with ID={id} does Not exist");
        try
        {
            ///Calculates the status
            //before createing project
            var status = BO.Status.Unscheduled;
            //after the creation
            if (_bl.IsCreate)
                //When start date is over
                if (_bl.Clock > doTask.StartDate)
                    //if the task was complitied
                    if (doTask.CompleteDate != null)
                        status = BO.Status.Done;
                    //if the deadline of the task is over
                    else if (_bl.Clock > doTask.DeadlineDate)
                        status = BO.Status.InJeopardy;
                    //while the task on track
                    else if (doTask.EngineerId != null)
                        status = BO.Status.OnTrack;
                    else
                        status = BO.Status.Available;

                //before the start date
                else
                    status = BO.Status.Scheduled;

            var milestoneInTask = Bl._dal.Dependency.ReadAll()
             .Where(dep => dep?.DependentTask == id)
             .Select(dep => new MilestoneInTask
             {
                 Id = dep?.DependsOnTask ?? throw new DalDoesNotExistException(""),
                 Alias = Bl._dal.Task.Read(dep.DependsOnTask.Value)?.Alias
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
                RequiredEffortTime = doTask.RequiredEffortTime,
                Engineer = doTask.EngineerId != null
                ? new EngineerInTask { Id = (int)doTask.EngineerId, Name = Bl._dal.Engineer.Read((int)doTask.EngineerId)?.Name }
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
        var allTasks = Bl._dal.Task.ReadAll().Select(doTask => Read(doTask!.Id) ?? throw new BlDoesNotExistException("the tasks dont exist"));
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
        if (_bl.IsCreate)
        {
            ///Check if there is a circle dependencies
            if (item!.Dependencies?.Count() > 0 && !ImpossibleDependency(item!))
                throw new BlWrongInputException("You cant insert a circle dependencies");
            taskUpdate = new DO.Task
            (item.Id, item.Description, item.RequiredEffortTime, item.Alias, false, item.CreateAtDate, item.StartDate, item.BaselineStartDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
            ///If this task has dependent tasks
            if (item.Dependencies != null)
            {
                var depToDelete = Bl._dal.Dependency.ReadAll(dep => dep?.DependentTask == item.Id);
                ///Deleting all dependencies that the task shares
                depToDelete.ToList().ForEach(dep =>
                    {
                        Bl._dal.Dependency.Delete(dep!.Id);
                    });
                ///Changing the dependencies of the task
                item?.Dependencies?.ForEach(taskDep =>
                {
                    var newDep = new DO.Dependency { DependentTask = item.Id, DependsOnTask = taskDep.Id };
                    Bl._dal.Dependency.Create(newDep);
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
                if (!Bl._dal.Dependency.ReadAll(dep => dep.DependentTask == item.Id).All(dep => Bl._dal.Task.Read((int)dep?.DependsOnTask!)?.DeadlineDate <= item.StartDate))
                    throw new BlWrongDateException($"A start date is less than the end date of the tasks that task {item.Id} depends on");
            }
            ///If the start date is less than the start date of the entire project
            if (!Bl._dal.Dependency.ReadAll().Any(dep => dep?.DependentTask == item.Id) && item.StartDate >= _bl.StartDate)
            {
                throw new BlWrongDateException($" The task: {item.Id}- start date is earlier than the project start date");
            }
            ///If the start date is greater than the end date
            if (item.StartDate > item.DeadlineDate)
                throw new BlWrongDateException($"The task:{item.Id}  start date is later than the end date");
            ///When you want to update an engineer who already has another engineer for this task
            if (Bl._dal.Task.Read(item.Id)?.EngineerId != null && item.Engineer != null && Bl._dal.Task.Read(item.Id)?.EngineerId != item.Engineer?.Id)
                throw new wrongInput($"The task:{item.Id} was taken by another engineer");
            if(item.Engineer!=null)
                 if ((BO.EngineerExperience)Bl._dal.Engineer.Read(item.Engineer.Id)!.Level! >= (BO.EngineerExperience)item.ComplexityLevel!)
                     throw new BlWrongInputException($"The level of the task:{item.Id} doesnt feet the level of the engineer");
            /////End date calculator
            //var deadLineDate = item.StartDate + item.RequiredEffortTime;
            ///duration calculator
            //var requiredEffortTime = item.DeadlineDate - item.StartDate;
            if (item.Dependencies != null)
                foreach (var dep in item.Dependencies)
                {
                    var newDepemdency = new Dependency
                    {
                        DependentTask = item.Id,
                        DependsOnTask = dep.Id,
                    };
                    Bl._dal.Dependency.Create(newDepemdency);
                }
            taskUpdate = new DO.Task
           (item.Id, item.Description, item.RequiredEffortTime, item.Alias, false, item.CreateAtDate, item.StartDate, item.BaselineStartDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        }
        ///Test Data
        try
        {
            Bl._dal.Task.Update(taskUpdate);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item!.Id} already exists", ex);
        }
    }
    public IEnumerable<TaskInList> AllTaskInList()
    {
        // Assuming ReadAll() returns a collection of tasks
        return ReadAll().Select(task => new TaskInList
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            Status = task.StatusTask
        });
    }


    public IEnumerable<TaskInEngineer> AllTaskInEngineer(BO.EngineerExperience? engineerExperience = BO.EngineerExperience.None)
    {
        /////Checking if all the tasks that this task depends on have already been done

        return ReadAll(task =>
            task.Engineer == null &&                      // משימה לא מבוצעת על ידי מהנדס אחר
            task.StatusTask != BO.Status.Done &&                         // משימה לא הסתיימה
            task.ComplexityLevel <= engineerExperience  // רמת המורכבות תואמת את ניסיון המהנדס
            //&& (task.Dependencies != null ? task.Dependencies.All(subtask => subtask.Status == BO.Status.Done) : true) // כל המשימות המשניות סגורות
        )
        .Select(task => new TaskInEngineer
        {
            Id = task.Id,
            Alias = task.Alias
        });
        //return ReadAll().Select(task => new TaskInEngineer
        //{
        //    Id = task.Id,
        //    Alias = task.Alias
        //});
    }
    public IEnumerable<TaskInList> TaskInListByLevel(BO.EngineerExperience? engineerExperience)
    {
        return ReadAll(task => task.ComplexityLevel == engineerExperience).Select(task => new TaskInList
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            Status = task.StatusTask
        });
    }
    /// <summary>
    /// check if all the dependencies of the func done
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    private bool isAllDepDone(BO.Task task)
    {
        if(task.Dependencies!=null) 
            return task.Dependencies.All(dep => dep.Status == BO.Status.Done);
        return true;
    }
    public IEnumerable<TaskInList> AllTaskInListByEngineerLevel(BO.EngineerExperience? engineerExperience = BO.EngineerExperience.None)
    {
        return ReadAll(task =>
            task.Engineer == null &&                      // משימה לא מבוצעת על ידי מהנדס אחר
             task.StatusTask != BO.Status.Done &&
            task.ComplexityLevel <= engineerExperience  // רמת המורכבות תואמת את ניסיון המהנדס
            //&&isAllDepDone(task)// כל המשימות המשניות סגורות
        )
        .Select(task => new TaskInList
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            Status = task.StatusTask
        });

    }
}

