
namespace Dal;
using DalApi;
using DO;
using System.Linq;

// realization of Engineer interface
internal class EngineerImplementation : IEngineer
{
   //בדיקות תקינות!!!!!\/
   ///הערות!!!
    public int Create(Engineer item)
    {
        //Creating a new Engineer
        if (DataSource.Engineers.Exists(Engineer => Engineer.Id == item.Id))
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        //Searches for the Engineer according to the id it received
        Engineer? engineerDelete = DataSource.Engineers.FirstOrDefault(Engineer => Engineer.Id == id);
        if (engineerDelete == null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exists");
        //check if the engineer is depended
        if (DataSource.Tasks.Exists(Task => Task.EngineerId == id))
            throw new DalDeletionImpossible($"Engineer in ID={id} in the middle of task");
        DataSource.Engineers.Remove(engineerDelete);
    }

    public Engineer? Read(Func<Engineer, bool> filter)// stage 2
    {
        return DataSource.Engineers.FirstOrDefault(filter);
    }
    public Engineer? Read(int id)
    {
        //Returns the requested Engineer, if not found returns null
        return DataSource.Engineers.FirstOrDefault(Engineer => Engineer.Id == id);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        //Searches for the engineer according to the id it received
        Engineer? engineerDelete = DataSource.Engineers.FirstOrDefault(Engineer => Engineer.Id == item.Id);
        if (engineerDelete == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        // if the engineer exists - deletes the engineer,and add the update one
        DataSource.Engineers.Remove(engineerDelete);
        DataSource.Engineers.Add(item);
    }
    public void Reset()
    {
        DataSource.Engineers.Clear();
    }
}
