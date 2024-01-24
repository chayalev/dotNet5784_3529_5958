using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// help entity-TaskInEngineer
/// </summary>
/// <param name="Id">The ID of the task</param>
/// <param name="Alias">the alias of the TaskInEngineer</param>

public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString() => Tools.ToStringProperty(this);
}

