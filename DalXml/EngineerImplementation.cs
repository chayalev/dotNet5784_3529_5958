
using DalApi;
using DO;
using System.ComponentModel;

namespace Dal;

internal class EngineerImplementation : IEngineer
{
    const string s_engineers = @"engineer";//XML Serializer
    public int Create(Engineer item)
    {
        List<Engineer?> listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer?>(s_engineers);
        if (listEngineer.FirstOrDefault(eng => eng?.Id == item.Id != null) == null)
            throw new DalAlreadyExistsException($"ID = {item.Id} already exist");
        listEngineer.Add(item);
        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
        return item.Id;
    }

    public void Delete(int id)
    {

        List<Engineer?> listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer?>(s_engineers);
        if (listEngineer.RemoveAll(eng => eng?.Id == id) == 0)
            throw new DalDoesNotExistException($"Engingineer ID = {id} does not exist");

        XMLTools.SaveListToXMLSerializer(listEngineer, s_engineers);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer?> listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer?>(s_engineers);
        return listEngineer.FirstOrDefault(filter) ??
            throw new DalDoesNotExistException($"Engingineer does not exist");
    }

    public Engineer? Read(int id)
    {
        List<Engineer?> listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer?>(s_engineers);
        return listEngineer.FirstOrDefault(eng => eng?.Id == id) ??
            throw new DalDoesNotExistException($"Engingineer ID = {id} does not exist");
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer?> listEngineer = XMLTools.LoadListFromXMLSerializer<Engineer?>(s_engineers);
        if (filter == null)
            return listEngineer.Select(eng => eng);
        else
            return listEngineer.Where(filter);
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer item)
    {
        Delete(item.Id);
        Create(item);
    }
}
