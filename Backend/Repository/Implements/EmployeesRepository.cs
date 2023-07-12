using Backend.Model;
using Backend.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements;

public class EmployeesRepository : IEmployeesRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetByFilter(FilterModel filters)
    {
        var employees = await _context.Employees!
            .Where(u => u.FullName.Contains(filters.Query ?? ""))
            .OrderBy(u => u.Id)
            .Skip((filters.Page - 1) * filters.Limit)
            .Take(filters.Limit)
            .ToListAsync();
        return employees;
    }

    public async Task<Employee> Add(Employee employee)
    {
        try
        {
            employee.CreateOn = DateTime.Now;
            employee.ModifiedOn = DateTime.Now;
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Count()
    {
        return await _context.Employees.CountAsync();
    }
}