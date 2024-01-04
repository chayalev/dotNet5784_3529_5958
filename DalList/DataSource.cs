

namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        
        //id for Task entity
        internal const int startTaskId = 1;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        // id for Dependency entity
        internal const int startDependencyId = 1;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        //start date of project
        internal static DateTime? startProject = null;

        //end date of project
        internal static DateTime? endProject = null;

    }

    // Lists that will contain all references to entities of the same type.
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();

}
