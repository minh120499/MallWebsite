using Backend.Model;
using Backend.Model.Request;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/stores")]
public class StoresController : ControllerBase
{
    private readonly StoresService _storeService;
    private readonly StoreProductsService _storeProductsService;

    public StoresController(StoresService storeService, StoreProductsService storeProductsService)
    {
        _storeService = storeService;
        _storeProductsService = storeProductsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
    {
        var store = await _storeService.GetByFilter(filters);
        return Ok(store);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StoreRequest request)
    {
        var response = await _storeService.Create(request);
        return Ok(response);
    }

    [HttpGet("{id:int}/products")]
    public async Task<IActionResult> GetProducts(int id, [FromQuery] FilterModel filters)
    {
        var storeItems = await _storeProductsService.GetByFilter(filters);
        return Ok(storeItems);
    }

    [HttpPost("{id:int}/products")]
    public async Task<IActionResult> AddProduct(int id, [FromBody] StoreProductRequest request)
    {
        var response = await _storeProductsService.Create(request);
        return Ok(response);
    }
}