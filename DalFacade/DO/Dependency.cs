
namespace DO;
/// <summary>
///  Dependency Entity represents a dependence with all its props
/// </summary>
/// <param name="Id"></param>
/// <param name="DependentTask"></param>
/// <param name="DependsOnTask"></param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
);
