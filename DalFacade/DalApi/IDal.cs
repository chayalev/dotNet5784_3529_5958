﻿
using System;
using System.Data;

namespace DalApi
{
    public interface IDal
    {
        IDependency Dependency { get; }
        IEngineer Engineer { get; }
        ITask Task { get; }
        void Reset();
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }


    }
}
