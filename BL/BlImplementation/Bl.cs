namespace BlImplementation;
using BlApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public bool IsCreate { get; set; } = false;

    public void Reset()
    {
        DalApi.Factory.Get.Reset();
    }
    //לבדוק אם אפשר להחליף את הFOREACH!!!!!
    private DateTime? EarliestStartDate(DO.Task task)
    {
        var dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id);
        DateTime? earliestDate = null;
        if (dependencies.Any())
        {

            earliestDate = _dal.Task.Read((int)dependencies.First()!.DependsOnTask!)?.DeadlineDate;
            foreach (var item in dependencies)
            {
                //לסדר חריגה
                DateTime endDate = _dal.Task.Read((int)item!.DependsOnTask!)?.DeadlineDate ?? throw new BO.BlWrongInput($"You dont have a deadline date to task with id:{item.Id}");
                earliestDate = endDate > earliestDate ? endDate : earliestDate;
            }
        }
        return earliestDate;
    }
    private DateTime GetStartDate()
    {
        Console.WriteLine("insert the start date:");
        var startDate = DateTime.TryParse(Console.ReadLine(), out DateTime result) ? result : throw new BO.BlWrongInput("must insert value!");
        return startDate;
    }
    private void updateTask(DO.Task task, DateTime? startDate)
    {
        _dal.Task.Update(task! with { StartDate = startDate, DeadlineDate = startDate + task.RequiredEffortTime });
    }
    private Queue<DO.Task> UpdateNextTask(IEnumerable<DO.Task> prevTasks)
    {
        Queue<DO.Task> taskQueue = new Queue<DO.Task>(prevTasks);

        while (taskQueue.Count > 0)
        {
            var currentTask = taskQueue.Dequeue();
            if (currentTask.StartDate == null)
            {
                DateTime? earliestDate = EarliestStartDate(currentTask);
                updateTask(currentTask, earliestDate);
            }
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

    public void CreateProject()
    {
        var startDate = GetStartDate();
        var tasks = _dal.Task.ReadAll();
        //מעדכן את המשימות הראשנות 
        var firstTasks = tasks.Where(task => !_dal.Dependency.ReadAll(dep => dep.DependentTask == task?.Id).Any()).ToList();
        firstTasks.ForEach(task => updateTask(task!, startDate));
        UpdateNextTask(firstTasks!).ToList();
        

        StartDate = startDate;
        _dal.startDate = startDate;
    }
}


