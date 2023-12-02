
namespace Dal;
using DalApi;
using DO;
// realization of Engineer interface
internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        //Creating a new Engineer
        if (DataSource.Engineers.Any(Engineer => Engineer.Id == item.Id))
            throw new Exception(message: "An object of type Engineer with such an ID already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        //Searches for the Engineer according to the id it received
        Engineer? engineerDelete = DataSource.Engineers.Find(Engineer => Engineer.Id == id);
        if (engineerDelete == null)
            throw new Exception("An object of type Engineer with such an ID is not exists");
        //check if the engineer is depended
        if (DataSource.Tasks.Any(Task => Task.EngineerId == id))
            throw new Exception("Can not delete Engineer in the middle of task");
        DataSource.Engineers.Remove(engineerDelete);
    }


    public Engineer? Read(int id)
    {
        //Returns the requested Engineer, if not found returns null
        return DataSource.Engineers.Find(Engineer => Engineer.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        //Returns the list of engineers
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        //Searches for the engineer according to the id it received
        Engineer? engineerDelete = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);
        if (engineerDelete == null)
            throw new Exception("An object of type Task with such an ID is not exists");
        // if the engineer exists - deletes the engineer,and add the update one
        DataSource.Engineers.Remove(engineerDelete);
        DataSource.Engineers.Add(item);
    }
    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
