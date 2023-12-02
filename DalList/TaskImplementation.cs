
namespace Dal;
using DalApi;
using DO;
// realization of Task interface
internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        //Creating a new dependency with the following ID in the system and the CreatedAtDate is the day it added
        int newID = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newID ,CreatedAtDate = (DateTime.Today).AddYears(-1)};
        DataSource.Tasks.Add(newTask);
        return newID;
    }

    public void Delete(int id)
    {
        //Searches for the Task according to the id it received, if it exists - deletes the task
        Task? taskDelete = DataSource.Tasks.Find(Task => Task.Id == id);
        if (taskDelete==null)
            throw new Exception("An object of type Task with such an ID is not exists");
        //check if the task is depended
        if(DataSource.Dependencies.Any(Dep => Dep.DependentTask == id || Dep.DependsOnTask==id ))
            throw new Exception("Can not delete depended task");
        DataSource.Tasks.Remove(taskDelete);
    }

    public Task? Read(int id)
    {
        //Returns the requested task, if not found returns null
        return DataSource.Tasks.Find(Task => Task.Id == id);
      
    }

    public List<Task> ReadAll()
    {
        //Returns the list of tasks
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        //Searches for the task according to the id it received
        Task? taskDelete = DataSource.Tasks.Find(Task => Task.Id == item.Id);
        if (taskDelete == null)
            throw new Exception("An object of type Task with such an ID is not exists");
        // if the dependency exists - deletes the dependency,and add the update one
        DataSource.Tasks.Remove(taskDelete);
        DataSource.Tasks.Add(item);
    }
    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}
