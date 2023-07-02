using Backend.Model;
using Backend.Model.Entities;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/[controller]")]
public class BannersController : ControllerBase
{
    private readonly BannersService _bannersService;

    public BannersController(BannersService bannersService)
    {
        _bannersService = bannersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
    {
        var banners = await _bannersService.GetByFilter(filters);
        return Ok(banners);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Banner banner)
    {
        var response = await _bannersService.Create(banner);
        return Ok(response);
    }
}