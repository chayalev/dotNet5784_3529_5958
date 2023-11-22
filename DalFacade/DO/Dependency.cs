
namespace DO;
/// <summary>
///  Dependency Entity represents a dependence with all its props
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="DependentTask">ID number of pending task</param>
/// <param name="DependsOnTask">Previous assignment ID number</param>
public record Dependency
(
    int Id,
    int? DependentTask=null,
    int? DependsOnTask=null
)
{
    public Dependency(): this(0){}
}
