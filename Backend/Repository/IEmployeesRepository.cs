using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;

namespace Backend.Repository;

public interface IEmployeesRepository
{
    Task<Employee> GetById(int employeeId);
    Task<List<Employee>> GetByFilter(FilterModel filters);
    Task<Employee> Add(Employee employee);
    Task<Employee> Update(int employeeId, EmployeeRequest request);
    Task<int> Count();
    Task<bool> Delete(List<string> ids);
}