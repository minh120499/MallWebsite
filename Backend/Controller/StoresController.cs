using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controller
{
    [ApiController]
    [Route("api/stores")]
    public class StoresController : ControllerBase
    {
        private readonly StoresService _storesService;

        public StoresController(StoresService storesService)
        {
            _storesService = storesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
        {
            if (filters.Page < 1)
            {
                throw new FormValidationException("Page must be greater than 0");
            }

            var stores = await _storesService.GetByFilter(filters);
            return Ok(stores);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var store = await _storesService.GetById(id);
            return Ok(store);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoreRequest request)
        {
            var response = await _storesService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StoreRequest request)
        {
            var response = await _storesService.Update(id, request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string ids)
        {
            await _storesService.Delete(ids);
            return Ok(new SuccessResponse());
        }
    }
}