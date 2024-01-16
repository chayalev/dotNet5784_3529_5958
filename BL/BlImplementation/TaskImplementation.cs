
namespace BlImplementation;
using BlApi;
using BLApi;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task item)
    {
        if (item.Id < 0)
            throw new Exception("תעודת זהות לא תקינה");
        if(item.Alias == null)
            throw new Exception("כינוי ריק");
        //הוספת משימות קודמות מתוך רשימת המשימות הקיימת
        DO.Task newTask = new DO.Task
        (item.Id, item.Description,null, item.Alias,false, item.CreateAtDate, item.StartDate,item.ForecastDate,item.DeadlineDate,item.ComleteDate,item.Deliverables,item.Remarks,item.Engineer?.Id,(DO.EngineerExperience?)item.ComplexityLevel);
        try
        {
            int idEng = _dal.Task.Create(newTask);
            return idEng;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {
        //בדיקה שהמשימה לא  קודמת למשימות אחרות
        //בדיקה -אי אפשר למחוק משימות לאחר לוז יצירת פרויקט
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
    }

    public BO.Task? Read(int id)
    {

        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = id,
            Alias =doTask.Alias,
            Description = doTask.Description,
            CreateAtDate = doTask.CreatedAtDate,
            StartDate = doTask.StartDate,
            ForecastDate=doTask.ScheduledDate,
            DeadlineDate=doTask.DeadlineDate,
            ComleteDate=doTask.CompleteDate,
            Deliverables=doTask.Deliverables,
            Remarks=doTask.Remarks,
            ComplexityLevel=(BO.EngineerExperience?)doTask.CopmlexityLevel,
        };

    }

    public IEnumerable<BO.Task> ReadAll()
    {
     
    }

    public void Update(BO.Task item)
    {
        //בדיקת נתונים
        try
        {
            DO.Task taskUpdate = new DO.Task
           (item.Id, item.Description, null, item.Alias, false, item.CreateAtDate, item.StartDate, item.ForecastDate, item.DeadlineDate, item.ComleteDate, item.Deliverables, item.Remarks, item.Engineer?.Id, (DO.EngineerExperience?)item.ComplexityLevel);
            _dal.Task.Update(taskUpdate);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={item.Id} already exists", ex);
        }
    }
}
