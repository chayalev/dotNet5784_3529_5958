using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;
/// <summary>
/// interface of Engineer
/// </summary>
public interface IEngineer
{
    /// <summary>
    /// Function that get engineer and add him to the data
    /// </summary>
    /// <param name="eng"></param>
    /// <returns>gets the engineers to add</returns>
    public int Create(BO.Engineer eng);
    /// <summary>
    /// Function to return the engineer by his id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>gets the id of the engineer</returns>
    public BO.Engineer? Read(int id);
    /// <summary>
    /// Function to update an engineer
    /// </summary>
    /// <param name="eng">gets the engineers to update</param>
    public void Update(BO.Engineer eng);
    /// <summary>
    /// Function to delete an engineer
    /// </summary>
    /// <param name="id">get the id of the engineers to deleted</param>
    public void Delete(int id);
    /// <summary>
    /// Function to get collection of engineers by selection
    /// </summary>
    /// <returns>collection of engineers</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    public IEnumerable<EngineerInTask> AllEngineerInTask();
}

