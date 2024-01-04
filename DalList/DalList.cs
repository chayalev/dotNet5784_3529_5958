﻿namespace Dal;
using DalApi;
using System;

sealed public class DalList : IDal
{
    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;

    public void Reset()
    {
        Engineer.Reset();   
        Dependency.Reset();
        Task.Reset();
    }

 
}
