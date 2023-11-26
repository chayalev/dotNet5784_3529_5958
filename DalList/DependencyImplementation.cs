

namespace Dal;
using DalApi;
using DO;
// realization of Dependency interface
public class DependencyImplementation : IDependency
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
        Dependency? dependencyDelete = DataSource.Dependencies.Find(Dependency => Dependency.Id == id);
         if (dependencyDelete==null)
            throw new Exception("An object of type Dependency with such an ID is not exists");
        DataSource.Dependencies.Remove(dependencyDelete);
    }

    public Dependency? Read(int id)
    {
        //Returns the requested dependency, if not found returns null
        return DataSource.Dependencies.Find(Dependency => Dependency.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        //Returns the list of dependencies
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        //Searches for the dependency according to the id it received
        Dependency? dependencyDelete = DataSource.Dependencies.Find(Dependency => Dependency.Id == item.Id);
        if (dependencyDelete == null)
            throw new Exception("An object of type Dependency with such an ID is not exists");
        // if the dependency exists - deletes the dependency,and add the update one
        DataSource.Dependencies.Remove(dependencyDelete);
        DataSource.Dependencies.Add(item);
    }

    public void Reset()
    {
        DataSource.Dependencies.Clear();
    } 
}
