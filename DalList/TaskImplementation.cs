
namespace Dal;
using DalApi;
using DO;
using System.Linq;

// realization of Task interface
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        //Creating a new dependency with the following ID in the system and the CreatedAtDate is the day it added
        int newID = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newID, CreatedAtDate = (DateTime.Today).AddYears(-1) };
        DataSource.Tasks.Add(newTask);
        return newID;
    }

    public void Delete(int id)
    {
        //Searches for the Task according to the id it received, if it exists - deletes the task
        Task? taskDelete = DataSource.Tasks.FirstOrDefault(Task => Task.Id == id);
        if (taskDelete == null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exists");
        //check if the task is depended
        if (DataSource.Dependencies.Exists(Dep => Dep.DependentTask == id || Dep.DependsOnTask == id))
            throw new DalDeletionImpossible("Can not delete depended task");
        DataSource.Tasks.Remove(taskDelete);
    }
   public Task? Read(Func<Task, bool> filter)// stage 2
    {
        return DataSource.Tasks.FirstOrDefault(filter);
    }
    public Task? Read(int id)
    {
        //Returns the requested task, if not found returns null
        return DataSource.Tasks.FirstOrDefault(Task => Task.Id == id);

    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }



    public void Update(Task item)
    {
        //Searches for the task according to the id it received
        Task? taskDelete = DataSource.Tasks.FirstOrDefault(Task => Task.Id == item.Id);
        if (taskDelete == null)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exists");
        // if the dependency exists - deletes the dependency,and add the update one
        DataSource.Tasks.Remove(taskDelete);
        DataSource.Tasks.Add(item);
    }
    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}
