using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

/// <summary>
///  help entity-MilestoneInList
/// </summary>
/// /// <param name="Id">The ID of the MilestoneInTask</param>
/// <param name="Description">Description</param>
/// <param name="Alias">the alias of the MilestoneInTask</param>
/// <param name="Status">the status of the MilestoneInTask</param>
/// <param name="CompletionPercentage">progress percentage</param>
public class MilestoneInList
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public BO.Status? Status { get; set; }
    public double? CompletionPercentage { get; set; }
}
