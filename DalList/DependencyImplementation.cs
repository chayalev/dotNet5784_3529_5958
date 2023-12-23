

namespace Dal;
using DalApi;
using DO;
using System.Linq;

// realization of Dependency interface
internal class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newID = DataSource.Config.NextDependencyId;
        //Creating a new dependency with the following ID in the system
        Dependency newDependency = item with { Id=newID};
        DataSource.Dependencies.Add(newDependency);
        return newID;
    }

    public void Delete(int id)
    {
        //Searches for the dependency according to the id it received, if it exists - deletes the dependency
        Dependency? dependencyDelete = DataSource.Dependencies.FirstOrDefault(Dependency => Dependency.Id == id);
         if (dependencyDelete==null)
            throw new DalDoesNotExistException($"Dependency with ID= {id} does not exist");
        DataSource.Dependencies.Remove(dependencyDelete);
    }
   public Dependency? Read(Func<Dependency, bool> filter)// stage 2
    {
            return DataSource.Dependencies.FirstOrDefault(filter);
    }
    public Dependency? Read(int id)
    {
        //Returns the requested dependency, if not found returns null
        return DataSource.Dependencies.FirstOrDefault(Dependency => Dependency.Id == id);
    }

     public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null) //stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);

    }

    public void Update(Dependency item)
    {
        //Searches for the dependency according to the id it received
        Dependency? dependencyDelete = DataSource.Dependencies.FirstOrDefault(Dependency => Dependency.Id == item.Id);
        if (dependencyDelete == null)
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        // if the dependency exists - deletes the dependency,and add the update one
        DataSource.Dependencies.Remove(dependencyDelete);
        DataSource.Dependencies.Add(item);
    }

    public void Reset()
    {
        DataSource.Dependencies.Clear();
    } 
}
