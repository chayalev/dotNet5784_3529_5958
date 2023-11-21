

namespace Dal;
using DalApi;
using DO;
// realization of Dependency interface
public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int newID = DataSource.Config.NextDependencyId;
        Dependency newDependency = item with { Id=newID};
        return newID;
    }

    public void Delete(int id)
    {
        if (!DataSource.Dependencies.Any(Dependency => Dependency.Id == id))
            throw new Exception("An object of type Dependency with such an ID already exists");
        DataSource.Dependencies.RemoveAll(Dependency => Dependency.Id == id);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(Dependency => Dependency.Id == id);

    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (!DataSource.Dependencies.Any(Dependency => Dependency.Id == item.Id))
            throw new Exception("An object of type Dependency with such an ID already exists");
        DataSource.Dependencies.RemoveAll(Dependency => Dependency.Id == item.Id);
        DataSource.Dependencies.Add(item);
    }
}
