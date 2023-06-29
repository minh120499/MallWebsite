using Backend.Model;
using Backend.Model.Entities;
using Backend.Repository;

namespace Backend.Service;

public class UsersService
{
    private readonly IEmployeesRepository _employeesRepository;

    public UsersService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public async Task<List<Employees>> GetByFilter(FilterModel filters)
    {
        var users = await _employeesRepository.GetByFilter(filters);
        return users;
    }
}