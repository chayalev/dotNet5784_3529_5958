using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// help entity-TaskInList
/// </summary>
/// <param name="Id">The ID of the TaskInList</param>
/// <param name="Descruption">description of the TaskInList</param>
/// <param name="Alias">The alias of the TaskInList</param>
/// <param name="Status">The status of the TaskInList work</param>
public class TaskInList
{
    public int Id { get; init; }
    public string? Descruption { get; set; }
    public string? Alias { get; set; }
    public Status? Status { get; set; }
}

