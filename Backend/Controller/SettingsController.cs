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
    
    [HttpPut("facilities/{id:int}")]
    public async Task<IActionResult> UpdateFacilities([FromRoute] int id, [FromBody] FacilityRequest request)
    {
        var response = await _facilitiesService.Update(id, request);
        return Ok(response);
    }
    
    [HttpPut("floors/{id:int}")]
    public async Task<IActionResult> UpdateFloors([FromRoute] int id, [FromBody] FloorRequest request)
    {
        var response = await _floorsService.Update(id, request);
        return Ok(response);
    }

    [HttpDelete("facilities")]
    public async Task<IActionResult> DeleteFacilities([FromQuery] string ids)
    {
        await _facilitiesService.Delete(ids);
        return Ok(new SuccessResponse());
    }

    [HttpDelete("floors")]
    public async Task<IActionResult> DeleteFloors([FromQuery] string ids)
    {
        await _floorsService.Delete(ids);
        return Ok(new SuccessResponse());
    }
}