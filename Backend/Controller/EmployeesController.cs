using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
        {
            if (filters.Page < 1)
            {
                throw new FormValidationException("Page must be greater than 0");
            }

            var employees = await _employeesService.GetByFilter(filters);
            return Ok(employees);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var employee = await _employeesService.GetById(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeRequest request)
        {
            var response = await _employeesService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] EmployeeRequest request)
        {
            var response = await _employeesService.Update(id, request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string ids)
        {
            await _employeesService.Delete(ids);
            return Ok(new SuccessResponse());
        }
    }
}