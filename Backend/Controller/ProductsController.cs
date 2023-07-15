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
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsService _productsService;

        public ProductsController(ProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
        {
            if (filters.Page < 1)
            {
                throw new FormValidationException("Page must be greater than 0");
            }

            var products = await _productsService.GetByFilter(filters);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _productsService.GetById(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequest request)
        {
            var response = await _productsService.Create(request);
            return Ok(response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductRequest request)
        {
            var response = await _productsService.Update(id, request);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string ids)
        {
            await _productsService.Delete(ids);
            return Ok(new SuccessResponse());
        }
    }
}