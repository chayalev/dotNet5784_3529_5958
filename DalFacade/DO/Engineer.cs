

namespace DO;
/// <summary>
/// Engineer Entity represents an engineer with all its props
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Level"></param>
/// <param name="Email"></param>
/// <param name="Cost"></param>
public record Engineer
(
    int Id,
    string Name,
    publicEngineerExperience Level,
    string? Email = null,
    double? Cost = null
);