
namespace Dal;
using DalApi;
using DO;
// realization of Task interface
public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        if(DataSource.Tasks.Any(Task => Task.Id == item.Id))
            throw new Exception("An object of type Task with such an ID already exists");
        int newID = DataSource.Config.NextTaskId;
        Task newTask = item with { Id = newID };
        DataSource.Tasks.Add(newTask);
        return newID;
    }

    public void Delete(int id)
    {
        if (!DataSource.Tasks.Any(Task => Task.Id == id))
            throw new Exception("An object of type Task with such an ID not exists");
        if(DataSource.Dependencies.Any(Dep => Dep.DependentTask == id || Dep.DependsOnTask==id ))
            throw new Exception("Can not delete depended task");
        DataSource.Tasks.RemoveAll(Task => Task.Id == id);
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(Task => Task.Id == id);
      
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        if (!DataSource.Tasks.Any(Task => Task.Id == item.Id))
            throw new Exception("An object of type Task with such an ID not exists");
        DataSource.Tasks.RemoveAll(Task => Task.Id == item.Id);
        DataSource.Tasks.Add(item);
    }
    public void Reset()
    {
        DataSource.Tasks.Clear();
    }
}
