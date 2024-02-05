namespace BlImplementation;
using BlApi;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? startDate { get; set; } =null;
    public DateTime? EndDate { get; set; } = null;
    public bool IsCreate { get; set; } = false;

    public void Reset()
    {
       DalApi.Factory.Get.Reset();
    }
}
