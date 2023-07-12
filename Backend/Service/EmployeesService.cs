using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service;

public class EmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public async Task<TableListResponse<Employee>> GetByFilter(FilterModel filters)
    {
        var employees = await _employeesRepository.GetByFilter(filters);
        var total = await _employeesRepository.Count();
        return new TableListResponse<Employee>()
        {
            Total = total,
            Limit = filters.Limit,
            Page = filters.Page,
            Data = employees
        };
    }

    public async Task<Employee> Create(EmployeeRequest request)
    {
        Validations.Employee(request);

        var employee = new Employee()
        {
            Status = StatusConstraint.ACTIVE,
        };
        return await _employeesRepository.Add(employee);
    }
}