using Backend.Model;
using Backend.Model.Entities;

namespace Backend.Repository;

public interface IEmployeesRepository
{
    public Task<List<Employee>> GetByFilter(FilterModel filters);
    
    public Task<Employee> Add(Employee employee);
    public Task<int> Count();
}