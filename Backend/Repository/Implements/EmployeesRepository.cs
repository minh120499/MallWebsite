using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Implements
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetById(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                throw new NotFoundException("Employee not found.");
            }

            return employee;
        }

        public async Task<List<Employee>> GetByFilter(FilterModel filters)
        {
            var employees = await _context.Employees
                .Where(u => u.FullName.Contains(filters.Query ?? ""))
                .OrderByDescending(u => u.Id)
                .Skip((filters.Page - 1) * filters.Limit)
                .Take(filters.Limit)
                .Reverse()
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

        public async Task<Employee> Update(int employeeId, EmployeeRequest request)
        {
            try
            {
                var employee = await GetById(employeeId);

                employee.Phone = request.Phone;
                employee.FullName = request.FullName;
                employee.Address = request.Address;
                employee.StoreId = request.StoreId;
                employee.Email = request.Email;
                employee.Store = request.Store;
                employee.Status = request.Status;
                employee.ModifiedOn = DateTime.Now;
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

        public async Task<bool> Delete(List<string> ids)
        {
            try
            {
                var employees = await _context.Employees.Where(e => ids.Contains(e.Id)).ToListAsync();
                _context.Employees.RemoveRange(employees);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
