using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task item);
    public void Delete(int id);
    public IEnumerable<TaskInList> AllTaskInList();
    public IEnumerable<TaskInList> AllTaskInListByEngineerLevel(BO.EngineerExperience? engineerExperience = BO.EngineerExperience.None);
    public IEnumerable<TaskInList> TaskInListByLevel(BO.EngineerExperience? engineerExperience=BO.EngineerExperience.None);
    public IEnumerable<TaskInList> AllTaskInListByEngineerLevel(BO.EngineerExperience? engineerExperience = BO.EngineerExperience.None);
    public IEnumerable<TaskInEngineer> AllTaskInEngineer(BO.EngineerExperience? engineerExperience);


}
