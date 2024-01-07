using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;
/// <summary>
/// help entity-MilestoneInTask
/// </summary>
/// <param name="Id">The ID of the MilestoneInTask</param>
/// <param name="Alias">the alias of the MilestoneInTask</param>
/// <param name="CreatedAt">the date of creation the MilestoneInTask</param>
/// <param name="Status">the status of the MilestoneInTask</param>
/// <param name="ProgressPercent">the percent of the progress of the MilestoneInTask</param>
public class MilestoneInTask
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public Status? Status { get; set; }
    public float? ProgressPercent {  get; set; }
}

