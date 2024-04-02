namespace BlImplementation;
using BlApi;
using System;
using System.Collections.Generic;

internal class Bl : IBl
{
    /// <summary>
    /// A data link variable
    /// </summary>
    public static DalApi.IDal _dal = DalApi.Factory.Get;

    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation(this);

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Checking whether the project status is after or before creating the schedule
    /// </summary>
    public bool IsCreate { get; set; } = false;

    /// <summary>
    /// Finding the earliest time to start the task
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlWrongInput"></exception>
    private DateTime? EarliestStartDate(DO.Task task)
    {
        ///Finding all the dependencies the task dependent
        var dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id);
        if (dependencies.Any())
        {
            ///Set start time to the latest time a task that the current task depends on ends
            //earliestDate = _dal.Task.Read((int)dependencies.First()!.DependsOnTask!)?.DeadlineDate;
            //foreach (var item in dependencies)
            //{
            //    DateTime endDate = _dal.Task.Read((int)item!.DependsOnTask!)?.DeadlineDate ?? throw new BO.BlWrongDateException($"You dont have a deadline date to task with id:{item.Id}");
            //    earliestDate = endDate > earliestDate ? endDate : earliestDate;
            //}
            return dependencies.Max(dep => _dal.Task.Read((int)dep!.DependsOnTask!)?.DeadlineDate)!.Value.AddHours(1);

        }
        else
            return StartDate;
    }

    /// <summary>
    ///// Receiving the initial bid from the manager
    ///// </summary>
    ///// <returns></returns>
    ///// <exception cref="BO.BlWrongInput"></exception>
    //private DateTime GetStartDate()
    //{
    //    Console.WriteLine("insert the start date:");
    //    ///Saving in a temporary variable the start time entered by the manager
    //    var startDate = DateTime.TryParse(Console.ReadLine(), out DateTime result) ? result : throw new BO.BlWrongDateException("must insert value!");
    //    return startDate;
    //}

    /// <summary>
    /// Sending the task to update the times in dal
    /// </summary>
    /// <param name="task"></param>
    /// <param name="startDate"></param>
    private void updateTask(DO.Task task, DateTime? startDate)
    {
        _dal.Task.Update(task! with { StartDate = startDate, DeadlineDate = startDate + task.RequiredEffortTime });
    }

    /// <summary>
    /// Going over all the tasks and sending to update the start and deadline times
    /// </summary>
    /// <param name="prevTasks"></param>
    /// <returns></returns>
    private Queue<DO.Task> UpdateNextTask(IEnumerable<DO.Task> prevTasks)
    {
        ///Creating a queue and placing all the first tasks in it
        Queue<DO.Task> taskQueue = new Queue<DO.Task>(prevTasks);

        while (taskQueue.Count > 0)
        {
            var currentTask = taskQueue.Dequeue();
           
                ///Sending to a function that finds the earliest time to start the task
                DateTime? earliestDate = EarliestStartDate(currentTask);
                updateTask(currentTask, earliestDate);
            
            ///Finding all the following tasks and putting them into the queue
            var dependedInPrevTask = _dal.Dependency
                .ReadAll(dep => dep.DependsOnTask == currentTask?.Id)
                .Select(dep =>
                {
                    var dependentTask = _dal.Task.Read((int)dep!.DependentTask!);
                    if (dependentTask != null)
                        taskQueue.Enqueue(dependentTask); // הוסף לתור
                    return dependentTask;
                })
                .ToList();
        }

        return taskQueue;
    }

    /// <summary>
    /// Creating a schedule for the project
    /// </summary>
    public void CreateProject(DateTime startDate)
    {
        // var startDate = _dal.Task.ReadAll().Min(x => x!.StartDate);
        StartDate = startDate;
        var tasks = _dal.Task.ReadAll();
        ///Updates the first tasks
        var firstTasks = tasks.Where(task => !_dal.Dependency.ReadAll(dep => dep.DependentTask == task?.Id).Any()).ToList();
       // firstTasks.ForEach(task => updateTask(task!, startDate));
        UpdateNextTask(firstTasks!).ToList();
        //update the start and end days of the project
        var allTask = _dal.Task.ReadAll();
        EndDate = allTask.Max(tsk => tsk?.DeadlineDate);
        _dal.EndDate = EndDate;
        _dal.StartDate = startDate;
        IsCreate = true;
    }

    public void InitializeDB() => DalTest.Initialization.Do();
    /// <summary>
    /// Deleting the database
    /// </summary>
    public void ResetDB() => DalTest.Initialization.Reset();

    #region Clock
    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }
    public void ResetClock()
    {
        Clock = DateTime.Now.Date;
    }

    public void AddYear()
    {
        Clock = Clock.AddYears(1);
    }
    public void AddMonth()
    {
        Clock = Clock.AddMonths(1);
    }

    public void AddDay()
    {
        Clock = Clock.AddDays(1);
    }

    public void AddHour()
    {
        Clock = Clock.AddHours(1);
    }

    #endregion

    /// <summary>
    /// Allows the user to choose a start date for a task, based on its dependencies.
    /// </summary>
    public void StartDateForTask(BO.Task tsk, DateTime startForTsk)
    {
        DateTime? canStartDate = EarliestStartDate(_dal.Task.Read(tsk.Id)!);
        if (startForTsk < canStartDate)
            throw new BO.BlWrongDateException("Invalid start date for the task");
        tsk.StartDate = startForTsk;
        Task.Update(tsk);
    }
}


