using Backend.Model;
using Backend.Model.Request;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/[controller]")]
public class StoresController : ControllerBase
{
    private readonly StoresService _storeService;

    public StoresController(StoresService storeService)
    {
        _storeService = storeService;
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
}