
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
        //אין אפשרות להוסיף משימה אחרי יצירת פרויקט
        if (Factory.Get().IsCreate)
            throw new BlCantChangeAfterScheduled("you cant add a task after a create a schedule");
        //בדיקות תקינות!!!
        if (item.Alias == null)
            throw new wrongInput("כינוי ריק");
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
        DO.Task newTask = new DO.Task
        (0, item!.Description, item.RequiredEffortTime, item.Alias, false, item.CreateAtDate, item.StartDate, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
        int idTask = _dal.Task.Create(newTask);
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

            //מחשב את הסטטוס
            var status = _dal.startDate == null ? 0 : (Status)1;

            //לבדוק אם השורות הבאות מביאות לי את אותה תוצאה כמו השורות שאחר כך

            var milestoneInTask = _dal.Dependency.ReadAll()
             .Where(dep => dep?.DependentTask == id)
             .Select(dep => new MilestoneInTask
             {
                 Id = dep?.DependsOnTask ?? throw new DalDoesNotExistException(""),
                 Alias = _dal.Task.Read(dep.DependsOnTask.Value)?.Alias
             })
             .FirstOrDefault();
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
        var allTasks = _dal.Task.ReadAll().Select(doTask => Read(doTask!.Id) ?? throw new BlDoesNotExistException("the tasks dont exist"));
        //var allTasks = _dal.Task.ReadAll().Select(doTask => new BO.Task
        //{
        //    Id = doTask!.Id,
        //    Alias = doTask.Alias,
        //    Description = doTask.Description,
        //    Dependencies = GetDependencies(doTask.Id),
        //    StatusTask = _dal.Task.ReadAll(task => task.IsMilestone == true) == null ? 0 : (Status)1,
        //    CreateAtDate = doTask.CreatedAtDate,
        //    StartDate = doTask.StartDate,
        //    ForecastDate = doTask.ScheduledDate,
        //    DeadlineDate = doTask.DeadlineDate,
        //    ComleteDate = doTask.CompleteDate,
        //    Deliverables = doTask.Deliverables,
        //    Remarks = doTask.Remarks,
        //    Engineer = doTask.EngineerId != null
        //            ? new EngineerInTask { Id = (int)doTask.EngineerId, Name = _dal.Engineer.Read(doTask.EngineerId.Value)?.Name }
        //            : null,
        //    ComplexityLevel = (BO.EngineerExperience?)doTask.CopmlexityLevel,
        //});
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
                if (!_dal.Dependency.ReadAll(dep => dep.DependentTask == item.Id).All(dep => _dal.Task.Read((int)dep?.DependsOnTask!)?.DeadlineDate <= item.StartDate))
                    //-אין אפשרות לשנות תאריך התחלה בכזה מצב.לעשות חריגה נורמלית
                    throw new BlWrongDate($"A start date is less than the end date of the tasks that task {item.Id} depends on");
            }
           //אם תאריך הההתחלה קטן מתאריך ההתחלה של כל הפרויקט
            if (!_dal.Dependency.ReadAll().Any(dep => dep?.DependentTask == item.Id) && item.StartDate >= Factory.Get().StartDate)
            {
                throw new BlWrongDate($" The task: {item.Id}- start date is earlier than the project start date");
            }
            //אם תאריך ההתחלה גדול מתאריך הסיום
            if (item.StartDate > item.DeadlineDate)
                throw new BlWrongDate($"The task:{item.Id}  start date is later than the end date");
            //כשרוצה לעדכן מהנדס שכבר קיים מהנדס אחר למשימה זו
            if (_dal.Task.Read(item.Id)?.EngineerId != null && _dal.Task.Read(item.Id)?.EngineerId != item.Engineer?.Id)
                throw new wrongInput($"The task:{item.Id} was taken by another engineer");
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

