using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
///Main logical entity-Engineer
/// </summary>
/// <param name="Id">The ID of the engineer</param>
/// <param name="Name">Full name of engineer</param>
/// <param name="Email">The email of the engineer</param>
/// <param name="Level">The level of the engineer work</param>
/// <param name="Cost">cost per hour</param>
/// <param name="Task">the current task of the engineer</param>

public class Engineer
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public EngineerExperience Level { get; set; }
    public double? Cost { get; set; }
    public TaskInEngineer? Task { get; set; }
}

