
namespace Dal;
using DalApi;
using DO;
// realization of Engineer interface
public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        if (DataSource.Engineers.Any(Engineer => Engineer.Id == item.Id))
            throw new Exception(message: "An object of type Engineer with such an ID already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (!DataSource.Engineers.Any(Engineer => Engineer.Id == id))
            throw new Exception("An object of type Engineer with such an ID already exists");
        if (DataSource.Tasks.Any(Task => Task.EngineerId == id))
            throw new Exception("Can not delete Engineer in the middle of task");
        DataSource.Engineers.RemoveAll(Engineer => Engineer.Id == id);
    }


    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(Engineer => Engineer.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        if (!DataSource.Engineers.Any(Engineer => Engineer.Id == item.Id))
            throw new Exception("An object of type Engineer with such an ID already exists");
        DataSource.Engineers.RemoveAll(Engineer => Engineer.Id == item.Id);
        DataSource.Engineers.Add(item);
    }
    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
