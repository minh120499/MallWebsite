using Backend.Model;
using Backend.Model.Entities;
using Backend.Repository;

namespace Backend.Service;

public class EmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public async Task<List<Employee>> GetByFilter(FilterModel filters)
    {
        var users = await _employeesRepository.GetByFilter(filters);
        return users;
    }
}