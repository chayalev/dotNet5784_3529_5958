

namespace DO;
/// <summary>
/// Engineer Entity represents an engineer with all its props
/// </summary>
/// <param name="Id">The ID of the engineer</param>
/// <param name="Name">Full name of engineer</param>
/// <param name="Level">The level of the engineer work</param>
/// <param name="Email">The email of the engineer</param>
/// <param name="Cost">cost per hour</param>
public record Engineer
(
    int Id,
    string? Name=null,
    EngineerExperience? Level=null,
    string? Email = null,
    double? Cost = null
)
{
 public Engineer() : this(0) { }
 }