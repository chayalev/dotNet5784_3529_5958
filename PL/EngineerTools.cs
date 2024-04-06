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
    static readonly IEnumerable<BO.TaskInEngineer> s_taskInEng = BlApi.Factory.Get().Task.AllTaskInEngineer(BO.EngineerExperience.Competent);
    public IEnumerator GetEnumerator() => s_taskInEng.GetEnumerator();
}


internal class EngineerToChoose : IEnumerable
{
    static readonly IEnumerable<BO.EngineerInTask> s_taskInEng = BlApi.Factory.Get().Engineer.AllEngineerInTask();
    public IEnumerator GetEnumerator() => s_taskInEng.GetEnumerator();
}
