using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IMilestone
{
    public int Create();
    public BO.Milestone? Read(int id);
    public void Update(BO.Milestone item);
}
