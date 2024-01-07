using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public IDependency Dependency { get; }
    public ITask Task { get; }

}

