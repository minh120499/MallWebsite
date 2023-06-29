using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IEmployeesRepository
{
    public Task<List<Employees>> GetByFilter(FilterModel filters);
}