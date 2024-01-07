

namespace BlImplementation;
using BlApi;
using BLApi;
using BO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Engineer eng)
    {
      

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Engineer eng)
    {
        throw new NotImplementedException();
    }
}

