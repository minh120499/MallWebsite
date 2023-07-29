using Backend.Exceptions;
using Backend.Model;
using Backend.Model.Request;
using Backend.Model.Response;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterModel filters)
    {
        if (filters.Page < 1)
        {
            throw new FormValidationException("Page must be greater than 0");
        }

        var categories = await _categoriesService.GetByFilter(filters);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var category = await _categoriesService.GetById(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CategoryRequest request)
    {
        var response = await _categoriesService.Create(request);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CategoryRequest request)
    {
        var response = await _categoriesService.Update(id, request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string ids)
    {
        await _categoriesService.Delete(ids);
        return Ok(new SuccessResponse());
    }
}