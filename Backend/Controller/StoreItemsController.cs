using Backend.Model;
using Backend.Model.Request;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/products")]
public class StoreItemsController : ControllerBase
{
    private readonly StoreItemsService _storeItemsService;

    public StoreItemsController(StoreItemsService storeItemsService)
    {
        _storeItemsService = storeItemsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
    {
        var storeItems = await _storeItemsService.GetByFilter(filters);
        return Ok(storeItems);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StoreItemRequest request)
    {
        var response = await _storeItemsService.Create(request);
        return Ok(response);
    }
}