using BO;
using PL.Engineer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PL;
internal class TaskToChoose : IEnumerable
{
    //  public static BO.EngineerExperience? LevelOfEng { get; set; } = BO.EngineerExperience.None;


    ////public BO.EngineerExperience? getEng(EngineerInTask eng)
    ////{
    ////    return App.s_bl.Engineer?.Read(eng.Id)?.Level;

    ////}

    //private IEnumerable<BO.TaskInEngineer> _tasks;

    //public IEnumerable<BO.TaskInEngineer> Tasks
    //{
    //    get { return _tasks; }
    //    set
    //    {
    //        if (_tasks != value)
    //        {
    //            _tasks = value;
    //            OnPropertyChanged("Tasks");
    //        }
    //    }
    //}

    static readonly IEnumerable<BO.TaskInEngineer> s_taskInEng = BlApi.Factory.Get().Task.AllTaskInEngineer(BO.EngineerExperience.Competent);
    public IEnumerator GetEnumerator() => s_taskInEng.GetEnumerator();
}

