
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

namespace Dal;

internal class DependencyImplementation : IDependency
{
    const string s_dependency = @"dependency";//XML Serializer

    /// <summary>
    /// create dependency entity for XML
    /// </summary>
    /// <param name="d">XElement object</param>
    /// <returns>The dependency as xml</returns>
    /// <exception cref="DalXMLFormatException"></exception>
    static DO.Dependency? createDependencyfromXElement(XElement d)
    {
        return new Dependency()
        {
            Id = d.ToIntNullable("Id") ?? throw new DalXMLFormatException("id"),
            DependentTask = (int?)d.Element("DependentTask"),
            DependsOnTask = (int?)d.Element("DependsOnTask")
        };
    }

    /// <summary>
    /// create a dependency 
    /// </summary>
    /// <param name="item">the dependency to create or update</param>
    /// <returns>the number of the dependency</returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Dependency item)
    {
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        int id = Config.NextDependencyId;
        XElement dependencyElem = new XElement("Dependency",
            new XElement("Id", id),
            new XElement("DependentTask", item.DependentTask),
            new XElement("DependsOnTask", item.DependsOnTask)
            );
        dependencyRootElem.Add(dependencyElem);
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
        return id;
    }

    /// <summary>
    /// Delete a dependency
    /// </summary>
    /// <param name="id">the id of the dependency</param>
    /// <exception cref="DalDoesNotExistException">if dependency with such id doesnt exist</exception>
    public void Delete(int id)
    {
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);
        XElement? dep = (from depen in dependencyRootElem.Elements()
                         where (int?)depen.Element("Id") == id
                         select depen).FirstOrDefault() ?? throw new DalDoesNotExistException($"Dependency with ID= {id} does not exist");
        dep.Remove();
        XMLTools.SaveListToXMLElement(dependencyRootElem , s_dependency);
    }

    /// <summary>
    /// get one dependency by spesific term
    /// </summary>
    /// <param name="filter">the terms of the dependency</param>
    /// <returns>the selected dependency</returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);
        return (from dep in dependencyRootElem.Elements()
                let d = createDependencyfromXElement(dep)
                where d != null && filter(d)
                select (Dependency?)d).FirstOrDefault()
                ?? throw new DalDoesNotExistException($"Dependency does not exist");
    }

    /// <summary>
    /// get one dependency by spesific term
    /// </summary>
    /// <param name="id">the id of the chosen dependency</param>
    /// <returns>the chosen dependency</returns>
    /// <exception cref="DalDoesNotExistException">check that the dependency exist</exception>
    public Dependency? Read(int id)
    {
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);
        return (from dep in dependencyRootElem.Elements()
                where dep.ToIntNullable("Id") == id
                select (createDependencyfromXElement(dep))).FirstOrDefault()
                ?? throw new DalDoesNotExistException($"Dependency with ID= {id} does not exist");

    }

    /// <summary>
    /// get all the dependencies that make the condition
    /// </summary>
    /// <param name="filter">The condition that the function must satisfy</param>
    /// <returns>the appropriate functions</returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);

        if (filter != null)
        {
            return from d in dependencyRootElem.Elements()
                   let doDep = createDependencyfromXElement(d)
                   where filter(doDep)
                   select (Dependency?)doDep;
        }
        else
            return from d in dependencyRootElem.Elements()
                   select createDependencyfromXElement(d);
    }

    /// <summary>
    /// A function to initialize the entity
    /// </summary>
    public void Reset()
    {
        XElement dependencyRootElem = XMLTools.LoadListFromXMLElement(s_dependency);
        dependencyRootElem.RemoveAll();
        XMLTools.SaveListToXMLElement(dependencyRootElem, s_dependency);
    }

    /// <summary>
    /// entity update
    /// </summary>
    /// <param name="item">Dependency on change</param>
    public void Update(Dependency item)
    {
        Delete(item.Id);
        Create(item);
    }
}
