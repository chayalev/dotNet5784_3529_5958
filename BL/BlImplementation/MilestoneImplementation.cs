
using BlApi;
using BO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create()
    {


        // Comparer s_comparer = new();
        ////לבדוק את העניין עם התז של המילסטון!
        //int countMil = 0;
        //   var taskDependencies =_dal.Dependency.ReadAll().OrderBy(dep=>dep?.DependsOnTask).GroupBy(dep=>dep?.DependentTask,dep=>dep?.DependsOnTask,(key,deps)=> new { TaskId = key, Dependencies = deps }).ToList();
        //    //לבדוק אם השורה קודם שווה לאלגוריתם הנל
        //    var dependencies = _dal.Dependency.ReadAll();
        //    var groupedTasks = from dep in dependencies
        //           let depTask = dep?.DependentTask
        //           where depTask != null
        //           group dep by depTask into grouped
        //           //מיון לפי משימות קודמות!!!!!
        //           orderby grouped
        //           select new { Task = grouped.Key, DependentTasks = grouped.Select(g => g?.DependsOnTask).ToList() };
        //    //לבדוק שהdistinct תקין!!!!
        //    var stage3 = taskDependencies.Select(dep => dep.Dependencies).Distinct(s_comparer);
        //    //var ksp.dosk zv kt gucs tbjbu f,cbu nacu agu cs c=-100 

        //    IEnumerable<IEnumerable<int>> groupedTasksDistinct= (IEnumerable<IEnumerable<int>>)groupedTasks.Distinct();
        //    //של המורה:
        //    var stage4=
        //    (from deps in stage3
        //     let milestoneId = _dal.Task.Create(new(
        //         Id: 0,
        //         Alias: "",
        //         Description: "",
        //         CreatedAtDate:"",
        //         ))
        //     select new { milestoneId =milestoneId,Dependencies=deps}).ToList();
        //   List<IEnumerable<int>> listOfGroups = groupedTasksDistinct.ToList();
        //   var newMileston = listOfGroups.Select(taskM=> new { id = countMil++, lastTaskMile = listOfGroups[1] });
        return 0;


    }

    public Milestone? Read(int id)
    {
        try
        {
            _dal.Task.Read(id);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);        
        }
        return null;
    }

    public void Update(Milestone item)
    {
       var taskToUpdate= _dal.Task.Read(task => task.Id == item.Id);
      




    }
}

