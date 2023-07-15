using Backend.Model;
using Backend.Model.Entities;
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
        var response = new SettingsResponse
        {
            Facilities = null,
            Floors = null
        };

        var facilities = await _facilitiesService.GetByFilter(filters);
        var floors = await _floorsService.GetByFilter(filters);
        response.Facilities = facilities.Data;
        response.Floors = floors.Data;
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SettingsRequest request)
    {
        var response = new SettingsResponse
        {
            Facilities = null,
            Floors = null
        };

        if (request.Facilities.Count > 0)
        {
            var facilities = await _facilitiesService.Update(request.Facilities);
            response.Facilities = facilities;
        }

        if (request.Floors.Count > 0)
        {
            var floors = await _floorsService.Update(request.Floors);
            response.Floors = floors;
        }

        return Ok(response);
    }
}