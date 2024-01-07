using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? alias { get; set; }
    public datetime? CreateAtDate { get; set; }
    public BO.Statut Status { get; set; }
    public List<BO.TaskInList> Dependencies { get; set; }
    public datetime LinkMilestone { get; set; }
    public datetime BaselineStartDate { get; set; }
    public datetime StartDate { get; set; }
    public datetime ForecastDate { get; set; }
    public datetime DeadlineDate { get; set; }
    public datetime ComleteDate { get; set; }
    public string Deliverables { get; set; }
    public string Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience ComplexityLevel { get; set; }
}
