using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Engineer
    {
        public int Id {  get; init; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public enum EngineerExperience { Novice, AdvancedBeginner, Competent, Proficient, Expert };
        public double? Cost { get; set; }
        public TaskInEngineer? Task {  get; set; }
    }
}
