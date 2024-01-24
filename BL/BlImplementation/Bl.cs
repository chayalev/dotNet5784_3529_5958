namespace BlImplementation;
using BlApi;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public ITask Task => new TaskImplementation();

}
