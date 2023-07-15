using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService _ordersService;

        public OrdersController(OrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
        {
            if (filters.Page < 1)
            {
                throw new FormValidationException("Page must be greater than 0");
            }

            var orders = await _ordersService.GetByFilter(filters);
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var order = await _ordersService.GetById(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            var response = await _ordersService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrderRequest request)
        {
            var response = await _ordersService.Update(id, request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string ids)
        {
            await _ordersService.Delete(ids);
            return Ok(new SuccessResponse());
        }
    }
}