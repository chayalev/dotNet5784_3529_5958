using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
/// help entity-EngineerInTask
/// </summary>
/// <param name="Id">The ID of the Engineer</param>
/// <param name="Name">the Name of the Engineer</param>
public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }
}
