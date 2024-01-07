
using DalApi;
using DO;
using System.Linq;
using System.Xml.Linq;

namespace Dal;

internal class TaskImplementation : ITask
{
    const string s_task = @"task";//XML Serializer

    /// <summary>
    /// Creating an task entity
    /// </summary>
    /// <param name="item">Details of the new task</param>
    /// <returns>the id of the task</returns>
    /// <exception cref="DalAlreadyExistsException">Returning an error from an existing task</exception>
    public int Create(DO.Task item)
    {
        int newID=item.Id;
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        if(item.Id == 0)
            newID = Config.NextTaskId;
        DO.Task newTask = item with { Id = newID, CreatedAtDate = (DateTime.Today).AddYears(-1) };
        listTask.Add(newTask);
        XMLTools.SaveListToXMLSerializer(listTask, s_task);
        return newID;
    }

    /// <summary>
    /// Deleting an task from the database
    /// </summary>
    /// <param name="id">ID number of the deleted task</param>
    /// <exception cref="DalDoesNotExistException">Error: task does not exist</exception>
    public void Delete(int id)
    {
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        if (listTask.RemoveAll(task => task?.Id == id) == 0)
            throw new DalDoesNotExistException($"Task ID = {id} does not exist");

        XMLTools.SaveListToXMLSerializer(listTask, s_task);
    }

    /// <summary>
    /// Finding an task by condition
    /// </summary>
    /// <param name="filter">The condition on which the task is found</param>
    /// <returns>the right task</returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        return listTask.FirstOrDefault(filter);
    }

    /// <summary>
    /// Finding an engineer by ID number
    /// </summary>
    /// <param name="id">The id on which the task is found</param>
    /// <returns>the right task</returns>
    public DO.Task? Read(int id)
    {
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        return listTask.FirstOrDefault(task => task?.Id == id);
    }

    /// <summary>
    /// Acceptance of all tasks who meet a certain condition
    /// </summary>
    /// <param name="filter">The condition on which the tasks are found</param>
    /// <returns>the appropriate tasks</returns>
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        if (filter == null)
            return listTask.Select(task => task);
        else
            return listTask.Where(filter);
    }

    /// <summary>
    /// A function to initialize the entity
    /// </summary>
    public void Reset()
    {
        List<DO.Task>? listTask = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_task);
        listTask.Clear();
        XMLTools.SaveListToXMLSerializer(listTask, s_task);
    }

    /// <summary>
    /// entity update
    /// </summary>
    /// <param name="item">Task on change</param>
    public void Update(DO.Task item)
    {
        Delete(item.Id);
        Create(item);
    }
}
