﻿
namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Runtime.Intrinsics.Arm;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    // הפונקציה GetDependencies כדי לקבל את התלות עבור משימה מסוימת
    private List<BO.TaskInList> GetDependencies(int taskId)
    {
        //עובר על רשימת התלויות ולוקח את כל מה שהמשימה הזו תלויה 
        List<BO.TaskInList> dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == taskId)
        //על כל אחת מהמשימות הקודמות
        .Select(dep =>
        {
            //יוצר איבר מסוג משימה ברשימה לפי הערכים שנמצאים במשימה
            var currentTask = _dal.Task.Read(task => task.Id == dep?.DependsOnTask);
            int newid = dep?.DependsOnTask ?? throw new DalDoesNotExistException("");
            //מחשב את הסטטוס 
            var status = _dal.Task.ReadAll().FirstOrDefault(task => task?.IsMilestone == true) == null ? 0 : (Status)1;
            //מחזיר איבר חדש מסוג משימה ברשימה
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
    public int Create(BO.Task item)
    {
        //להוסיף חריגה מתאימה
        if (Factory.Get().IsCreate)
            throw new Exception("f");
        //בדיקות תקינות!!!
        if (item.Alias == null)
            throw new wrongInput("כינוי ריק");
        //   var temp = item?.Dependencies?
        //.Select(dep => _dal.Task.Read(dep.Id))
        //.Select(taskInList => new TaskInList
        //{
        //    Id = taskInList!.Id,
        //    Description = taskInList.Description,
        //    Alias = taskInList.Alias,
        //}).ToList();
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
        //item?.Dependencies?.Select(dep => _dal.Dependency.Create(new Dependency { DependentTask = item.Id, DependsOnTask = dep.Id }));
        DO.Task newTask = new DO.Task
        (0, item!.Description, item.RequiredEffortTime, item.Alias, false, item.CreateAtDate, item.StartDate, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        int idTask = _dal.Task.Create(newTask);
        //var listDep = item.Dependencies;
        //if (listDep != null)
        //    foreach (var taskDep in listDep)
        //    {
        //        var newDep = new DO.Dependency { DependentTask = idEng, DependsOnTask = taskDep.Id };
        //        _dal.Dependency.Create(newDep);
        //    }
        item?.Dependencies?.ForEach(taskDep =>
                        {
                            var newDep = new DO.Dependency { DependentTask = idTask, DependsOnTask = taskDep.Id };
                            _dal.Dependency.Create(newDep);
                        });

        return idTask;
    }
    public void Delete(int id)
    {
        //בדיקה שהמשימה לא  קודמת למשימות אחרות
        if (_dal.Dependency.ReadAll(dep => dep.DependsOnTask == id).Any())
            throw new BlDeletionImpossible($"Task with ID={id} is in depended");
        //בדיקה -אי אפשר למחוק משימות לאחר לוז יצירת פרויקט
        if (_dal.Task.ReadAll(task => task.IsMilestone == true).Any())
            throw new BlDeletionImpossible($"Cannot delete a task after a Creating a schedule");
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={id} doesnt exists", ex);
        }
    }

    public BO.Task? Read(int id)
    {

        DO.Task doTask = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"task with ID={id} does Not exist");

        try
        {


            var status = _dal.Task.ReadAll().FirstOrDefault(task => task?.IsMilestone == true) == null ? 0 : (Status)1;

            //לבדוק אם השורות הבאות מביאות לי את אותה תוצאה כמו השורות שאחר כך

            var milestoneInTask = _dal.Dependency.ReadAll()
             .Where(dep => dep?.DependentTask == id)
             .Select(dep => new MilestoneInTask
             {
                 Id = dep?.DependsOnTask ?? throw new DalDoesNotExistException(""),
                 Alias = _dal.Task.Read(dep.DependsOnTask.Value)?.Alias
             })
             .FirstOrDefault();



            //להחליף לשאילתת 
            //var listMilestone = _dal.Dependency.Read(dep => dep.DependentTask == id);
            //var milestoneInTask = new MilestoneInTask { Id = (int)listMilestone!.DependsOnTask!, Alias = _dal.Task.Read((int)listMilestone.DependsOnTask)?.Alias };

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

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        //יצירת רשימת משימות חדשה מסוג BO 
        //var allTasks = _dal.Task.ReadAll().Where(tsk=>tsk?.IsMilestone==false).Select(doTask => Read(doTask!.Id)==null?throw new Exception("as"):);
        var allTasks = _dal.Task.ReadAll().Select(doTask => new BO.Task
        {
            Id = doTask!.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Dependencies = GetDependencies(doTask.Id),
            StatusTask = _dal.Task.ReadAll(task => task.IsMilestone == true) == null ? 0 : (Status)1,
            CreateAtDate = doTask.CreatedAtDate,
            StartDate = doTask.StartDate,
            ForecastDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            ComleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = doTask.EngineerId != null
                    ? new EngineerInTask { Id = (int)doTask.EngineerId, Name = _dal.Engineer.Read(doTask.EngineerId.Value)?.Name }
                    : null,
            ComplexityLevel = (BO.EngineerExperience?)doTask.CopmlexityLevel,
        });
        if (filter == null)
            return allTasks;
        else
            return allTasks.Where(filter);
    }

    public void Update(BO.Task item)
    {
        DO.Task taskUpdate;
        //לפני יצירת לוז
        if (!Factory.Get().IsCreate)
        {
            taskUpdate = new DO.Task
            (item.Id, item.Description, item.RequiredEffortTime, item.Alias, false, null, null, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
            if (item.Dependencies != null)
            {
                var depToDelete = _dal.Dependency.ReadAll(dep => dep?.DependentTask == item.Id);
                depToDelete.ToList().ForEach(dep =>
                    {
                        _dal.Dependency.Delete(dep!.Id);
                    });
                item?.Dependencies?.ForEach(taskDep =>
                {
                    var newDep = new DO.Dependency { DependentTask = item.Id, DependsOnTask = taskDep.Id };
                    _dal.Dependency.Create(newDep);
                });
            }
        }
        //אחרי יצירת פרויקט
        else
        {
            //כשרוצה לשנות תאריך התחלה של משימה
            if (item.StartDate != null)
            {
                //אם התאריך התחלה קטן מתאריך הסיום של המשימות שתלוי בהן
                if (!_dal.Dependency.ReadAll(dep => dep.DependentTask == item.Id).All(dep => _dal.Task.Read((int)dep?.DependsOnTask)?.DeadlineDate <= item.StartDate))
                    //-אין אפשרות לשנות תאריך התחלה בכזה מצב.לעשות חריגה נורמלית
                    throw new Exception("dd");
            }
           //אם תאריך הההתחלה קטן מתאריך ההתחלה של כל הפרויקט
            if (!_dal.Dependency.ReadAll().Any(dep => dep?.DependentTask == item.Id) && item.StartDate >= Factory.Get().startDate)
            {
                throw new Exception("dd");
            }
            //אם תאריך ההתחלה גדול מתאריך הסיום
            if (item.StartDate > item.DeadlineDate)
                throw new Exception("yg");
            //כשרוצה לעדכן מהנדס שכבר קיים מהנדס אחר למשימה זו
            if (_dal.Task.Read(item.Id)?.EngineerId != null && _dal.Task.Read(item.Id)?.EngineerId != item.Engineer?.Id)
                throw new Exception("j");
            //מחשב תאריך סיום
            var deadLineDate = item.StartDate + item.RequiredEffortTime;
            //מחשב משך זמן
            var requiredEffortTime = item.DeadlineDate - item.StartDate;
            taskUpdate = new DO.Task
           (item.Id, item.Description, requiredEffortTime, item.Alias, false, null, null, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        }
        //בדיקת נתונים
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

