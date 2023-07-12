using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly FacilitiesService _facilitiesService;
    private readonly FloorsService _floorsService;

    public SettingsController(FloorsService floorsService, FacilitiesService facilitiesService)
    {
        _floorsService = floorsService;
        _facilitiesService = facilitiesService;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var filters = new FilterModel();

        var facilities = await _facilitiesService.GetByFilter(filters);
        var floors = await _floorsService.GetByFilter(filters);
        return Ok(new SettingsResponse()
        {
            Facilities = facilities,
            Floors = floors
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SettingsRequest request)
    {
        var facilities = await _facilitiesService.Update(request.Facilities);
        var floors = await _floorsService.Update(request.Floors);
        return Ok(new SettingsResponse()
        {
            Facilities = facilities,
            Floors = floors
        });
    }
}