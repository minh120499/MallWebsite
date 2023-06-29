using Backend.Model;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
    {
        var a = await _usersService.GetByFilter(filters);
        return Ok(a);
    }
}