

namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer eng)
    {
        DO.Engineer doEngineer = new DO.Engineer
          (eng.Id, eng.Name, (DO.EngineerExperience?)eng.Level, eng.Email, eng.Cost);
        try
        {
            int idStud = _dal.Engineer.Create(doEngineer);
            return idStud;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={eng.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Engineer? boEng = this.Read(id);
        if (boEng?.Task != null)
            throw new DalDeletionImpossible("Engineer in the middle of task");
        if (_dal.Engineer.Read(id) == null)
            throw new DalDoesNotExistException($"Engingineer ID = {id} does not exist");
        else
            _dal.Engineer.Delete(id);

    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience?)doEngineer.Level,
            Cost = doEngineer.Cost,
        };
    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Email = doEngineer.Email,
                    Level = (BO.EngineerExperience?)doEngineer.Level,
                    Cost = doEngineer.Cost,
                });

    }

    public void Update(BO.Engineer eng)
    {
        DO.Engineer doEngineer = new DO.Engineer
          (eng.Id, eng.Name, (DO.EngineerExperience?)eng.Level, eng.Email, eng.Cost);
        try
        {
            _dal.Engineer.Update(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Student with ID={eng.Id} already exists", ex);
        }
    }
}

