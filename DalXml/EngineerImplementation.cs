
using DalApi;
using DO;
using System.ComponentModel;
using System.Xml.Linq;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    const string s_engineers = @"engineer";//XML Serializer

    /// <summary>
    /// Creating an engineer entity
    /// </summary>
    /// <param name="item">Details of the new engineer</param>
    /// <returns>the id of the engineer</returns>
    /// <exception cref="DalAlreadyExistsException">Returning an error from an existing engineer</exception>
    public int Create(Engineer item)
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        if (listEngineer.FirstOrDefault(eng => eng?.Id == item.Id ) != null)
            throw new DalAlreadyExistsException($"ID = {item.Id} already exist");
        listEngineer.Add(item);
        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
        return item.Id;
    }

    /// <summary>
    /// Deleting an engineer from the database
    /// </summary>
    /// <param name="id">ID number of the deleted engineer</param>
    /// <exception cref="DalDoesNotExistException">Error: Engineer does not exist</exception>
    public void Delete(int id)
    {

        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        if (listEngineer.RemoveAll(eng => eng?.Id == id) == 0)
            throw new DalDoesNotExistException($"Engingineer ID = {id} does not exist");

        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
    }

    /// <summary>
    /// Finding an engineer by condition
    /// </summary>
    /// <param name="filter">The condition on which the engineer is found</param>
    /// <returns>the right engineer</returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        return listEngineer.FirstOrDefault(filter);
    }

    /// <summary>
    /// Finding an engineer by ID number
    /// </summary>
    /// <param name="id">The id on which the engineer is found</param>
    /// <returns>the right engineer</returns>
    public Engineer? Read(int id)
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        return listEngineer.FirstOrDefault(eng => eng?.Id == id);
    }

    /// <summary>
    /// Acceptance of all engineers who meet a certain condition
    /// </summary>
    /// <param name="filter">The condition on which the engineers are found</param>
    /// <returns>the appropriate engineers</returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        if (filter == null)
            return listEngineer.Select(eng => eng);
        else
            return listEngineer.Where(filter);
    }

    /// <summary>
    /// A function to initialize the entity
    /// </summary>
    public void Reset()
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        listEngineer.Clear();
        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
    }

    /// <summary>
    /// entity update
    /// </summary>
    /// <param name="item">Engineerה on change</param>
    public void Update(Engineer item)
    {
        List<Engineer>? listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers);
        Engineer? engineerDelete = listEngineer.FirstOrDefault(Enginner => Enginner.Id == item.Id);
        if (engineerDelete == null)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exists");
        listEngineer.Remove(engineerDelete);
        listEngineer.Add(item);
        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
    }
}
