using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Repository;
using Backend.Utils;

namespace Backend.Service
{
    public class EmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;

        public EmployeesService(IEmployeesRepository employeesRepository)
        {
            _employeesRepository = employeesRepository;
        }

        public async Task<Employee> GetById(int employeeId)
        {
            return await _employeesRepository.GetById(employeeId);
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
                Phone = request.Phone,
                FullName = request.FullName,
                Address = request.Address,
                Email = request.Email,
                StoreId = request.StoreId,
                Store = request.Store,
                Status = request.Status,
            };
            return await _employeesRepository.Add(employee);
        }

        public async Task<Employee> Update(int employeeId, EmployeeRequest request)
        {
            Validations.Employee(request);

            return await _employeesRepository.Update(employeeId, request);
        }

        public async Task<bool> Delete(string ids)
        {
            var employeeIds = ids.Split(',').Select(int.Parse).ToList();

            return await _employeesRepository.Delete(ids.Split(",").ToList());
        }
    }
}