using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Entities;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/banners")]
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
        if (filters.Page < 1)
        {
            throw new FormValidationException("Page must be greater than 0");
        }
        var banners = await _bannersService.GetByFilter(filters);
        return Ok(banners);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BannerRequest request)
    {
        var response = await _bannersService.Create(request);
        return Ok(response);
    }
}